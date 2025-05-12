using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjPooling : MonoBehaviour
{
    [SerializeField] private SphereBullet _sphereBulletPreFab;
    private int _sphereTotalBullets = 1;

    private List<SphereBullet> _sphereBulletPool = new List<SphereBullet>();

    private void Start()
    {
        for(int i = 0; i < _sphereTotalBullets; i++)
        {
            SphereBullet sphereBullet = Instantiate(_sphereBulletPreFab);
            sphereBullet.gameObject.SetActive(false);
            _sphereBulletPool.Add(sphereBullet);
        }
    }
    public SphereBullet PoolBullet(Transform weaponPos)
    {
        for (var i = 0; i < _sphereBulletPool.Count; i++)
        {
            if (!_sphereBulletPool[i].gameObject.activeInHierarchy)
            {
                print("estava dentro");
                _sphereBulletPool[i].gameObject.SetActive(true);
                _sphereBulletPool[i].gameObject.transform.position = weaponPos.transform.position;
                _sphereBulletPool[i].gameObject.transform.rotation = weaponPos.transform.rotation;
                return _sphereBulletPool[i];
            }
           
        }
        SphereBullet sphereBullet = Instantiate(_sphereBulletPreFab, weaponPos);
        sphereBullet.gameObject.SetActive(true);
        _sphereBulletPool.Add(sphereBullet);
        return sphereBullet;
    }
}
