using UnityEngine;

/// <summary>
///     Represents an abstract player instance, handling various input modes and player models
/// </summary>
public class Player : MonoBehaviour
{
    /// <summary>
    ///     Prefab to use for player
    /// </summary>
    [SerializeField] private GameObject _playerPrefab;

    /// <summary>
    ///     Current player prefab
    /// </summary>
    private GameObject _playerObject;

    private void Awake()
    {
        SpawnPlayer();
    }

    /// <summary>
    ///     Instantiates player prefab as a child of this manager
    /// </summary>
    private void SpawnPlayer()
    { 
        _playerObject = Instantiate(_playerPrefab, transform);
    }

    /// <summary>
    ///     Destroys current player prefab
    /// </summary>
    private void DestroyPlayer()
    {
        Destroy(_playerObject);
    }
}
