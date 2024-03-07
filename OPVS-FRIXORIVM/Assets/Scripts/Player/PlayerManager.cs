using System;
using System.Collections.Generic;
using System.Linq;
using Events;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

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

    [SerializeField]
    private float _spawnDistance = 2.0f;
    
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
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    ///     Triggered when local player joined via PlayerInputManager
    /// </summary>
    [UsedImplicitly]
    private void OnPlayerJoined(PlayerInput input)
    {
        var playerTransform = input.gameObject.transform;
        playerTransform.parent = transform;
        var playerIndex = Array.IndexOf(_joinSlots, null);
        playerTransform.position += Quaternion.AngleAxis((360.0f / _maxPlayers) * playerIndex, Vector3.up) * Vector3.right * _spawnDistance + Vector3.up;
        
        _joinSlots[playerIndex] = input.gameObject.GetComponent<Player>();
        _joinSlots[playerIndex].PlayerData = new PlayerData(_dataChangedEvent, playerIndex)
        {
            DeviceClass = input.devices[0].description.deviceClass,
            Color = Random.ColorHSV(0, 1, 1, 1, 1, 1)
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
        var playerIndex = Array.IndexOf(_joinSlots, leftPlayer);
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
        _playerInputManager.EnableJoining();
        UpdateControllersOnPlayers();
    }

    private void EnterGameMode()
    {
        _currentController = _gameController;
        _playerInputManager.DisableJoining();
        UpdateControllersOnPlayers();
    }

    private void UpdateControllersOnPlayers()
    {
        foreach (var player in _joinSlots.NotUnityNull())
        {
            player.SetController(_currentController);
        }
    }

    public bool AllPlayersReady()
    {
        var readyCount = 0;
        var playerCount = 4;
        
        if (_joinSlots[0] == null)
            return false;
        
        foreach (var player in _joinSlots)
        {
            if (player == null)
            {
                playerCount--;
                continue;
            }
                
            if (player.PlayerData.IsReady)
                readyCount++;
        }

        return readyCount == playerCount;
    }

    private void OnDestroy()
    {
        _lobbyEnteredListener.Dispose();
        _gameStartedListener.Dispose();
    }

    public IEnumerable<PlayerData> GetAllPlayerData()
    {
        return _joinSlots.NotNull().Select(player => player.PlayerData);
    }
}
