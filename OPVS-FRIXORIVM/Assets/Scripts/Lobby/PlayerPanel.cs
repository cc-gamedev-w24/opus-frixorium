using Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///     Lobby Panel Representing Player State
/// </summary>
[RequireComponent(typeof(Image))]
public class PlayerPanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _text;
    
    [Header("Events")]
    [SerializeField]
    private GameEvent _playerJoinedEvent;
    [SerializeField]
    private GameEvent _playerLeftEvent;
    [SerializeField]
    private GameEvent _playerDataChangedEvent;
    [SerializeField]
    private ushort _playerNumber;
    
    [Header("Colors")]
    [SerializeField]
    private Color _disabledColor = Color.grey;
    [SerializeField]
    private Color _joinedColor = Color.yellow;
    [SerializeField]
    private Color _readyColor = Color.green;
    
    private DelegateGameEventListener _joinedListener;
    private DelegateGameEventListener _leftListener;
    private DelegateGameEventListener _playerDataChangedListener;

    private Image _backgroundImage;

    private void Awake()
    {
        _backgroundImage = GetComponent<Image>();
        _backgroundImage.color = _disabledColor;

        _text.text = string.Empty;
        
        _joinedListener = new DelegateGameEventListener(_playerJoinedEvent, _ => OnJoinedEvent(), _playerNumber);
        _leftListener = new DelegateGameEventListener(_playerLeftEvent, _ => OnLeftEvent(), _playerNumber);
        _playerDataChangedListener = new DelegateGameEventListener(_playerDataChangedEvent, OnPlayerDataChangedEvent, _playerNumber);
    }

    /// <summary>
    ///     Triggered when player data changes
    /// </summary>
    /// <param name="value">PlayerData</param>
    private void OnPlayerDataChangedEvent(object value)
    {
        if (value is not PlayerData playerData)
        {
            Debug.LogError("Provided event data is wrong type");
            return;
        }
        
        // Update UI
        _text.text =  playerData.DeviceClass;
        _backgroundImage.color = playerData.IsReady ? _readyColor : _joinedColor;
    }

    /// <summary>
    ///     Dispose of event listeners when this object is destroyed
    /// </summary>
    private void OnDestroy()
    {
        _joinedListener.Dispose();
        _leftListener.Dispose();
        _playerDataChangedListener.Dispose();
    }
    
    private void OnJoinedEvent()
    {
        _backgroundImage.color = _joinedColor;
    }

    private void OnLeftEvent()
    {
        _text.text = string.Empty;
        _backgroundImage.color = _disabledColor;
    }
}
