using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerInput: MonoBehaviour
{
    private CharacterController _characterController;
    
    [SerializeField] private InputActionReference _walkAction;
    [SerializeField] private InputActionReference _jumpAction;

    [SerializeField] private float _baseWalkSpeed = 2.0f;
    [SerializeField] private float _jumpVelocity = 5.0f;

    private Vector3 _moveDir = Vector3.zero;
    
    private void Awake()
    {
        _characterController = gameObject.GetComponent<CharacterController>();
    }

    private void Update()
    {
       var walkValue = _walkAction.action.ReadValue<Vector2>() * _baseWalkSpeed;
       _moveDir.x = walkValue.x;
       _moveDir.z = walkValue.y;
       
       if (_jumpAction.action.WasPressedThisFrame())
       {
           _moveDir.y = _jumpVelocity;
       }

       _moveDir.y -= 9.81f * Time.deltaTime;

       _characterController.Move(_moveDir);
    }
}