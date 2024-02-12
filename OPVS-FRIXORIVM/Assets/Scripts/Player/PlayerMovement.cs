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

    public GameObject _attackTarget;

    private Quaternion _lookRotation;
    private Vector3 _direction;
    private float _attackCountdown;
    private bool _isAttacking;
    private void Awake()
    {
        _characterController = gameObject.GetComponent<CharacterController>();
        _attackTarget = GameObject.CreatePrimitive(PrimitiveType.Cube);
        _attackTarget.SetActive(false);
        _attackCountdown = 3.0f;
        _isAttacking = false;
    }

    private void Update()
    {
        // Apply gravity
        _velocity.y -= 9.81f * Time.deltaTime;
        
        _characterController.Move(_velocity * Time.deltaTime);

        //Debug.Log(Mouse.current.position.ReadValue());
        transform.LookAt(Camera.main.ScreenToWorldPoint(new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, Camera.main.transform.position.y)));
        Vector3 mousePos = Mouse.current.position.ReadValue();
        Vector3 lookPoint = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, transform.position.z));
        lookPoint.y = transform.position.y;
        //transform.LookAt(lookPoint);

        _direction = lookPoint - transform.position;
        //_direction.y = transform.position.y;
        Debug.Log(_direction);
        //_direction.y = 20f;
        
        _lookRotation = Quaternion.LookRotation(_direction);
        //Debug.Log(_lookRotation);

        transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * 1);

        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);


        //Debug.Log(lookPoint);
        _attackTarget.transform.position = transform.position + transform.forward*2.0f;
        
        if (_isAttacking)
        {
            if (_attackCountdown == 3.0f)
            {
                _attackTarget.SetActive(true);
            }
            else if (_attackCountdown <= 0.0f)
            {
                _isAttacking = false;
                _attackTarget.SetActive(false);
            }
            _attackCountdown -= Time.deltaTime;
        }

        /*float oldCameraY = _camera.transform.position.y;
        _cameraPos = transform.position;
        _cameraPos.y = oldCameraY;
        _camera.transform.position = _cameraPos;*/
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
            _attackTarget.SetActive(true);
            _attackCountdown = 3.0f;
        }
    }
}