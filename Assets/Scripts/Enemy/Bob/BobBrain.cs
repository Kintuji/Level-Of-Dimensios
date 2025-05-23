using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BobBrain : MonoBehaviour
{
    private Transform _target;
    private List<Transform> _targetsInMemory;

    [SerializeField] private float _maxDistance;
    [SerializeField] private float _coneAngle;

    [SerializeField] private State _state;

    protected Animator _animator;

    [SerializeField] private EnemyBullet _bulletPrefab;
    private bool haveShooted;

    [SerializeField] private Transform _bulletPosition;

    private ObjPooling _objPulling;
    private BobWeaponPos _weaponPos;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _objPulling = GameObject.Find("ObjPulling").GetComponent<ObjPooling>();
        _weaponPos = GetComponentInChildren<BobWeaponPos>();
    }

    private enum State
    {
        None,
        Idle,
        Attacking
    }

    private void Update()
    { 
            CheckState();
            ExecuteStateBehavior();   
    }

    private void ExecuteStateBehavior()
    {
        if (_state == State.Idle)
        {
            Search();
            CancelInvoke("Shoot");
            _animator.SetBool("isShooting", false);
        }
        else 
        if (_state == State.Attacking) 
        {
            RotateTowardTarget();
            _animator.SetBool("isShooting", true);

            if (haveShooted == true)
            {
               // _objPulling.PoolEnemyBullet(_weaponPos.transform);
                InvokeRepeating("Shoot", 2, 2);

                haveShooted = false;
            }
        }
    }

    private void CheckState()
    {
        if (_target == null)
        {
            _state = State.Idle;
        }
        else if (Vector3.Distance(transform.position, _target.position) > _maxDistance)
        {
            _state = State.Idle;
            _target = null; 
        }
        else if (Vector3.Distance(transform.position, _target.position) <= _maxDistance)
        {
            _state = State.Attacking;
        }
    }


    private void Search()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _maxDistance);
        bool foundPlayer = false;

        for (var  i = 0; i < hitColliders.Length; i++)
        {
            Transform tempTarget = hitColliders[i].transform;
            Vector3 direction = tempTarget.position - transform.position;

            if (Vector3.Angle(direction, transform.forward) <= _coneAngle / 2)
            {
                Player player = tempTarget.GetComponent<Player>();
                if (player != null)
                {
                    _target = player.transform;
                    foundPlayer = true;
                    haveShooted = true;
                }
            }
        }
        if (!foundPlayer)
        {
            _target = null;
        }
    }

    private void RotateTowardTarget()
    {
        if (_target == null) return;

        Vector3 direction = (_target.position - transform.position).normalized;
        direction.y = 0; 

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    protected virtual void Shoot()
    {
        if (_target == null) return;

        Vector3 directionToPlayer = (_target.position - _bulletPosition.position).normalized;
        Quaternion bulletRotation = Quaternion.LookRotation(directionToPlayer);

        EnemyBullet currentBullet = Instantiate(_bulletPrefab, _bulletPosition.position, bulletRotation);
        currentBullet.Move(directionToPlayer);
    }

}
