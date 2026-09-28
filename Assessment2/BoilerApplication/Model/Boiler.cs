using BoilerApplication.Enums;

namespace BoilerApplication.Model;

/// <summary>
/// Defines the properties of a boiler machine.
/// </summary>
public class Boiler
{
    /// <summary>
    /// Initializes the properties of the boiler machine.
    /// </summary>
    /// <param name="id">Identifier of the machine.</param>
    /// <param name="boilerState">BoilerState of the boiler.</param>
    /// <param name="toggleState">Toggle state of the boiler</param>
    public Boiler(Guid id)
    {
        this.BoilerID = id;
        this.BoilerState = boilerState;
        this.ToggleState = toggleState;
    }
    public Guid BoilerID { get; init; }

    public BoilerState BoilerState { get; set; }

    public ToggleState ToggleState { get; set; }
}
