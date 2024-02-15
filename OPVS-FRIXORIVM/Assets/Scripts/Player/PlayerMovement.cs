using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
///     Player Movement Handling
///     TODO: Abstract to allow for network support
/// </summary>
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement: MonoBehaviour
{
    private Rigidbody _rigidbody;
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

    private Camera _camera;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.isKinematic = true;
        _deviceClass =  GetComponentInParent<PlayerInput>().devices[0].description.deviceClass;
        _camera = Camera.main;
    }

    private void Update()
    {
        if (!_characterController.enabled) return;
        // Apply gravity
        _velocity.y -= 9.81f * Time.deltaTime;
        if (_characterController.isGrounded) _velocity.y = 0;
        
       _characterController.Move(_velocity * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        Debug.DrawLine(transform.position, transform.position + transform.forward * 3.0f, Color.red);
    }

    /// <summary>
    ///     Triggered on Jump input action
    /// </summary>
    private void OnJump(InputValue value)
    {
        if (value.Get<float>() == 1f)
        {

            if (_characterController.isGrounded)
            {
                Debug.Log("Boing");
                _velocity.y = _jumpVelocity;
            }
            else
            {
                StartCoroutine(KnockOut());
            }
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

    /// <summary>
    ///     Triggered on Look input action
    /// </summary>
    private void OnLook(InputValue value)
    {
        var vecValue = value.Get<Vector2>();
        if (_deviceClass == "Keyboard")
        {
            vecValue -= (Vector2)_camera.WorldToScreenPoint(transform.position);
        }
        transform.rotation = Quaternion.AngleAxis(Mathf.Rad2Deg * Mathf.Atan2(-vecValue.y, vecValue.x) + 90f, Vector3.up);
    }

    private IEnumerator KnockOut()
    {
        _characterController.enabled = false;
        _rigidbody.isKinematic = false;

        yield return new WaitForSeconds(_knockoutTime);

        _characterController.enabled = true;
        _rigidbody.isKinematic = true;
    }
    
}