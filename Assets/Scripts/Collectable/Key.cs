using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour, ICollectable
{
    public static Action _onKeyCollected;

    public void Collect()
    {
        //GameManager.instance.Addkeys();
        _onKeyCollected?.Invoke();
        Destroy(gameObject);
    }
    
}
