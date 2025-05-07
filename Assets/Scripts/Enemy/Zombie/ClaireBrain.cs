using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ClaireBrain : MonoBehaviour
{
    protected Transform _target;
    private List<Transform> _targetsInMemory;

    protected NavMeshAgent _agent;

    [SerializeField] protected float _maxDistance;
    [SerializeField] protected float _coneAngle;
    [SerializeField] protected float _distanceToAttack;

    [SerializeField] protected State _state;

    [SerializeField] Vector3 _idlePos;

    private Animator _animator;

    [SerializeField] private GameObject _key;

    private void Awake()
    {
        _state = State.Idle;
        _idlePos = transform.position;
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    protected enum State
    {
        None,
        Idle,
        Chasing,
        Attacking,
        Returning,
        Blinded
    }

    protected virtual void Update()
    {
        CheckState();
        ExecuteStateBehaviour();
        if (_key == null)
        {
            _coneAngle = 360;
            _maxDistance = 100;
            GameManager.instance.ActiveNavMeshLink();
        }
    }

    private void CheckState()
    {
        if (_target == null)
        {
            if (Vector3.Distance(_idlePos, transform.position) < 1)
            {
                _state = State.Idle;
                _animator.SetBool("FoundPlayer", false);
            }
            else
            {
                _state = State.Returning;
            }
        }
        else if (Vector3.Distance(_target.position, transform.position) < _distanceToAttack)
        {
            _state = State.Attacking;
            StartCoroutine(AttackingCooldown());
        }
        else
        {
            _state = State.Chasing;
        }
    }




    private void ExecuteStateBehaviour()
    {

        if (_state == State.Idle)
        {
            //idle logic
            Search();
        }
        else if (_state == State.Chasing)
        {
            Chasing();
            if (Vector3.Distance(_target.position, transform.position) > _maxDistance)
            {
                _target = null;
                _animator.SetBool("FoundPlayer", false);
            }
            //check range
        }
        else if (_state == State.Returning)
        {
            Returning();
            Search();
        }
        if (_state == State.Attacking)
        {
           // _animator.SetTrigger("Attacking");
        }
    }
    private void Returning()
    {
        _agent.SetDestination(_idlePos);
        _animator.SetBool("FoundPlayer", true);
    }

    private void Chasing()
    {
        _agent.SetDestination(_target.position);
        _animator.SetBool("FoundPlayer", true);
    }

    void Search()
    {
        Collider[] hitColliders = new Collider[100];
        _targetsInMemory = new List<Transform>();
        hitColliders = Physics.OverlapSphere(transform.position, _maxDistance);

        for (var i = 0; i < hitColliders.Length; i++)
        {
            Transform tempTarget = hitColliders[i].transform;

            Vector3 direction = tempTarget.position - transform.position;

            if (Vector3.Angle(direction, transform.forward) <= _coneAngle / 2)
            {
                Player player = tempTarget.GetComponent<Player>();
                if (player != null)
                {
                    _target = player.transform;
                }
            }
        }
    }

    IEnumerator AttackingCooldown()
    {
        yield return new WaitForSeconds(3);

        _state = State.Chasing;
    }
}
