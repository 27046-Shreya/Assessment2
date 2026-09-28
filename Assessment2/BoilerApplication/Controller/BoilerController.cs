using System.Security.Cryptography.X509Certificates;
using BoilerApplication.Enums;
using BoilerApplication.EventArguments;
using BoilerApplication.Service;

namespace BoilerApplication.Controller;

public class BoilerController
{
    private readonly BoilerService _boilerService;
    private readonly NotificationService _notificationService;
    private readonly LoggingService _loggingService;


    public BoilerController(BoilerService boilerService, NotificationService notificationService, LoggingService loggingService)
    {
        this._boilerService = boilerService;
        this._notificationService = notificationService;
        this._notificationService.NotificationEventHandler += NotifyUser;
        this._loggingService = loggingService;
    }

    public void Initialize()
    {
        _boilerService.Initialize();
        Task.Run(() => _boilerService.StartBoiler(new CancellationTokenSource()));
    }

    public void HandleUserMenu()
    {
        while (true)
        {
            Console.WriteLine("Enter input:");
            string? input = Console.ReadLine();
            switch(input)
            {
                case "1":
                    _boilerService.StartBoilerSequence();
                    break;
                case "2":
                    _boilerService.Stop();
                    break;
                case "3":
                    _boilerService.SimulateError();
                    break;
                case "4":
                    _boilerService.Toggle();
                    break;
                case "5":
                    _boilerService.ResetLockout();
                    break;
                case "6":
                    DisplayEventLogs();
                    break;
                case "7":
                    return;
            }
        }
        
    }

    public void DisplayEventLogs()
    {
        string logs = _loggingService.GetLogs();
    }

    public void NotifyUser(object? sender, NotificationEventArgs e)
    {
        Console.WriteLine(e.Message);
    }
}
