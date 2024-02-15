using UnityEditor.Build;
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