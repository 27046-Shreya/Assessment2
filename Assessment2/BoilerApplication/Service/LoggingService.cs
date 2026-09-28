using System.Runtime.CompilerServices;
using BoilerApplication.EventArguments;
using BoilerApplication.Repository;
using BoilerApplication.Model;
using BoilerApplication.Enums;

namespace BoilerApplication.Service;

/// <summary>
/// Handles the logging operation of boiler event logs.
/// </summary>
public class LoggingService
{
    private readonly BoilerService _boilerService;
    private readonly LogRepository _logRepository;

    public LoggingService(BoilerService boilerService, LogRepository logRepository)
    {
        this._boilerService = boilerService;
        this._logRepository = logRepository;
    }

    public void SubscribeLogging()
    {
        this._boilerService.BoilerEventHandler += LogEvent;
    }

    public void UnsubscribeLogging()
    {
        this._boilerService.BoilerEventHandler -= LogEvent;
    }

    public string GetLogs()
    {
        List<string> logs = this._logRepository.ReadLogs();
        string log = string.Join("\n ", logs);
        return log;
    }

    private void LogEvent(object? sender, BoilerEventArgs boilerEvent)
    {
        if (boilerEvent.State == BoilerState.Lockout)
        {
            this._logRepository.WriteLog(new BoilerEventLog(LogType.WARNING, boilerEvent.State, boilerEvent.ToggleState, boilerEvent.EventDescription,DateTime.Now));
        }
        else
        {
            this._logRepository.WriteLog(new BoilerEventLog(LogType.INFO, boilerEvent.State, boilerEvent.ToggleState,boilerEvent.EventDescription, DateTime.Now));
        }
    }

}

