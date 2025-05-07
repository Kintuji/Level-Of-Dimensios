using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MannequinBrain : MonoBehaviour
{
    protected Transform _target;
    private List<Transform> _targetsInMemory;
    [SerializeField] private List<Transform> _poinsToRun;
    [SerializeField] private Transform _currentPoint;

    protected NavMeshAgent _agent;

    [SerializeField] protected float _maxDistance;
    [SerializeField] protected float _coneAngle;
    [SerializeField] protected float _distanceToAttack;

    [SerializeField] protected State _state;

    private Animator _animator;
    private float movingSpeed;

    private bool isAttacking;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();

        if (_poinsToRun.Count > 0)
        {
            _currentPoint = _poinsToRun[0]; 
            _agent.SetDestination(_currentPoint.position); 
        }
    }
    protected enum State
    {
        None,
        Running,
        Chasing,
        Attacking,
        Blinded
    }
    protected virtual void Update()
    {
        CheckState();
        ExecuteStateBehavior();
    }

    private void CheckState()
    {
        if (_target == null)
        {
            _state = State.Running;
             Search();
        }
        else if (Vector3.Distance(transform.position, _target.position) < _distanceToAttack)
        {
            _state = State.Attacking;
            //attacking
        }
        else
        if(Vector3.Distance(transform.position, _target.position) < _maxDistance) 
        {
            _state = State.Chasing;
        }
        else
        {
            _state = State.Running;
        }
        
    }
    private void ExecuteStateBehavior()
    {
        if (_state == State.Running)
        {
            Move();
        }
        else if (_state == State.Chasing)
        {
            Chasing();
        }
        else if(_state == State.Attacking)
        {
            if(isAttacking == false)
            {
                StartCoroutine(Attack());
            }

        }
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        _animator.SetTrigger("attackTrigger");
        yield return new WaitForSeconds(2);
        isAttacking = false;

    }

    private void Chasing()
    {
        _agent.SetDestination(_target.position);
    }

    private void Move()
    {
        if (_poinsToRun == null || _poinsToRun.Count == 0) return; 
        if (_currentPoint == null) _currentPoint = _poinsToRun[0]; 

        float distance = Vector3.Distance(_currentPoint.position, transform.position);
        _agent.SetDestination(_currentPoint.position); 

        if (distance < 1f) 
        {
            int index = _poinsToRun.IndexOf(_currentPoint);

            if (index == -1) index = 0; 
            index = (index + 1) % _poinsToRun.Count; 

            _currentPoint = _poinsToRun[index];
            _agent.SetDestination(_currentPoint.position); 
        }
    }
    void Search()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _maxDistance);
        bool foundPlayer = false;

        for (var i = 0; i < hitColliders.Length; i++)
        {
            Transform tempTarget = hitColliders[i].transform;
            Vector3 direction = tempTarget.position - transform.position;

            Player player = tempTarget.GetComponent<Player>();

            if (player != null)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, direction, out hit, _maxDistance, LayerMask.GetMask("Player")))
                {
                    if (hit.collider.CompareTag("Player")) 
                    {
                        _target = player.transform;
                        foundPlayer = true;
                        break;
                    }
                }
            }
        }
        if (!foundPlayer)
        {
            _target = null;
        }
    }

}
