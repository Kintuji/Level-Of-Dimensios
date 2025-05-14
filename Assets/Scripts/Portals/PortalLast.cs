using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PortalLast : MonoBehaviour
{
    [SerializeField] private Transform _dreamSpawn;
    [SerializeField] private UnityEvent _onWin;

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();

        if (player != null)
        {
            GameManager.instance.PlayerTeleportedForDreamSpawn = true;
            player.transform.position = _dreamSpawn.position;
            _onWin?.Invoke();
        }
    }
}
