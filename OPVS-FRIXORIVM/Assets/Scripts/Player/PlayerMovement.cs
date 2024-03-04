using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
///     Player Movement Handling
/// </summary>
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(RagdollController))]
public class PlayerMovement: PlayerController
{
    private static readonly int WalkSpeedHash = Animator.StringToHash("WalkSpeed");
    private static readonly int StrafeSpeedHash = Animator.StringToHash("StrafeSpeed");
    private static readonly int IsJumpingHash = Animator.StringToHash("IsJumping");
    private static readonly int IsFallingHash = Animator.StringToHash("IsFalling");
    private static readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");
    private static readonly int IsAttackingHash = Animator.StringToHash("IsAttacking");
    protected override string ActionMap => "Player Movement";
    
    private CharacterController _characterController;
    private Player _player;
    private Animator _animator;
    private RagdollController _ragdollController;
    
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
    public GameObject EquippedWeapon;
    public GameObject DefaultWeapon;

    private Quaternion _lookRotation;
    private Vector3 _direction;
    private float _actionCountdown;
    private float _timeSinceLastAction;
    private float _attackCooldown;
    private float _blockCooldown;
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
    private GameObject weapon;
    private GameObject defaultWeapon;

    private Vector3 rangedTarget = Vector3.zero;

    private List<GameObject> projectiles = new List<GameObject>();
    private List<Vector3> projectileTargets = new List<Vector3>();
    private int burstCount;

    protected override void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _characterController = GetComponent<CharacterController>();
        _player = GetComponentInParent<Player>();
        _deviceClass =  GetComponentInParent<PlayerInput>().devices[0].description.deviceClass;
        _ragdollController = GetComponent<RagdollController>();
        _camera = Camera.main;
        hitbox = Instantiate(AttackTarget, transform);
        blockbox = Instantiate(BlockVisual, transform);
        hitbox.SetActive(false);
        blockbox.SetActive(false);
        weapon = Instantiate(DefaultWeapon, transform);
        defaultWeapon = Instantiate(DefaultWeapon, transform);
        _oldWalkSpeed = _baseWalkSpeed;
        _actionCountdown = 0.0f;
        _iFrameCountdown = 0.0f;
        _timeSinceLastAction = 0.0f;
        _isAttacking = false;
        _playerLayer = LayerMask.NameToLayer("Player");
        _invincibleLayer = LayerMask.NameToLayer("Invincible");

        for (int i = 0; i < 5; i++)
        {
            projectiles.Add(Instantiate(AttackTarget));
            projectiles[i].transform.localScale = new Vector3(0.3f, 0.3f, 1.0f);
            projectileTargets.Add(projectiles[i].transform.position);
        }

        EquipWeapon();

