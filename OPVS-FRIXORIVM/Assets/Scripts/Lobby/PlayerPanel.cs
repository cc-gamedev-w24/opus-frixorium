using DG.Tweening;
using Events;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

/// <summary>
///     Lobby Panel Representing Player State
/// </summary>
[RequireComponent(typeof(Image))]
public class PlayerPanel : MonoBehaviour
{
    private readonly int JoinedHash = Animator.StringToHash("Joined");
    
    [SerializeField]
    private RawImage _previewImage;
    [SerializeField]
    private CanvasGroup _previewCanvasGroup;
    [SerializeField]
    private Camera _previewCamera;
    [SerializeField]
    private int _previewSize = 512;
    [SerializeField]
    private Animator _previewAnimator;
    [SerializeField]
    private Renderer _previewRenderer;
    [SerializeField]
    private GameObject _colorWheel;
    [SerializeField]
    private Transform _colorWheelKnob;
    
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
    private RenderTexture _previewRenderTexture;

    private void Awake()
    {
        _backgroundImage = GetComponent<Image>();
        _backgroundImage.color = _disabledColor;
        
        _previewRenderTexture = RenderTexture.GetTemporary(_previewSize, _previewSize);
        _previewCamera.targetTexture = _previewRenderTexture;
        _previewImage.texture = _previewRenderTexture;
        
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

        _previewRenderer.material.color = playerData.Color;
        
        Color.RGBToHSV(playerData.Color, out var hue, out _, out _);
        _colorWheelKnob.rotation = Quaternion.AngleAxis(hue * -360.0f, Vector3.forward);
        _colorWheel.SetActive(!playerData.IsReady);
        
        // Update UI
        _backgroundImage.DOColor(playerData.IsReady ? _readyColor : _joinedColor, 0.5f);
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
        _backgroundImage.DOColor(_joinedColor, 0.5f);
        _previewCanvasGroup.DOFade(1f, 0.5f);
        _colorWheel.SetActive(true);
        _previewAnimator.SetBool(JoinedHash, true);
    }

    private void OnLeftEvent()
    {
        _backgroundImage.DOColor(_disabledColor, 0.5f);
        _previewCanvasGroup.DOFade(0.5f, 0.5f);
        _colorWheel.SetActive(false);
        _previewRenderer.material.color = Color.grey;
        _previewAnimator.SetBool(JoinedHash, false);
    }
}
