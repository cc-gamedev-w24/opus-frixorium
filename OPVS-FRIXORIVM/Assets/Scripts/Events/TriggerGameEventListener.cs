using System;
public class TriggerGameEventListener: IGameEventListener, IDisposable
{
    public bool Triggered { get; private set; }
    
    private readonly int _channel;
    private readonly GameEvent _event;
    
    public TriggerGameEventListener(GameEvent gameEvent, int channel = GameEvent.GlobalChannel)
    {
        _event = gameEvent;
        _channel = channel;
        _event.Register(this, channel);
    }

    public void OnEvent(object value = null)
    {
        Triggered = true;
    }
    
    public void Dispose()
    {
        _event.Unregister(this, _channel);
    }
}
