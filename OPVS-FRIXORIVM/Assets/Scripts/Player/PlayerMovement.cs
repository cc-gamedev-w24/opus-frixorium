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
    private Player _player;
    
    /// <summary>
    ///     Base player movement speed
    /// </summary>
    [SerializeField] private float _baseWalkSpeed = 2.0f;

    [SerializeField]
    private float _accelerationSpeed = 2.0f;

    [SerializeField]
    private float _decelerationSpeed = 4.0f;
    
    // TODO:
    private float _oldWalkSpeed;
    
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

    public GameObject AttackTarget;
    public GameObject BlockVisual;

    private Quaternion _lookRotation;
    private Vector3 _direction;
    private float _actionCountdown;
    private float _timeSinceLastAction;
    private bool _isAttacking;
    private bool _isBlocking;
    private float _iFrameCountdown;
    private int _invincibleLayer;
    private int _playerLayer;

    /// <summary>
    ///     Type of device controlling this player
    /// </summary>
    private string _deviceClass;
    
    private bool _knockedOut;
    private Camera _camera;
    private Quaternion _orientation;
    private Vector2 _walkValue;
    
    private GameObject hitbox;
    private GameObject blockbox;

    protected override void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _rigidbody = GetComponent<Rigidbody>();
        _player = GetComponentInParent<Player>();
        _rigidbody.isKinematic = true;
        _deviceClass =  GetComponentInParent<PlayerInput>().devices[0].description.deviceClass;
        _camera = Camera.main;
        hitbox = Instantiate(AttackTarget, transform);
        blockbox = Instantiate(BlockVisual, transform);
        hitbox.SetActive(false);
        blockbox.SetActive(false);
        _oldWalkSpeed = _baseWalkSpeed;
        _actionCountdown = 0.0f;
        _iFrameCountdown = 0.0f;
        _timeSinceLastAction = 0.0f;
        _isAttacking = false;
        _playerLayer = LayerMask.NameToLayer("Player");
        _invincibleLayer = LayerMask.NameToLayer("Invincible");
        base.Awake(); 
    }

    private void Update()
    {
        UpdateWalk();
        
        if (_knockedOut) return;
        UpdateLook();
        ApplyGravity();
        ApplyMovement();
        UpdateAttacks();
        UpdateBlocking();
        UpdateIFrames();
        UpdateStamina();
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
        hitbox.transform.rotation = _orientation;
        blockbox.transform.rotation = _orientation;
    }

    /// <summary>
    ///     Apply movement to player controller
    /// </summary>
    private void ApplyMovement()
    {
       _characterController.Move(_velocity * Time.deltaTime);
        if (_characterController.isGrounded) _velocity.y = 0;
    }

    /// <summary>
    ///     Attack Logic
    /// </summary>
    private void UpdateAttacks()
    {
        //Attacking calculations
        if (!_isAttacking)
            return;
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

    private void UpdateBlocking()
    {
        //Blocking calculations
        if (!_isBlocking)
            return;
        if (_actionCountdown <= 0.0f)
        {
            _isBlocking = false;
            _baseWalkSpeed = _oldWalkSpeed;
            blockbox.SetActive(false);
        }
        else if (_actionCountdown < 1.2f)
        {
            _player.PlayerData.PlayerBlocked = false;
        }
        _actionCountdown -= Time.deltaTime;
    }

    private void UpdateIFrames()
    {
            //IFrame (invulnerability after being hit) calculations
        switch (_iFrameCountdown)
        {
            case <= 0.0f when _player.PlayerData.PlayerHit:
                _iFrameCountdown = 20.0f;
                gameObject.layer = _invincibleLayer;
                break;
            case > 0.0f:
                _iFrameCountdown -= Time.deltaTime;
                break;
            default:
                gameObject.layer = _playerLayer;
                break;
        }

        _player.PlayerData.PlayerHit = false;
    }

    private void UpdateStamina()
    {
        //Stamina refreshing
        if (_timeSinceLastAction <= 0.0f && _player.PlayerData.PlayerStamina < _player.PlayerData.PlayerMaxStamina)
        {
            _player.PlayerData.PlayerStamina += 1;
        }

        if (_timeSinceLastAction > 0.0f)
        {
            _timeSinceLastAction -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        var transform1 = transform;
        var position = transform1.position;
        var forward = transform1.forward;
        Debug.DrawLine(position, position + forward * 3.0f, Color.red);
        hitbox.transform.position = position + forward * 2.0f;
        blockbox.transform.position = position + forward * 2.0f;
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

    private void OnAttack()
    {
        if (_isAttacking || _isBlocking)
            return;
        if (_player.PlayerData.PlayerStamina < 10)
            return;
        
        _oldWalkSpeed = _baseWalkSpeed;
        _baseWalkSpeed /= 2.0f;
        _isAttacking = true;
        hitbox.SetActive(true);
        _actionCountdown = 1.0f;
        _timeSinceLastAction = 5.0f;
        _player.PlayerData.PlayerStamina -= 10;
    }

    private void OnBlock()
    {
        if (_isBlocking || _isAttacking)
            return;
        if (_player.PlayerData.PlayerStamina < 10)
            return;
        _oldWalkSpeed = _baseWalkSpeed;
        _baseWalkSpeed = 0.0f;
        _isBlocking = true;
        blockbox.SetActive(true);
        _actionCountdown = 2.0f;
        _player.PlayerData.PlayerStamina -= 10;
        _player.PlayerData.PlayerBlocked = true;
        _timeSinceLastAction = 5.0f;
    }

    /// <summary>
    ///     Triggered on Look input action
    /// </summary>
    private void OnLook(InputValue value)
    {
        if (_knockedOut) return;
        if (_isBlocking || _isAttacking) return;
        
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