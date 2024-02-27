using System;
public class GameEventTriggerPredicate: IPredicate, IDisposable 
{
    private readonly TriggerGameEventListener _listener;

    public GameEventTriggerPredicate(GameEvent @event, int channel = GameEvent.GlobalChannel)
    {
        _listener = new TriggerGameEventListener(@event, channel);
    }

    public bool Evaluate() => _listener.Triggered;

    public void Dispose()
    {
        _listener.Dispose();
    }
}
