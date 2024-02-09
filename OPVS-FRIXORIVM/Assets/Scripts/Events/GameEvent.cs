using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Event", fileName = "New Game Event")]
public class GameEvent : ScriptableObject
{
    public const int GlobalChannel = -1;
    
    private readonly Dictionary<int, HashSet<GameEventListener>> _channelListeners = new();
    
    public void Invoke() => Invoke(GlobalChannel);
    
    public void Invoke(int channel)
    {
        if (!_channelListeners.TryGetValue(channel, out var listeners))
        {
            Debug.LogError("Specified channel does not exist!");
        }

        foreach (var listener in listeners!)
        {
            listener.RaiseEvent();
        }

        if (channel == GlobalChannel || !_channelListeners.TryGetValue(GlobalChannel, out var globalListeners))
            return;
        foreach (var listener in globalListeners)
        {
            listener.RaiseEvent();
        }
    }


    public void Register(GameEventListener gameEventListener) => Register(gameEventListener, GlobalChannel);

    public void Register(GameEventListener gameEventListener, int channel)
    {
        if (!_channelListeners.ContainsKey(channel))
        {
            _channelListeners.Add(channel, new HashSet<GameEventListener>());
        }
        _channelListeners[channel].Add(gameEventListener);
    }

    public void Unregister(GameEventListener gameEventListener) => Unregister(gameEventListener, GlobalChannel);

    public void Unregister(GameEventListener gameEventListener, int channel)
    {
        if (!_channelListeners.ContainsKey(channel))
        {
            Debug.LogWarning("Attempted to unsubscribe from a nonexistent event channel.");
            return;
        }
        _channelListeners[channel].Remove(gameEventListener);
    }
}
