using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour, ICollectable
{
    private Action _CheckAllKeys;
    public Action CheckAllKeys { get => _CheckAllKeys; set => _CheckAllKeys = value; }

    public void Collect()
    {
        GameManager.instance.Addkeys();
        _CheckAllKeys?.Invoke();
        Destroy(gameObject);
    }
    
}
