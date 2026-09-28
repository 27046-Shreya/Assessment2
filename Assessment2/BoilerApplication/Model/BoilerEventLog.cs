using BoilerApplication.Enums;

namespace BoilerApplication.Model;

/// <summary>
/// Defines the properties of boiler event log.
/// </summary>
public class BoilerEventLog
{
    public BoilerEventLog(LogType logType, BoilerState boilerState, ToggleState toggleState,string EventName, DateTime timeStamp)
    {
        this.Type = logType;
        this.State = boilerState;
        this.ToggleState = toggleState;
        this.EventName = EventName;
        this.TimeStamp = timeStamp;
    }

    public LogType Type { get; set; }
    public BoilerState State { get; set; }

    public ToggleState ToggleState { get; set; }

    public string EventName { get; set; }

    public DateTime TimeStamp { get; set; }
}
