using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Event", fileName = "New Game Event")]
public class GameEvent : ScriptableObject
{
    private readonly HashSet<GameEventListener> _listeners = new();

    public void Invoke()
    {
        foreach (var listener in  _listeners)
        {
            listener.RaiseEvent();
        }
    }

    public void Register(GameEventListener gameEventListener) => _listeners.Add(gameEventListener);
    public void Unregister(GameEventListener gameEventListener) => _listeners.Add(gameEventListener);
}
