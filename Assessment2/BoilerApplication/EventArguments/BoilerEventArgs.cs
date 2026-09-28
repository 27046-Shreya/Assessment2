using BoilerApplication.Enums;

namespace BoilerApplication.EventArguments;

/// <summary>
/// Defines the arguments or data to be flowed across events.
/// </summary>
public class BoilerEventArgs : EventArgs
{
    public BoilerEventArgs(BoilerState boilerState, ToggleState toggleState, string eventName)
    {
        this.State = boilerState;
        this.ToggleState = toggleState;
        this.EventDescription = eventName;
    }

    public BoilerState State { get; }
    public ToggleState ToggleState { get; }

    public string EventDescription { get; }
}
