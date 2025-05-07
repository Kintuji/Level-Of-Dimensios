using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour, ICollectable
{
    public void Collect()
    {
        GameManager.instance.Addkeys();
        GameManager.instance.CheckIfHaveAllKeys();
        Destroy(gameObject);
    }
    
}
