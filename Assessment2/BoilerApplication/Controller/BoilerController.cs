using System.Security.Cryptography.X509Certificates;
using BoilerApplication.Enums;
using BoilerApplication.EventArguments;
using BoilerApplication.Service;
using BoilerApplication.View;

namespace BoilerApplication.Controller;

public class BoilerController
{
    private readonly BoilerService _boilerService;
    private readonly NotificationService _notificationService;
    private readonly LoggingService _loggingService;
    private readonly BoilerDashboardView _view;


    public BoilerController(BoilerService boilerService, NotificationService notificationService, LoggingService loggingService, BoilerDashboardView view)
    {
        this._boilerService = boilerService;
        this._notificationService = notificationService;
        this._notificationService.NotificationEventHandler += NotifyUser;
        this._loggingService = loggingService;
        this._view = view;
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
            this._view.DisplayUserMenu();
            string? input = Console.ReadLine();
            this._view.ClearInputLine();
            try
            {
                switch (input)
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
                    default:
                        _view.DisplayErrorMessage("Invalid option");
                        break;
                }
            }
            catch (OperationCanceledException)
            {
                _view.DisplayErrorMessage("Application stops.");
            }
        }
        
    }

    public void DisplayEventLogs()
    {
        string logs = _loggingService.GetLogs();
        _view.DisplayEventLogs(logs);
    }

    public void NotifyUser(object? sender, NotificationEventArgs e)
    {
        _view.AddMessage(e.Message);
    }
}
