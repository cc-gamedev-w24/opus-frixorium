using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
///     Represents an abstract player instance, handling various input modes and player models
/// </summary>
[RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour
{
    /// <summary>
    ///     Prefab to use for player
    /// </summary>
    [SerializeField]
    public GameObject PlayerPrefab;

    [SerializeField]
    public string ActionMap;

    /// <summary>
    ///     Current player prefab
    /// </summary>
    private GameObject _playerObject;

    /// <summary>
    ///     Player data
    /// </summary>
    public PlayerData PlayerData { get; set; }

    private PlayerInput _playerInput;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        if(!_playerObject) SetController(PlayerPrefab, ActionMap);
    }

    public void SetController(GameObject prefab, string actionMap)
    {
        _playerInput.SwitchCurrentActionMap(actionMap);
        if(_playerObject) Destroy(_playerObject);
        _playerObject = Instantiate(prefab, transform);
    }
}