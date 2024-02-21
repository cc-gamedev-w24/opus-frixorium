using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
///     Player Movement Handling
///     TODO: Abstract to allow for network support
/// </summary>
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement: PlayerController
{
    protected override string ActionMap => "Player Movement";
    
    private Rigidbody _rigidbody;
    private CharacterController _characterController;
    
    /// <summary>
    ///     Base player movement speed
    /// </summary>
    [SerializeField] private float _baseWalkSpeed = 2.0f;

    [SerializeField]
    private float _accelerationSpeed = 2.0f;

    [SerializeField]
    private float _decelerationSpeed = 4.0f;
    
    
    /// <summary>
    ///     Velocity impulse on jump
    /// </summary>
    [SerializeField] private float _jumpVelocity = 5.0f;

    /// <summary>
    ///     Amount of time to stay knocked out
    /// </summary>
    [SerializeField]
    private float _knockoutTime = 4.0f;

    /// <summary>
    ///      Current velocity of player
    /// </summary>
    private Vector3 _velocity = Vector3.zero;

    /// <summary>
    ///     Type of device controlling this player
    /// </summary>
    private string _deviceClass;
    
    private bool _knockedOut;
    private Camera _camera;
    private Quaternion _orientation;
    private Vector2 _walkValue;
    
    protected override void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.isKinematic = true;
        _deviceClass =  GetComponentInParent<PlayerInput>().devices[0].description.deviceClass;
        _camera = Camera.main;
        base.Awake(); 
    }

    private void Update()
    {
        UpdateWalk();
        
        if (_knockedOut) return;
        UpdateLook();
        ApplyGravity();
        ApplyMovement();   
    }

    /// <summary>
    ///     Applies gravity
    /// </summary>
    private void ApplyGravity()
    {
        _velocity.y -= 9.81f * Time.deltaTime;
    }

    /// <summary>
    ///     Update horizontal movement (acceleration, deceleration, etc.)
    /// </summary>
    private void UpdateWalk()
    {
        // Stop moving character controller if knocked out (rigidbody will continue on its own)
        if (_knockedOut)
        {
            _velocity = Vector3.zero;
            return;
        }
        
        var playerMoveDir = new Vector2(_velocity.x, _velocity.z).normalized;
        if (_walkValue == Vector2.zero)
        {
            if (_velocity.x != 0.0f || _velocity.z != 0.0f)
            {
                
                _velocity.x -= playerMoveDir.x * _decelerationSpeed * Time.deltaTime;
                _velocity.z -= playerMoveDir.y * _decelerationSpeed * Time.deltaTime;
                return;
            }
        }

        var walkVelocity = Vector2.ClampMagnitude(new Vector2(
            _velocity.x + _walkValue.x * _accelerationSpeed * Time.deltaTime,
            _velocity.z + _walkValue.y * _accelerationSpeed * Time.deltaTime
        ), _baseWalkSpeed);
        _velocity.x = walkVelocity.x;
        _velocity.z = walkVelocity.y;
    }

    /// <summary>
    ///     Updates player rotation from look direction
    /// </summary>
    private void UpdateLook()
    {
        transform.rotation = _orientation;
    }

    /// <summary>
    ///     Apply movement to player controller
    /// </summary>
    private void ApplyMovement()
    {
       _characterController.Move(_velocity * Time.deltaTime);
        if (_characterController.isGrounded) _velocity.y = 0;
    }

    private void FixedUpdate()
    {
        var position = transform.position;
        Debug.DrawLine(position, position + transform.forward * 3.0f, Color.red);
    }

    /// <summary>
    ///     Triggered on Jump input action
    /// </summary>
    private void OnJump(InputValue value)
    {
        if (!value.isPressed)
            return;
        _velocity.y = _jumpVelocity;

        if (!_characterController.isGrounded && !_knockedOut)
        {
            StartCoroutine(KnockOut());
        }
    }

    /// <summary>
    ///     Triggered on Walk input action
    /// </summary>
    private void OnWalk(InputValue value)
    {
        _walkValue = value.Get<Vector2>();
    }

    /// <summary>
    ///     Triggered on Look input action
    /// </summary>
    private void OnLook(InputValue value)
    {
        if (_knockedOut) return;
        var vecValue = value.Get<Vector2>();
        if (vecValue == Vector2.zero) return;
        if (_deviceClass == "Keyboard")
        {
            vecValue -= (Vector2)_camera.WorldToScreenPoint(transform.position);
        }
        _orientation = Quaternion.AngleAxis(Mathf.Rad2Deg * Mathf.Atan2(-vecValue.y, vecValue.x) + 90f, Vector3.up);
    }

    private IEnumerator KnockOut()
    {
        _characterController.enabled = false;
        _rigidbody.isKinematic = false;
        _rigidbody.velocity = _velocity;
        _knockedOut = true;

        yield return new WaitForSeconds(_knockoutTime);

        _knockedOut = false;
        _characterController.enabled = true;
        _rigidbody.isKinematic = true;
    }

}