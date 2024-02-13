using Events;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class PlayerPanel : MonoBehaviour
{
    [Header("Events")]
    [SerializeField]
    private GameEvent _playerJoinedEvent;
    [SerializeField]
    private GameEvent _playerLeftEvent;
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

    private Image _backgroundImage;

    private void Awake()
    {
        _backgroundImage = GetComponent<Image>();
        
        _joinedListener = new DelegateGameEventListener(_playerJoinedEvent, _ => OnJoinedEvent(), _playerNumber);
        _leftListener = new DelegateGameEventListener(_playerLeftEvent, _ => OnLeftEvent(), _playerNumber);
    }

    private void OnDestroy()
    {
        _joinedListener.Dispose();
        _leftListener.Dispose();
    }

    private void OnJoinedEvent()
    {
        _backgroundImage.color = _joinedColor;
    }

    private void OnLeftEvent()
    {
        _backgroundImage.color = _disabledColor;
    }
}
