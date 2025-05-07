using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalOne : MonoBehaviour
{
    [SerializeField] private Transform _secondLevelSpawn;
    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();

        if (player != null)
        {
            player.transform.position = _secondLevelSpawn.position;
            GameManager.instance.PlayerTeleportForLevelTwo = true;
            GameManager.instance.CheckLevelOneWin();
        }
    }
}
