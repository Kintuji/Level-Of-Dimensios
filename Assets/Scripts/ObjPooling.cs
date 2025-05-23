using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjPooling : MonoBehaviour
{
    [SerializeField] private SphereBullet _sphereBulletPreFab;
    private int _sphereTotalBullets = 1;

    private List<SphereBullet> _sphereBulletPool = new List<SphereBullet>();

    [SerializeField] private EnemyBullet _enemyBulletPreFab;
    private int _enemyTotalBullets = 3;
    private List<EnemyBullet> _enemyBulletPool = new List<EnemyBullet>();

    private void Start()
    {
        for(int i = 0; i < _sphereTotalBullets; i++)
        {
            SphereBullet sphereBullet = Instantiate(_sphereBulletPreFab);
            sphereBullet.gameObject.SetActive(false);
            _sphereBulletPool.Add(sphereBullet);
        }

        for(int i = 0; i < _enemyTotalBullets; i++)
        {
            EnemyBullet enemyBullet = Instantiate(_enemyBulletPreFab);
            enemyBullet.gameObject.SetActive(false);
            _enemyBulletPool.Add(enemyBullet);
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
        SphereBullet sphereBullet = Instantiate(_sphereBulletPreFab, weaponPos.position, weaponPos.rotation);

        sphereBullet.gameObject.SetActive(true);
        _sphereBulletPool.Add(sphereBullet);
        return sphereBullet;
    }
    public EnemyBullet PoolEnemyBullet(Transform weaponPos)
    {
        for (var i = 0; i < _enemyBulletPool.Count; i++)
        {
            if (!_enemyBulletPool[i].gameObject.activeInHierarchy)
            {
                _enemyBulletPool[i].gameObject.SetActive(true);
                _enemyBulletPool[i].gameObject.transform.position = weaponPos.transform.position;
                _enemyBulletPool[i].gameObject.transform.rotation = weaponPos.transform.rotation;
                return _enemyBulletPool[i];
            }
        }
        EnemyBullet enemyBullet = Instantiate(_enemyBulletPreFab, weaponPos.position, weaponPos.rotation);
        enemyBullet.gameObject.SetActive(true);
        _enemyBulletPool.Add(enemyBullet);
        return enemyBullet;
    }
}
