using System;
using System.Linq;
using Events;
using JetBrains.Annotations;
using RotaryHeart.Lib.SerializableDictionary;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using static GameStateMachine;

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
    
    [Header("Outgoing Events")]
    [SerializeField] private GameEvent _playerJoinedEvent;
    [SerializeField]
    private GameEvent _playerLeftEvent;
    [SerializeField]
    private GameEvent _dataChangedEvent;

    [Header("Incoming Events")]
    [SerializeField]
    private GameEvent _gameStateChangedEvent;

    [FormerlySerializedAs("_playerPrefabs")]
    [SerializeField]
    private SerializableDictionaryBase<GameState, PlayerControllerValue> _playerControllers;
    private GameState _currentState;

    private DelegateGameEventListener _gameStateChangedListener;
    
    private void Awake()
    {
        _playerInputManager = gameObject.GetComponent<PlayerInputManager>();
        _joinSlots = new Player[_maxPlayers];
        _gameStateChangedListener = new DelegateGameEventListener(_gameStateChangedEvent, UpdatePlayerModelOnStateChange);
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
        var controller = _playerControllers[_currentState];
        _joinSlots[playerIndex].PlayerPrefab = controller.Prefab;
        _joinSlots[playerIndex].ActionMap = controller.InputActionMap;
        
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

    private void UpdatePlayerModelOnStateChange(object value)
    {
        if (value is not GameState gameState) return;

        _currentState = gameState;
        if (!_playerControllers.ContainsKey(gameState))
        {
            foreach (var player in _joinSlots.NotNull())
            {
                player.gameObject.SetActive(false);
            }
        }
        else
        {
            foreach (var player in _joinSlots.NotNull())
            {
                player.gameObject.SetActive(true);
                var controller = _playerControllers[gameState];
                player.SetController(controller.Prefab, controller.InputActionMap);
            }
        }
    }

    private void OnDestroy()
    {
        _gameStateChangedListener.Dispose();
    }

    [Serializable]
    private struct PlayerControllerValue
    {
        public GameObject Prefab;
        public string InputActionMap;
    }
}
