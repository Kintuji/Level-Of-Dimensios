using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalOne : MonoBehaviour
{
    [SerializeField] private Transform _secondLevelSpawn;
    private Action _levelOneDone;

    public Action LevelOneDone { get => _levelOneDone; set => _levelOneDone = value; }

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();

        if (player != null)
        {
            player.transform.position = _secondLevelSpawn.position;
            GameManager.instance.PlayerTeleportForLevelTwo = true;
            _levelOneDone?.Invoke();
            GameManager.instance.CheckLevelOneWin();
        }
    }
}
