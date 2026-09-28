namespace BoilerApplication.Enums;

/// <summary>
/// Represents the states of the boiler machine.
/// </summary>
public enum BoilerState
{
    /// <summary>
    /// Represents the lockout state of the boiler where it cannot perform any operation.
    /// </summary>
    Lockout,

    /// <summary>
    /// Represents that the boiler is ready to start.
    /// </summary>
    Ready,

    /// <summary>
    /// Represents that the boiler is in pre-purge state.
    /// </summary>
    PrePurge,

    /// <summary>
    /// Represents that the boiler is igniting.
    /// </summary>
    Ignition,

    /// <summary>
    /// Represent that the boiler is in operational state.
    /// </summary>
    Operational,
}
