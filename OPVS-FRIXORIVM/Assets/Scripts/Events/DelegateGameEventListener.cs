using System;

namespace Events
{
    public class DelegateGameEventListener: IGameEventListener, IDisposable
    {
        private readonly int _channel;
        private readonly GameEvent _event;
        private readonly Action<object> _action;
        
        public DelegateGameEventListener(GameEvent gameEvent, Action<object> action, int channel = GameEvent.GlobalChannel)
        {
            _event = gameEvent;
            _action = action;
            _channel = channel;
            _event.Register(this, channel);
        }

        public void OnEvent(object value = null)
        {
            _action.Invoke(value);
        }

        public void Dispose()
        {
            _event.Unregister(this, _channel);
        }
    }
}
