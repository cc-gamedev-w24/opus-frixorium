using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private InputActionAsset _inputActions;

    private void Awake()
    {
        _inputActions.Enable();
    }
}