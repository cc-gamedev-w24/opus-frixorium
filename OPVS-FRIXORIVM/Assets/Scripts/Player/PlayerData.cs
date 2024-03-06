using UnityEngine;

/// <summary>
///     Player state which triggers event when updated
/// </summary>
public class PlayerData
{
    private readonly GameEvent _dataChangedEvent;
    
    /// <summary>
    ///     Which player slot this player occupies
    /// </summary>
    public int PlayerNumber { get; }
    
    public PlayerData(GameEvent dataChangedEvent, int playerNumber)
    {
        _playerMaxHP = 100;
        _playerHP = 100;
        _playerMaxStamina = 100;
        _playerStamina = 100;
        _playerHit = false;
        _playerBlocked = false;
        _dataChangedEvent = dataChangedEvent;
        PlayerNumber = playerNumber;
    }

    private bool _isReady;
    
    /// <summary>
    ///     Whether this player is readied up in the lobby
    /// </summary>
    public bool IsReady
    {
        get => _isReady;
        set => SetValue(out _isReady, value);
    }

    private string _deviceClass;
    
    /// <summary>
    ///     Device class of this player, e.g. "Keyboard" or "Gamepad"
    /// </summary>
    public string DeviceClass
    {
        get => _deviceClass;
        set => SetValue(out _deviceClass, value);
    }

    private bool _isKnockedOut;

    public bool IsKnockedOut
    {
        get => _isKnockedOut;
        set => SetValue(out _isKnockedOut, value);
    }
    
    private int _playerHP;
    /// <summary>
    ///     Device class of this player, e.g. "Keyboard" or "Gamepad"
    /// </summary>
    public int PlayerHP
    {
        get => _playerHP;
        set => SetValue(out _playerHP, value);
    }

    private int _playerMaxHP;
    /// <summary>
    ///     Device class of this player, e.g. "Keyboard" or "Gamepad"
    /// </summary>
    public int PlayerMaxHP
    {
        get => _playerMaxHP;
        set => SetValue(out _playerMaxHP, value);
    }

    private int _playerStamina;
    /// <summary>
    ///     Device class of this player, e.g. "Keyboard" or "Gamepad"
    /// </summary>
    public int PlayerStamina
    {
        get => _playerStamina;
        set => SetValue(out _playerStamina, value);
    }

    private int _playerMaxStamina;
    /// <summary>
    ///     Device class of this player, e.g. "Keyboard" or "Gamepad"
    /// </summary>
    public int PlayerMaxStamina
    {
        get => _playerMaxStamina;
        set => SetValue(out _playerMaxStamina, value);
    }

    private bool _playerHit;
    /// <summary>
    ///     Device class of this player, e.g. "Keyboard" or "Gamepad"
    /// </summary>
    public bool PlayerHit
    {
        get => _playerHit;
        set => SetValue(out _playerHit, value);
    }

    private bool _playerBlocked;
    /// <summary>
    ///     Device class of this player, e.g. "Keyboard" or "Gamepad"
    /// </summary>
    public bool PlayerBlocked
    {
        get => _playerBlocked;
        set => SetValue(out _playerHit, value);
    }

    private Color _color;
    public Color Color {
        get => _color;
        set => SetValue(out _color, value);
    }
    
    /// <summary>
    ///     Sets value and invokes event
    /// </summary>
    /// <param name="field">Field to set</param>
    /// <param name="value">Value to assign to field</param>
    /// <typeparam name="T">Field type</typeparam>
    private void SetValue<T>(out T field, T value)
    {
        field = value;
        _dataChangedEvent.Invoke(PlayerNumber, this);
    }
}