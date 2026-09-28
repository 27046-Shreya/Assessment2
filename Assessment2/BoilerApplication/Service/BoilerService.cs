using BoilerApplication.EventArguments;
using BoilerApplication.Model;
using BoilerApplication.Enums;

namespace BoilerApplication.Service;

/// <summary>
/// Handles the operations performed by the boiler and manages states
/// </summary>
public class BoilerService
{
    public event EventHandler<BoilerEventArgs>? BoilerEventHandler;
    private readonly Boiler _boiler;
    private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
    private object _lock = new object();
    private bool _isReady = false;
    private bool _canStart = false;

    public BoilerService(Boiler boiler)
    {
        this._boiler = boiler;
    }

    public void Initialize()
    {
        lock (_lock)
        {
            _boiler.BoilerState = BoilerState.Lockout;
            _boiler.ToggleState = ToggleState.Open;
        }

        BoilerEventHandler?.Invoke(this, new BoilerEventArgs(_boiler.BoilerState, _boiler.ToggleState, "Boiler Controller Initialized"));
    }

    public void ResetInterLock()
    {
        string message = string.Empty;
        lock (_lock)
        {
            if (_boiler.BoilerState == BoilerState.Lockout && _boiler.ToggleState == ToggleState.Close)
            {
                _boiler.BoilerState = BoilerState.Ready;
                message = $"Boiler Status changed to {BoilerState.Ready}";
            }
            else if (_boiler.BoilerState != BoilerState.Lockout)
            {
                _boiler.BoilerState = BoilerState.Lockout;
                _boiler.ToggleState = ToggleState.Close;
                message = $"Boiler status changed to {BoilerState.Lockout} ToggleState: {ToggleState.Close}";
            }  
        }

        BoilerEventHandler?.Invoke(this, new BoilerEventArgs(_boiler.BoilerState, _boiler.ToggleState, message));
    }

    public void Toggle()
    {
        lock (_lock)
        {
            _boiler.ToggleState = _boiler.ToggleState == ToggleState.Open ? ToggleState.Close : ToggleState.Open;
        }

        BoilerEventHandler?.Invoke(this, new BoilerEventArgs(_boiler.BoilerState, _boiler.ToggleState, $"Interlock Switch toggled to {_boiler.ToggleState}"));
    }

    public bool CheckReadyStatus()
    {
        if (_boiler.BoilerState == BoilerState.Ready && _boiler.ToggleState == ToggleState.Close)
        {
            return true;
        }
        return false;
    }

    public void StartBoilerSequence()
    {
        lock (_lock)
        {
            if (_isReady)
            {
                _canStart = true;
            }
        }
    }

    public void SimulateError()
    {
        lock (_lock)
        {
            _boiler.BoilerState = BoilerState.Lockout;
            _boiler.ToggleState = ToggleState.Close;
        }
        BoilerEventHandler?.Invoke(this, new BoilerEventArgs(_boiler.BoilerState, _boiler.ToggleState, $"Error:Try to simulate error. System in Lockout"));
    }

    public async Task StartBoiler()
    {
        await ProcessBoilerAsync(new CancellationTokenSource());
    }

    private async Task ProcessBoilerAsync(CancellationTokenSource cancellationTokenSource)
    {
        PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromSeconds(1));
        try
        {
            while (await timer.WaitForNextTickAsync(cancellationTokenSource.Token))
            {
                if (_canStart)
                {
                    await PerformBoilerSequence(cancellationTokenSource.Token);
                }
            }
        }
        catch (OperationCanceledException)
        {
            BoilerEventHandler?.Invoke(this, new BoilerEventArgs(_boiler.BoilerState, _boiler.ToggleState, "Operation stopped! Changing the boiler state from Pre-Purge to ready"));
        }

    }

    private async Task PerformBoilerSequence(CancellationToken cancellationToken)
    {
        if (!cancellationToken.IsCancellationRequested)
        {
            lock (_lock)
            {
                _boiler.BoilerState = BoilerState.PrePurge;
            }
            await PerformPrePurge(cancellationToken);
            BoilerEventHandler?.Invoke(this, new BoilerEventArgs(_boiler.BoilerState, _boiler.ToggleState, "Pre-Purge completed"));

            lock (_lock)
            {
                _boiler.BoilerState = BoilerState.Ignition;
            }
            await PerformIgnition(cancellationToken);
            BoilerEventHandler?.Invoke(this, new BoilerEventArgs(_boiler.BoilerState, _boiler.ToggleState, "Ignition phase completed"));

            lock(_lock)
            {
                _boiler.BoilerState = BoilerState.Operational;
            }

            BoilerEventHandler?.Invoke(this, new BoilerEventArgs(_boiler.BoilerState, _boiler.ToggleState, "Boiler now operational"));
        }
        else
        {
            lock(_lock)
            {
                _boiler.BoilerState = BoilerState.Ready;
            }
        }
    }

    private async Task PerformPrePurge(CancellationToken cancellationToken)
    {
        try
        {
            await Task.Delay(10000, cancellationToken);
        }
        catch(OperationCanceledException)
        {
            lock(_lock)
            {
                _boiler.BoilerState = BoilerState.Ready;
            }

            BoilerEventHandler?.Invoke(this, new BoilerEventArgs(_boiler.BoilerState, _boiler.ToggleState, "Operation stopped! Changing the boiler state from Pre-Purge to ready"));
        }
        
    }

    private async Task PerformIgnition(CancellationToken cancellationToken)
    {
        try
        {
            await Task.Delay(10000, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            lock (_lock)
            {
                _boiler.BoilerState = BoilerState.Ready;
            }

            BoilerEventHandler?.Invoke(this, new BoilerEventArgs(_boiler.BoilerState, _boiler.ToggleState, "Operation stopped! Changing the boiler state from Ignition to ready"));
        }

    }

}
