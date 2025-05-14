#region Namespaces
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
#endregion

public class Player : MonoBehaviour, IDamageable
{
    #region EnumsStateMachine
    private enum State 
    { 
        None, 
        Grounded, 
        Walled, 
        OnAir, 
        RunningOnWall 
    }
    
    #endregion

    #region Movement Settings
    [Header("Movement Settings")]
    [SerializeField] private float _currentMovementSpeed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private float groundCheckRadius;

    private float _originalMovementSpeed = 5;
    private float _sprintMovementSpeed = 8;
    private State _currentState;
    private bool isGrounded;
    private bool isOnWall;
    #endregion

    #region Stats
    [Header("Stats")]
    [SerializeField] private float _hp;
    private bool _isPlayerAlive = true;
    private bool _haveFlashLight = true;
    private bool _haveSphereWeapon;
    private int isBurning;
    private IEnumerator _currentBurnCoroutine;
    #endregion

    #region References
    [Header("My References")]
    private Rigidbody _rigidBody;
    private Animator _animator;
    private FlashLight _flashLight;
    private PickupClass _pickupClass;
    private SphereWeapon _sphereWeapon;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    #endregion

    #region Input System
    private PlayerInput _playerInput;
    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _lightAction;
    private InputAction _shootAction;
    private Vector2 _currentMovementInput;
    #endregion

    #region Events
    private Action _onDeath;
    public Action OnDeath { get => _onDeath; set => _onDeath = value; }
    #endregion

    #region Properties
    public Rigidbody RigidBody => _rigidBody;
    public bool HaveFlashLight { get => _haveFlashLight; set => _haveFlashLight = value; }
    public float Hp { get => _hp; set => _hp = value; }
    public bool HaveSphereWeapon { get => _haveSphereWeapon; set => _haveSphereWeapon = value; }
    public bool IsPlayerAlive { get => _isPlayerAlive; set => _isPlayerAlive = value; }
    #endregion

    #region MonoBehaviour
    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _flashLight = GetComponentInChildren<FlashLight>();
        _pickupClass = GetComponent<PickupClass>();
        _sphereWeapon = GetComponentInChildren<SphereWeapon>();
        _animator = GetComponent<Animator>();
        _playerInput = GetComponent<PlayerInput>();

        _moveAction = _playerInput.actions["Movement"];
        _jumpAction = _playerInput.actions["Jump"];
        _lightAction = _playerInput.actions["Light"];
        _shootAction = _playerInput.actions["Shoot"];
    }

    private void OnEnable()
    {
        _moveAction.performed += OnMovementInput;
        _moveAction.canceled += OnMovementInput;
        _jumpAction.performed += OnJumpInput;
        _lightAction.performed += OnLightInput;
        _shootAction.performed += OnShootInput;
    }

    private void OnDisable()
    {
        _moveAction.performed -= OnMovementInput;
        _moveAction.canceled -= OnMovementInput;
        _jumpAction.performed -= OnJumpInput;
        _lightAction.performed -= OnLightInput;
        _shootAction.performed -= OnShootInput;
    }

    private void Update()
    {
        if (!GameManager.instance.PlayerTeleportedForDreamSpawn)
        {
            GroundCheck();
            StateMachineLayersCheck();
            Sprint();

            if (_haveFlashLight && _flashLight != null && _flashLight.IsFlashLightOn)
                _flashLight.Blind();
        }
    }

    private void FixedUpdate()
    {
        Move(_currentMovementInput);
    }
    #endregion

    #region InputHandlers
    private void OnMovementInput(InputAction.CallbackContext context)
    {
        _currentMovementInput = context.ReadValue<Vector2>();
    }

    private void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.performed && _currentState == State.Grounded)
            Jump();
    }

    private void OnLightInput(InputAction.CallbackContext context)
    {
        if (context.performed)
            FlashLight();
    }

    private void OnShootInput(InputAction.CallbackContext context)
    {
        if (context.performed)
            Shoot();
    }
    #endregion

    #region Movement & Actions
    private void Move(Vector2 direction)
    {
        Vector3 moveDir = (transform.right * direction.x + transform.forward * direction.y).normalized;
        Vector3 velocity = moveDir * _currentMovementSpeed;
        velocity.y = _rigidBody.velocity.y;
        _rigidBody.velocity = velocity;
    }

    private void Jump()
    {
        _rigidBody.velocity = Vector3.zero;
        _rigidBody.AddForce(Vector2.up * _jumpForce, ForceMode.Impulse);
    }

    private void Sprint()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
            _currentMovementSpeed = _sprintMovementSpeed;
        else if (Input.GetKeyUp(KeyCode.LeftShift))
            _currentMovementSpeed = _originalMovementSpeed;
    }
    #endregion

    #region Abilities
    private void FlashLight()
    {
        if (_haveFlashLight)
            _flashLight.TurnOnAndOff();
    }

    private void Shoot()
    {
        if (_haveSphereWeapon)
            _sphereWeapon.Shoot();
    }

    private void Interact()
    {
        // _pickupClass.PickUp();
    }
    #endregion

    #region Environment Checks
    private void GroundCheck()
    {
        isGrounded = Physics.SphereCast(transform.position, 0.9f, Vector2.down, out _, groundCheckDistance, groundLayer);
    }

    private void StateMachineLayersCheck()
    {
        if (isGrounded)
            _currentState = State.Grounded;
        else if (isOnWall)
            _currentState = _rigidBody.velocity.magnitude > 5 ? State.RunningOnWall : State.Walled;
        else
            _currentState = State.OnAir;

        if (_currentState == State.RunningOnWall)
        {
            _rigidBody.useGravity = false;
            Vector3 finalVelocity = _rigidBody.velocity;
            finalVelocity.y = 0;
            _rigidBody.velocity = finalVelocity;
        }
        else
        {
            _rigidBody.useGravity = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, groundCheckRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position - Vector3.up * groundCheckDistance, groundCheckRadius);
    }
    #endregion

    #region Combat & Damage
    public void TakeDamage(int damage)
    {
        if (!_isPlayerAlive) return;

        _hp -= damage;
        UIManager.instance.UpdateHP(_hp, 100);

        if (_hp <= 0)
        {
            _isPlayerAlive = false;
            _onDeath?.Invoke();
        }
    }

    public void RestoreHP()
    {
        _hp = 100;
        UIManager.instance.UpdateHP(_hp, 100);
    }

    public IEnumerator SlowDown(float movementSpeed, float time)
    {
        _currentMovementSpeed = movementSpeed;
        yield return new WaitForSeconds(time);
        _currentMovementSpeed = _originalMovementSpeed;
    }

    public void Burning()
    {
        if (!_isPlayerAlive) return;

        if (_currentBurnCoroutine != null)
            StopCoroutine(_currentBurnCoroutine);

        _currentBurnCoroutine = BurnDamage();
        StartCoroutine(_currentBurnCoroutine);
    }

    private IEnumerator BurnDamage()
    {
        isBurning = 0;
        while (isBurning < 3)
        {
            yield return new WaitForSeconds(1);
            TakeDamage(8);
            isBurning++;
        }
    }
    #endregion

    #region Collisions
    private void OnTriggerEnter(Collider other)
    {
        ICollectable collectable = other.GetComponent<ICollectable>();
        collectable?.Collect();
    }
    #endregion

    #region Animation
    public void PlayerAnimation()
    {
        _animator.SetBool("WakeUp", true);
    }
    #endregion
}
