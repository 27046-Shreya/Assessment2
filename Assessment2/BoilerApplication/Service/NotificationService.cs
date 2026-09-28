using BoilerApplication.EventArguments;

namespace BoilerApplication.Service;

/// <summary>
/// Handles sending notifications to the user.
/// </summary>
public class NotificationService
{
    private readonly BoilerService boilerService;
    public event EventHandler<NotificationEventArgs>? NotificationEventHandler;

    public NotificationService(BoilerService boilerService)
    {
        this.boilerService = boilerService;
    }

    public void SubscribeNotification()
    {
        this.boilerService.BoilerEventHandler += NotifyUser;
    }

    public void UnsubscribeNotification()
    {
        this.boilerService.BoilerEventHandler -= NotifyUser;
    }

    private void NotifyUser(object? sender, BoilerEventArgs boilerEvent)
    {
        NotificationEventHandler?.Invoke(this, new NotificationEventArgs(boilerEvent.EventDescription, DateTime.Now));
    }
}