        base.Awake(); 
    }

    private void Update()
    {
        UpdateWalk();

        if (_knockedOut) return;
        UpdateLook();
        ApplyGravity();
        ApplyMovement();
        UpdateAnimator();
        UpdateAttacks();
        UpdateBlocking();
        UpdateIFrames();
        UpdateAttackCooldown();
        UpdateBlockCooldown();
        UpdateStamina();
        UpdateProjectiles();

        if (weapon.GetComponent<WeaponData>().Type == WeaponData.WeaponType.Ranged && weapon.GetComponent<WeaponData>().AmmoCount <= 0)
        {
            weapon = defaultWeapon;
        }
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
        weapon.transform.rotation = _orientation;
    }

    /// <summary>
    ///     Apply movement to player controller
    /// </summary>
    private void ApplyMovement()
    {
        _characterController.Move(_velocity * Time.deltaTime);
        if (_characterController.isGrounded) _velocity.y = 0;
    }

    private void UpdateAnimator()
    {
        var localVelocity = transform.InverseTransformVector(_velocity);
        _animator.SetFloat(WalkSpeedHash, localVelocity.z / _baseWalkSpeed);
        _animator.SetFloat(StrafeSpeedHash, localVelocity.x / _baseWalkSpeed);
        _animator.SetBool(IsJumpingHash, _velocity.y > 0);
        _animator.SetBool(IsFallingHash, _velocity.y < 0);
        _animator.SetBool(IsAttackingHash, _isAttacking);
        _animator.SetBool(IsGroundedHash, _characterController.isGrounded);
    }

    /// <summary>
    ///     Attack Logic
    /// </summary>
    private void UpdateAttacks()
    {
        //Attacking calculations
        if (!_isAttacking)
            return;
        if (_actionCountdown <= 0.0f)
        {
            if (weapon.GetComponent<WeaponData>().Type != WeaponData.WeaponType.Ranged)
            {
                _isAttacking = false;
                _baseWalkSpeed = _oldWalkSpeed;
                hitbox.SetActive(false);
                _attackCooldown = weapon.GetComponent<WeaponData>().Cooldown;
            }
            else if (burstCount > 1)
            {
                burstCount -= 1;
                _actionCountdown = weapon.GetComponent<WeaponData>().UseTime;
                FireProjectile();
            }
            else
            {
                _isAttacking = false;
                _baseWalkSpeed = _oldWalkSpeed;
                _attackCooldown = weapon.GetComponent<WeaponData>().Cooldown;
                burstCount = weapon.GetComponent<WeaponData>().BurstAmount;
            }
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
            _blockCooldown = 5.0f;
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

    private void UpdateAttackCooldown()
    {
        if (!_isAttacking && _attackCooldown > 0.0f)
        {
            _attackCooldown -= Time.deltaTime;
        }
    }

    private void UpdateBlockCooldown()
    {
        if (!_isAttacking && _blockCooldown > 0.0f)
        {
            _blockCooldown -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        var transform1 = transform;
        var position = transform1.position;
        var forward = transform1.forward;
        Debug.DrawLine(position, position + forward * 3.0f, Color.red);
        if (!_isAttacking)
        {
            hitbox.transform.position = position + forward * (1.0f + (weapon.GetComponent<WeaponData>().Range.z / 2.0f));
        }
        //hitbox.transform.position += new Vector3(0.0f, 0.0f, weapon.GetComponent<WeaponData>().Range.z / 2);
        blockbox.transform.position = position + forward * 2.0f;
        weapon.transform.position = position + forward * 2.0f;
        
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
        if (_attackCooldown > 0.0f)
            return;
        if (_isAttacking || _isBlocking)
            return;
        if (_player.PlayerData.PlayerStamina < weapon.GetComponent<WeaponData>().StaminaCost)
            return;
        
        _oldWalkSpeed = _baseWalkSpeed;
        _baseWalkSpeed /= 2.0f;
        _isAttacking = true;
        _actionCountdown = weapon.GetComponent<WeaponData>().UseTime;
        _timeSinceLastAction = 5.0f;
        _player.PlayerData.PlayerStamina -= weapon.GetComponent<WeaponData>().StaminaCost;
        
        if (weapon.GetComponent<WeaponData>().Type == WeaponData.WeaponType.Ranged)
        {
            FireProjectile();
        }
        else
        {
            hitbox.SetActive(true);
        }
    }

    private void OnBlock()
    {
        if (_blockCooldown > 0.0f)
            return;
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
        _knockedOut = true;
        _ragdollController.EnableRagdoll();
        

        yield return new WaitForSeconds(_knockoutTime);

        _ragdollController.DisableRagdoll();
        _knockedOut = false;
    }

    private void EquipWeapon()
    {
        hitbox.gameObject.transform.localScale = weapon.GetComponent<WeaponData>().Range;
        burstCount = weapon.GetComponent<WeaponData>().BurstAmount;
    }

    private void UpdateProjectiles()
    {
        var speed = 12.0f;
        var step = speed * Time.deltaTime;

        for (int i = 0; i < 5; i++)
        {
            if (projectiles[i].transform.position != projectileTargets[i])
            {
                projectiles[i].transform.position = Vector3.MoveTowards(projectiles[i].transform.position, projectileTargets[i], step);
            }
            else if (projectiles[i].activeSelf == true)
            {
                projectiles[i].SetActive(false);
            }
        }
    }

    private void FireProjectile()
    {
        var transform1 = transform;
        var position = transform1.position;
        var forward = transform1.forward;

        for (int i = 0; i < 5; i++)
        {
            if (projectiles[i].transform.position == projectileTargets[i])
            {
                projectiles[i].transform.position = hitbox.transform.position;
                projectiles[i].transform.rotation = transform.rotation;
                projectiles[i].SetActive(true);
                projectileTargets[i] = position + forward * 15.0f;
                break;
            }
        }

        weapon.GetComponent<WeaponData>().AmmoCount -= 1;
    }
}