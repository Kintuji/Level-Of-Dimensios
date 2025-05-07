using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private Rigidbody _rigidBody;
    [SerializeField] private float _bulletSpeed;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        Destroy(gameObject, 5);
    }
    public void Move(Vector3 direction)
    {
        //transform.Translate(direction * _bulletSpeed * Time.deltaTime);
        _rigidBody.velocity = direction.normalized * _bulletSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Player playerCollided = collision.collider.gameObject.GetComponent<Player>();

        if (playerCollided != null)
        {
            playerCollided.TakeDamage(17);
            Destroy(gameObject);
        }
    }
}
