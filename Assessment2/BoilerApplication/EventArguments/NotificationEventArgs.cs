namespace BoilerApplication.EventArguments;

/// <summary>
/// Defines the arguments or data to be passed across events
/// </summary>
public class NotificationEventArgs : EventArgs
{
    public NotificationEventArgs(string message, DateTime time)
    {
        this.Message = message;
        this.Time = time;
    }
    public string Message { get; }

    public DateTime Time { get; }

}
