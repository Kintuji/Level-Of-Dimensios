using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalLast : MonoBehaviour
{
    [SerializeField] private Transform _dreamSpawn;

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();

        if (player != null)
        {
            GameManager.instance.PlayerTeleportedForDreamSpawn = true;
            player.transform.position = _dreamSpawn.position;
            GameManager.instance.CheckGameWin();
        }
    }
}
