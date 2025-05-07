using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereBullet : MonoBehaviour
{
    private Rigidbody _rb;
    [SerializeField] private float bulletSpeed;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        Destroy(gameObject,5);
    }

    public void Move(Vector3 direction)
    {
        Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();
        rigidbody.velocity = direction.normalized * bulletSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        IDamageable enemies = collision.gameObject.GetComponent<IDamageable>();

        if (enemies != null)
        {
            enemies.TakeDamage(20);
        }
    }
}
