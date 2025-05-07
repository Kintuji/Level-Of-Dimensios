using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour, IDamageable
{
    public void TakeDamage(int damage)
    {
        Debug.Log("BOOM B*TCH!");
    }
}
