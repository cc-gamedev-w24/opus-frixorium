using System;
using Unity.VisualScripting;
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

    /// <summary>
    ///     Type of device controlling this player
    /// </summary>
    private string _deviceClass;

    private Camera _camera;

    private void Awake()
    {
        _characterController = gameObject.GetComponent<CharacterController>();
        _deviceClass = gameObject.GetComponentInParent<PlayerInput>().devices[0].description.deviceClass;
        _camera = Camera.main;
    }

    private void Update()
    {
        // Apply gravity
        _velocity.y -= 9.81f * Time.deltaTime;
        
       _characterController.Move(_velocity * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        Debug.DrawLine(transform.position, transform.position + transform.forward * 3.0f, Color.red);
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
}