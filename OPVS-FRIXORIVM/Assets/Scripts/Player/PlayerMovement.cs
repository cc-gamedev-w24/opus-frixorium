using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
///     Player Movement Handling
///     TODO: Abstract to allow for network support
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement: MonoBehaviour
{
    private CharacterController _characterController;
    
    /// <summary>
    ///     Base player movement speed
    /// </summary>
    [SerializeField] private float _baseWalkSpeed = 2.0f;
    
    /// <summary>
    ///     Velocity impulse on jump
    /// </summary>
    [SerializeField] private float _jumpVelocity = 5.0f;

    /// <summary>
    ///      Current velocity of player
    /// </summary>
    private Vector3 _velocity = Vector3.zero;
    
    private void Awake()
    {
        _characterController = gameObject.GetComponent<CharacterController>();
    }

    private void Update()
    {
        // Apply gravity
        _velocity.y -= 9.81f * Time.deltaTime;
        
       _characterController.Move(_velocity * Time.deltaTime);
    }

    /// <summary>
    ///     Triggered on Jump input action
    /// </summary>
    private void OnJump()
    {
        if (_characterController.isGrounded)
        {
            _velocity.y = _jumpVelocity;
        }
    }

    /// <summary>
    ///     Triggered on Walk input action
    /// </summary>
    private void OnWalk(InputValue value)
    {
        var moveValue = value.Get<Vector2>();
        _velocity.x = moveValue.x * _baseWalkSpeed;
        _velocity.z = moveValue.y * _baseWalkSpeed;
    }
}