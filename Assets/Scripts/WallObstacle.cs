using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallObstacle : MonoBehaviour
{
    [SerializeField] GameObject wallObstacle;
    public void DestroyWall()
    {
        Destroy(wallObstacle);
    }
}
