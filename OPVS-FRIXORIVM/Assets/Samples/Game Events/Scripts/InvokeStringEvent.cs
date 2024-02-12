using UnityEngine;

/// <summary>
///     Invokes an event with a string value
/// </summary>
public class InvokeStringEvent : MonoBehaviour
{
    /// <summary>
    ///     Event to invoke
    /// </summary>
    [SerializeField]
    private GameEvent _event;

    /// <summary>
    ///     Invokes event with given string value on the global channel
    /// </summary>
    /// <param name="value"></param>
    public void InvokeEvent(string value)
    {
        _event.Invoke(GameEvent.GlobalChannel, value);
    }
}
