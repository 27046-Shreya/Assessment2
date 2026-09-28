using BoilerApplication.EventArguments;
using BoilerApplication.Model;
using BoilerApplication.Enums;
using System.Runtime.CompilerServices;

namespace BoilerApplication.Service;

/// <summary>
/// Handles the operations performed by the boiler and manages states
/// </summary>
public class BoilerService
{
    public event EventHandler<BoilerEventArgs>? BoilerEventHandler;
    private readonly Boiler _boiler;
    private CancellationTokenSource? _currentCancelToken;
    private object _lock = new object();
    private bool _isReady = false;
    private bool _canStart = false;
    private bool _isRunning = true;

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
        BoilerEventHandler?.Invoke(this, new BoilerEventArgs(_boiler.BoilerState, _boiler.ToggleState, "Toggle to reach ready state"));
    }

    public void ResetLockout()
    {
        string message = string.Empty;
        lock (_lock)
        {
            if (_boiler.BoilerState == BoilerState.Lockout && _boiler.ToggleState == ToggleState.Open)
            {
                message = "Toggle to reset the lockout and reach ready state";
            }
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
            if (_boiler.BoilerState == BoilerState.Lockout && _boiler.ToggleState == ToggleState.Open)
            {
                BoilerEventHandler?.Invoke(this, new BoilerEventArgs(_boiler.BoilerState, _boiler.ToggleState, $"In {BoilerState.Lockout} state, ToggleState: {ToggleState.Open} Toggle to close and reset."));
            }
            else if (_boiler.BoilerState == BoilerState.Lockout && _boiler.ToggleState == ToggleState.Close)
            {
                BoilerEventHandler?.Invoke(this, new BoilerEventArgs(_boiler.BoilerState, _boiler.ToggleState, $"In {BoilerState.Lockout} state, Reset to attain ready and then start the sequence."));
            }

            if (_isReady)
            {
                _canStart = true;
            }
        }
    }

    public void SimulateError()
    {
        if(_boiler.BoilerState == BoilerState.Operational)
        {
            lock (_lock)
            {
                _boiler.BoilerState = BoilerState.Lockout;
                _boiler.ToggleState = ToggleState.Close;
            }
            BoilerEventHandler?.Invoke(this, new BoilerEventArgs(_boiler.BoilerState, _boiler.ToggleState, $"Error:Try to simulate error. System in Lockout"));
        }
        else
        {
            BoilerEventHandler?.Invoke(this, new BoilerEventArgs(_boiler.BoilerState, _boiler.ToggleState, $"Simulated is disabled at {_boiler.BoilerState}"));
        }
        
    }

    public void Stop()
    {
        lock (_lock)
        {
            if(_boiler.BoilerState != BoilerState.Ready || _boiler.BoilerState != BoilerState.Lockout)
            {
                if(_currentCancelToken  != null)
                {
                    _currentCancelToken.Cancel();
                }
                
                _boiler.BoilerState = BoilerState.Ready;
            }
            else
            {
                BoilerEventHandler?.Invoke(this, new BoilerEventArgs(_boiler.BoilerState, _boiler.ToggleState, $"Process have not started. Press start to stop the application."));
            }
        }
    }

    public async Task StartBoiler(CancellationTokenSource cancellationTokenSource)
    {
        _currentCancelToken = cancellationTokenSource;
        await ProcessBoilerAsync(cancellationTokenSource);
    }

    private async Task ProcessBoilerAsync(CancellationTokenSource cancellationTokenSource)
    {
        PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromSeconds(1));
        try
        {
            if(_isRunning)
            {
                while (await timer.WaitForNextTickAsync(cancellationTokenSource.Token))
                {
                    lock (_lock)
                    {
                        if (_boiler.BoilerState == BoilerState.Ready && _boiler.ToggleState == ToggleState.Close)
                        {
                            _isReady = true;
                        }
                    }

                    if (_canStart)
                    {
                        await PerformBoilerSequence(cancellationTokenSource.Token);
                        _canStart = false;
                    }
                }
            }
            else
            {
                throw new OperationCanceledException();
            }
            
        }
        catch (OperationCanceledException ex)
        {
            BoilerEventHandler?.Invoke(this, new BoilerEventArgs(_boiler.BoilerState, _boiler.ToggleState, ex.Message));
            Restart();
            _canStart = false;

        }

    }

    public void Restart()
    {
        Task.Run(() => StartBoiler(new CancellationTokenSource()));
    }

    private async Task PerformBoilerSequence(CancellationToken cancellationToken)
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


        lock (_lock)
        {
            _boiler.BoilerState = BoilerState.Operational;
        }

        BoilerEventHandler?.Invoke(this, new BoilerEventArgs(_boiler.BoilerState, _boiler.ToggleState, "Boiler now operational"));


    }

    private async Task PerformPrePurge(CancellationToken cancellationToken)
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
            throw new OperationCanceledException($"Operation stopped at {BoilerState.PrePurge} state. Currently in {BoilerState.Ready} state. ");
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
            throw new OperationCanceledException($"Operation stopped at {BoilerState.Ignition} state. Currently in {BoilerState.Ready} state. ");
        }
    }
}
