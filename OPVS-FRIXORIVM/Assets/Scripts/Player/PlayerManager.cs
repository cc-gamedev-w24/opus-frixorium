using System.Linq;
using Events;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

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

    [Header("Incoming Events")]
    [SerializeField]
    private GameEvent _gameStartedEvent;
    [SerializeField]
    private GameEvent _lobbyEnteredEvent;
    
    [Header("Outgoing Events")]
    [SerializeField] private GameEvent _playerJoinedEvent;
    [SerializeField]
    private GameEvent _playerLeftEvent;
    [SerializeField]
    private GameEvent _dataChangedEvent;

    [Header("Player Controllers")]
    [SerializeField]
    private GameObject _menuController;
    [SerializeField]
    private GameObject _gameController;

    [FormerlySerializedAs("_spawnInGameMode")]
    [SerializeField]
    private bool _startInGameMode;

    private GameObject _currentController;

    private DelegateGameEventListener _gameStartedListener;
    private DelegateGameEventListener _lobbyEnteredListener;

    private void Awake()
    {
        _currentController = _startInGameMode ? _gameController : _menuController;
        _playerInputManager = gameObject.GetComponent<PlayerInputManager>();
        _joinSlots = new Player[_maxPlayers];
        _gameStartedListener = new DelegateGameEventListener(_gameStartedEvent, _ => EnterGameMode());
        _lobbyEnteredListener = new DelegateGameEventListener(_lobbyEnteredEvent, _ => EnterLobbyMode());
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
        _joinSlots[playerIndex].PlayerPrefab = _currentController;
        
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

    private void EnterLobbyMode()
    {
        _currentController = _menuController;
        UpdateControllersOnPlayers();
    }

    private void EnterGameMode()
    {
        _currentController = _gameController;
        UpdateControllersOnPlayers();
    }

    private void UpdateControllersOnPlayers()
    {
        foreach (var player in _joinSlots.NotUnityNull())
        {
            player.SetController(_currentController);
        }
    }

    private void OnDestroy()
    {
        _lobbyEnteredListener.Dispose();
        _gameStartedListener.Dispose();
    }
}
