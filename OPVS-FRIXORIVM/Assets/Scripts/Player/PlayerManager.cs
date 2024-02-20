using System.Linq;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
///     Keeps track of number of players in game and prevents PlayerInputManager from adding too many when combined with
///     networked players.
///
///     TODO: Track Networked Players
/// </summary>
[RequireComponent(typeof(PlayerInputManager))]
public class PlayerManager : MonoBehaviour
{
    private PlayerInputManager _playerInputManager;
    
    /// <summary>
    ///     Maximum number of total players allowed
    /// </summary>
    [SerializeField] private uint _maxPlayers = 4;

    /// <summary>
    ///     Array of available player slots. Unoccupied slots will be null.
    /// </summary>
    private Player[] _joinSlots;
    
    [Header("Game Events")]
    [SerializeField] private GameEvent _playerJoinedEvent;
    [SerializeField]
    private GameEvent _playerLeftEvent;
    [SerializeField]
    private GameEvent _dataChangedEvent;

    [SerializeField] private Player playerModel;
    private void Awake()
    {
        _playerInputManager = gameObject.GetComponent<PlayerInputManager>();
        _joinSlots = new Player[_maxPlayers];
    }

    /// <summary>
    ///     Triggered when local player joined via PlayerInputManager
    /// </summary>
    [UsedImplicitly]
    private void OnPlayerJoined(PlayerInput input)
    {

        var playerIndex = ArrayUtility.IndexOf(_joinSlots, null);
        _joinSlots[playerIndex] = input.gameObject.GetComponent<Player>();
        _joinSlots[playerIndex].PlayerData = new PlayerData(_dataChangedEvent, playerIndex)
        {
            DeviceClass = input.devices[0].description.deviceClass
        };
        
        if (_joinSlots.All(slot => slot != null))
        {
            _playerInputManager.DisableJoining();
        }
        _playerJoinedEvent.Invoke(playerIndex);
    }

    /// <summary>
    ///     Triggered when local player left via PlayerInputManager
    /// </summary>
    [UsedImplicitly]
    private void OnPlayerLeft(PlayerInput input)
    {
        var leftPlayer = input.gameObject.GetComponent<Player>();
        var playerIndex = ArrayUtility.IndexOf(_joinSlots, leftPlayer);
        _joinSlots[playerIndex] = null;
        _playerLeftEvent.Invoke(playerIndex);
        if (_joinSlots.Any(slot => slot == null))
        {
            _playerInputManager.EnableJoining();
        }
    }
}
