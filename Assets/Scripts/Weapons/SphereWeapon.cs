using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereWeapon : MonoBehaviour
{
    [SerializeField] private SphereBullet _sphereBulletPreFab;
    private bool _sphereSent;
    private MeshRenderer _sphereRenderer;

    private ObjPooling _objPulling;

    private void Awake()
    {
        _sphereRenderer = GetComponent<MeshRenderer>();
        _objPulling = GameObject.Find("ObjPulling").GetComponent<ObjPooling>();
    }

    public void Shoot()
    {
        if(_sphereSent == false)
        {
            //  SphereBullet sphereBullet = Instantiate(_sphereBulletPreFab, transform.position, Quaternion.identity);
            //  sphereBullet.Move(transform.forward);
            _objPulling.PoolBullet(gameObject.transform);
            _sphereRenderer.enabled = false;
            _sphereSent = true;
        }
        else
        {
            StartCoroutine(AttackCooldown());
        }
    }

    IEnumerator AttackCooldown()
    {
        
        yield return new WaitForSeconds(1f);
        _sphereSent = false;
        _sphereRenderer.enabled = true;
    }
}
