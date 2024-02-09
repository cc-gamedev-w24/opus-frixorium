using System.Collections.Generic;
using UnityEngine;
public abstract class GameEventListener: MonoBehaviour
{
    [SerializeField]
    private GameEvent _gameEvent;

    [SerializeField]
    private bool _useChannels;

    [SerializeField]
    private List<ushort> _channels;
    

    private void Awake()
    {
        if (_useChannels)
        {
            foreach (var channel in _channels)
            {
                _gameEvent.Register(this, channel);
            }
        }
        else
        {
            _gameEvent.Register(this);
        }
    }

    private void OnDestroy()
    {
        if (_useChannels)
        {
            foreach (var channel in _channels)
            {
                _gameEvent.Unregister(this, channel);
            }
        }
        else
        {
            _gameEvent.Unregister(this);
        }
    }

    public abstract void RaiseEvent();
}