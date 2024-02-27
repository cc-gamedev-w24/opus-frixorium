using UnityEngine;
using UnityEngine.InputSystem;
public abstract class PlayerController: MonoBehaviour
{
    protected PlayerData PlayerData;
    
    protected abstract string ActionMap { get; }
    
    protected virtual void Awake()
    {
        PlayerData = GetComponentInParent<Player>().PlayerData;
        var playerInput = GetComponentInParent<PlayerInput>();
        playerInput.SwitchCurrentActionMap(ActionMap);
    }
}
