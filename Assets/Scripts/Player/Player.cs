#region Namespaces/Directives

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#endregion

public class Player : MonoBehaviour, IDamageable
{
    #region States
    private enum State
    {
        None,
        Grounded,
        Walled,
        OnAir,
        RunningOnWall
    }
    #endregion

    #region Declarations

    [Header("Movement Settings")]
    [SerializeField] private float _currentMovementSpeed;
    private float _originalMovementSpeed = 5;
    private float _sprintMovementSpeed = 8;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private State _currentState;

    [Header("Stats")]
    [SerializeField] private float _hp;
    private bool _isPlayerAlive;
    private bool _haveFlashLight = true;
    private bool _haveSphereWeapon;

    [Header("My References")]
    private Rigidbody _rigidBody;
    private Animator _animator;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float groundCheckRadius;
    private bool isGrounded;
    private bool isOnWall;
    private FlashLight _flashLight;
    private bool _flashLightOn;

    private int isBurning;
    private IEnumerator _currentBurnCoroutine;

    private PickupClass _pickupClass;
    private SphereWeapon _sphereWeapon;

    private 

    public Rigidbody RigidBody { get => _rigidBody; set => _rigidBody = value; }
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
    }

    void Update()
    {
        if (GameManager.instance.PlayerTeleportedForDreamSpawn == false)
        {
            GroundCheck();
            if (isGrounded)
            {
                _currentState = State.Grounded;
            }
            else if (isOnWall)
            {
                if (_rigidBody.velocity.magnitude > 5)
                {
                    _currentState = State.RunningOnWall;
                }
                else
                {
                    _currentState = State.Walled;
                }
            }
            else
            {
                _currentState = State.OnAir;
            }

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
            //Debug.Log(_rigidBody.velocity.magnitude);
            PlayerInput();
            MoveInDirection(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
            Sprint();

            if (_haveFlashLight && _flashLight != null && _flashLight.IsFlashLightOn)
            {
                _flashLight.Blind();
            }
        }
    }

    #endregion

    private void PlayerInput()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            FlashLight();
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Shoot();
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (_currentState == State.Grounded)
            {
                Jump();
            }
            else if (_currentState == State.Walled)
            {
                _rigidBody.velocity = Vector3.zero;
                _rigidBody.AddForce(Vector2.up * _jumpForce * 15, ForceMode.Impulse);
            }
        }
    }

    private void Shoot()
    {
        if (_haveSphereWeapon == true)
        {
        _sphereWeapon.Shoot();
        }
    }
    private void Interact()
    {
     //   _pickupClass.PickUp();
    }
    private void FlashLight()
    {
        if (_haveFlashLight == true)
        {
        _flashLight.TurnOnAndOff();
        }
    }
    private void Sprint()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _currentMovementSpeed = _sprintMovementSpeed;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _currentMovementSpeed = _originalMovementSpeed;
        }
    }
    private void GroundCheck()
    {
        RaycastHit hit1;

        if (Physics.SphereCast(transform.position, 0.9f, Vector2.down, out hit1, groundCheckDistance, groundLayer))
        {
            isGrounded = true;
           // Debug.Log("isGrounded True");
        } else
        {
           // Debug.Log("isGrounded False");
            isGrounded = false;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, groundCheckRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position - Vector3.up * groundCheckDistance, groundCheckRadius);
    }
    private void Jump()
    {  
        _rigidBody.velocity = Vector3.zero;
        _rigidBody.AddForce(Vector2.up * _jumpForce, ForceMode.Impulse);
    }

    private void MoveInDirection(Vector2 direction)
    {
        Vector3 finalVelocity = (direction.x * transform.right + direction.y * transform.forward).normalized * _currentMovementSpeed;

        finalVelocity.y = _rigidBody.velocity.y;
        _rigidBody.velocity = finalVelocity;
    }
    public void TakeDamage(int damage)
    {
       if (_isPlayerAlive == true)
       {
            _hp -= damage;
            UIManager.instance.UpdateHP(_hp, 100);
        }
       if (_hp <= 0)
       {
            _isPlayerAlive = false;
            GameManager.instance.CheckPlayerDead();
       }
        
    }
    public IEnumerator SlowDown(float movementSpeed, float time)
    {
        _currentMovementSpeed = movementSpeed;
        yield return new WaitForSeconds(time);
        _currentMovementSpeed = _originalMovementSpeed;
    }

    public void Burning()
    {
        if (_isPlayerAlive == true)
        {
            if (_currentBurnCoroutine != null)
            {
                StopCoroutine(_currentBurnCoroutine);
            }
            _currentBurnCoroutine = BurnDamage();
            StartCoroutine(_currentBurnCoroutine);
        }
    }
    IEnumerator BurnDamage()
    {
        isBurning = 0;

        while (isBurning < 3)
        {
            yield return new WaitForSeconds(1);
            TakeDamage(8);
            isBurning++;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        ICollectable collectable = other.GetComponent<ICollectable>();

        if (collectable != null)
        {
            collectable.Collect();
        }
    }
    public void RestoreHP()
    {
        _hp = 100;
        UIManager.instance.UpdateHP(_hp, 100);
    }

    public void PlayerAnimation()
    {
        _animator.SetBool("WakeUp", true);
    }
}
