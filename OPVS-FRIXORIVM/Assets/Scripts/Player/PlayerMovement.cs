using System;
using Unity.Mathematics;
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

    public GameObject _attackTarget;

    private Quaternion _lookRotation;
    private Vector3 _direction;
    private float _attackCountdown;
    private bool _isAttacking;

    /// <summary>
    ///     Type of device controlling this player
    /// </summary>
    private string _deviceClass;

    private Camera _camera;

    private GameObject hitbox;
    
    private void Awake()
    {
        _characterController = gameObject.GetComponent<CharacterController>();
        hitbox = Instantiate(_attackTarget, transform);
        hitbox.SetActive(false);
        _attackCountdown = 3.0f;
        _isAttacking = false;
        _deviceClass = gameObject.GetComponentInParent<PlayerInput>().devices[0].description.deviceClass;
        _camera = Camera.main;
    }

    private void Update()
    {
        // Apply gravity
        _velocity.y -= 9.81f * Time.deltaTime;
        
        _characterController.Move(_velocity * Time.deltaTime);

        Vector3 mousePos = Mouse.current.position.ReadValue();
        Vector3 lookPoint = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, transform.position.z));
        lookPoint.y = transform.position.y;

        _direction = lookPoint - transform.position;
        //Debug.Log(_direction);
        
        _lookRotation = Quaternion.LookRotation(_direction);

        hitbox.transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * 1);

        //transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);


        
        
        if (_isAttacking)
        {
            if (_attackCountdown == 3.0f)
            {
                hitbox.SetActive(true);
            }
            else if (_attackCountdown <= 0.0f)
            {
                _isAttacking = false;
                hitbox.SetActive(false);
            }
            _attackCountdown -= Time.deltaTime;
        }

        /*float oldCameraY = _camera.transform.position.y;
        _cameraPos = transform.position;
        _cameraPos.y = oldCameraY;
        _camera.transform.position = _cameraPos;*/
    }

    private void FixedUpdate()
    {
        Debug.DrawLine(transform.position, transform.position + transform.forward * 3.0f, Color.red);
        hitbox.transform.position = transform.position + transform.forward * 2.0f;

        //Debug.Log("x: " + hitbox.transform.position.x + ", z: " + hitbox.transform.position.z);
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

    private void OnAttack()
    {
        if (!_isAttacking)
        {
            _isAttacking = true;
            hitbox.SetActive(true);
            _attackCountdown = 3.0f;
        }
    }

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