using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour, ICollectable
{
    public void Collect()
    {
        Debug.Log("HealthPack Collected");
        Destroy(gameObject);
    }
}
