using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement: MonoBehaviour
{
    private CharacterController _characterController;
    
    [SerializeField] private float _baseWalkSpeed = 2.0f;
    [SerializeField] private float _jumpVelocity = 5.0f;

    private Vector3 _moveDir = Vector3.zero;
    
    private void Awake()
    {
        _characterController = gameObject.GetComponent<CharacterController>();
    }

    private void Update()
    {
        _moveDir.y -= 9.81f * Time.deltaTime;
       _characterController.Move(_moveDir * Time.deltaTime);
    }

    private void OnJump()
    {
        if (_characterController.isGrounded)
        {
            _moveDir.y = _jumpVelocity;
        }
    }

    private void OnWalk(InputValue value)
    {
        var moveValue = value.Get<Vector2>();
        _moveDir.x = moveValue.x * _baseWalkSpeed;
        _moveDir.z = moveValue.y * _baseWalkSpeed;
    }
}