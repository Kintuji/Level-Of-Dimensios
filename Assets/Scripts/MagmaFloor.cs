using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagmaFloor : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();

        if (player != null)
        {
            player.Burning();
        }
    }
}
