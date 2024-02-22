using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
///     Player Movement Handling
///     TODO: Abstract to allow for network support
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement: MonoBehaviour
{
    public CharacterController _characterController;
    
    /// <summary>
    ///     Base player movement speed
    /// </summary>
    [SerializeField] private float _baseWalkSpeed = 2.0f;
    private float _oldWalkSpeed;
    
    /// <summary>
    ///     Velocity impulse on jump
    /// </summary>
    [SerializeField] private float _jumpVelocity = 5.0f;

    /// <summary>
    ///      Current velocity of player
    /// </summary>
    private Vector3 _velocity = Vector3.zero;

    public GameObject _attackTarget;
    public GameObject _blockVisual;

    private Quaternion _lookRotation;
    private Vector3 _direction;
    private float _actionCountdown;
    private float _timeSinceLastAction;
    private bool _isAttacking;
    private bool _isBlocking;
    private float _IFrameCountdown;
    int invincibleLayer;
    int playerLayer;

    /// <summary>
    ///     Type of device controlling this player
    /// </summary>
    private string _deviceClass;

    private Camera _camera;

    private GameObject hitbox;
    private GameObject blockbox;

    private void Awake()
    {
        _characterController = gameObject.GetComponent<CharacterController>();
        hitbox = Instantiate(_attackTarget, transform);
        blockbox = Instantiate(_blockVisual, transform);
        hitbox.SetActive(false);
        blockbox.SetActive(false);
        _oldWalkSpeed = _baseWalkSpeed;
        _actionCountdown = 0.0f;
        _IFrameCountdown = 0.0f;
        _timeSinceLastAction = 0.0f;
        _isAttacking = false;
        _deviceClass = gameObject.GetComponentInParent<PlayerInput>().devices[0].description.deviceClass;
        _camera = Camera.main;
        invincibleLayer = LayerMask.NameToLayer("Invincible");
        playerLayer = LayerMask.NameToLayer("Player");
    }

    private void Update()
    {
        // Apply gravity
        _velocity.y -= 9.81f * Time.deltaTime;
        
        _characterController.Move(_velocity * Time.deltaTime);


        //Attacking calculations
        if (_isAttacking)
        {
            if (_actionCountdown == 3.0f)
            {
                hitbox.SetActive(true);
            }
            else if (_actionCountdown <= 0.0f)
            {
                _isAttacking = false;
                _baseWalkSpeed = _oldWalkSpeed;
                hitbox.SetActive(false);
            }
            _actionCountdown -= Time.deltaTime;
        }


        //Blocking calculations
        if (_isBlocking)
        {
            if (_actionCountdown <= 0.0f)
            {
                _isBlocking = false;
                _baseWalkSpeed = _oldWalkSpeed;
                blockbox.SetActive(false);
            } 
            else if (_actionCountdown < 1.2f)
            {
                GetComponentInParent<Player>().PlayerData.PlayerBlocked = false;
            }
            _actionCountdown -= Time.deltaTime;
        }


        //IFrame (invulnerability after being hit) calculations
        if ((_IFrameCountdown <= 0.0f) && (GetComponentInParent<Player>().PlayerData.PlayerHit))
        {
            _IFrameCountdown = 20.0f;
            gameObject.layer = invincibleLayer;
        }
        else if (_IFrameCountdown > 0.0f)
        {
            _IFrameCountdown -= Time.deltaTime;
        }
        else
        {
            gameObject.layer = playerLayer;
        }

        GetComponentInParent<Player>().PlayerData.PlayerHit = false;


        //Stamina refreshing
        if (_timeSinceLastAction <= 0.0f && GetComponentInParent<Player>().PlayerData.PlayerStamina < GetComponentInParent<Player>().PlayerData.PlayerMaxStamina)
        {
            GetComponentInParent<Player>().PlayerData.PlayerStamina += 1;
        }

        if (_timeSinceLastAction > 0.0f)
        {
            _timeSinceLastAction -= Time.deltaTime;
        }

    }

    private void FixedUpdate()
    {
        Debug.DrawLine(transform.position, transform.position + transform.forward * 3.0f, Color.red);
        hitbox.transform.position = transform.position + transform.forward * 2.0f;
        blockbox.transform.position = transform.position + transform.forward * 2.0f;
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
        if (!_isAttacking && !_isBlocking)
        {
            if (GetComponentInParent<Player>().PlayerData.PlayerStamina >= 10)
            {
                _oldWalkSpeed = _baseWalkSpeed;
                _baseWalkSpeed = _baseWalkSpeed / 2;
                _isAttacking = true;
                hitbox.SetActive(true);
                _actionCountdown = 1.0f;
                _timeSinceLastAction = 5.0f;
                GetComponentInParent<Player>().PlayerData.PlayerStamina -= 10;
            }
        }
    }

    private void OnBlock()
    {
        if (!_isBlocking && !_isAttacking)
        {
            if (GetComponentInParent<Player>().PlayerData.PlayerStamina >= 10)
            {
                _oldWalkSpeed = _baseWalkSpeed;
                _baseWalkSpeed = 0.0f;
                _isBlocking = true;
                blockbox.SetActive(true);
                _actionCountdown = 2.0f;
                GetComponentInParent<Player>().PlayerData.PlayerStamina -= 10;
                GetComponentInParent<Player>().PlayerData.PlayerBlocked = true;
                _timeSinceLastAction = 5.0f;
            }
        }
    }

    /// <summary>
    ///     Triggered on Look input action
    /// </summary>
    private void OnLook(InputValue value)
    {
        if (!_isAttacking && !_isBlocking)
        {
            var vecValue = value.Get<Vector2>();
            if (_deviceClass == "Keyboard")
            {
                vecValue -= (Vector2)_camera.WorldToScreenPoint(transform.position);
            }
            transform.rotation = Quaternion.AngleAxis(Mathf.Rad2Deg * Mathf.Atan2(-vecValue.y, vecValue.x) + 90f, Vector3.up);
            hitbox.transform.rotation = Quaternion.AngleAxis(Mathf.Rad2Deg * Mathf.Atan2(-vecValue.y, vecValue.x) + 90f, Vector3.up);
            blockbox.transform.rotation = Quaternion.AngleAxis(Mathf.Rad2Deg * Mathf.Atan2(-vecValue.y, vecValue.x) + 90f, Vector3.up);
        }
    }
}