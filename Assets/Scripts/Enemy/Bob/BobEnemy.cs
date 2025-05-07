using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BobEnemy : BobBrain, IDamageable
{
    [SerializeField] private float _hp;
    public void TakeDamage(int damage)
    {
        _hp -= damage;

        if(_hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
