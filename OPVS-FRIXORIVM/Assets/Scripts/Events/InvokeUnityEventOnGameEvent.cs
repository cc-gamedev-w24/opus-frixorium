using UnityEngine;
using UnityEngine.Events;

namespace Events
{
    public class InvokeUnityEventOnGameEvent: GameEventListener
    {
        [SerializeField]
        private UnityEvent _unityEvent;

        public override void RaiseEvent() => _unityEvent.Invoke();
    }
}
