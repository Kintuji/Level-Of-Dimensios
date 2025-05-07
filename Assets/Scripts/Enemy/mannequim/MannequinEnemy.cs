using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MannequinEnemy : MannequinBrain, IDamageable, IBlindable
{
    [SerializeField] private float _hp;

    private void OnCollisionEnter(Collision collision)
    {
        IDamageable target = collision.gameObject.GetComponent<IDamageable>();

        if (target != null && !collision.gameObject.CompareTag("Enemy"))
        {
            target.TakeDamage(17);
        }
    }
    public void TakeDamage(int damage)
    {
        _hp -= damage;

        if( _hp < 0)
        {
            Destroy(gameObject);
        }
    }
    IEnumerator IBlindable.Blinded()
    {
        _agent.speed = 0;
        _state = State.Blinded;
        yield return new WaitForSeconds(2);
        _agent.speed = 10;
    }
}
