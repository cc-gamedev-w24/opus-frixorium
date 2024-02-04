using JetBrains.Annotations;
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
    ///     Current number of players
    /// </summary>
    private uint _playerCount;

    private void Awake()
    {
        _playerInputManager = gameObject.GetComponent<PlayerInputManager>();
    }

    /// <summary>
    ///     Triggered when local player joined via PlayerInputManager
    /// </summary>
    [UsedImplicitly]
    private void OnPlayerJoined()
    {
        _playerCount++;
        if (_playerCount >= _maxPlayers)
        {
            _playerInputManager.DisableJoining();
        }
    }

    /// <summary>
    ///     Triggered when local player left via PlayerInputManager
    /// </summary>
    [UsedImplicitly]
    private void OnPlayerLeft()
    {
        _playerCount--;
        if (_playerCount < _maxPlayers)
        {
            _playerInputManager.EnableJoining();
        }
    }
}
