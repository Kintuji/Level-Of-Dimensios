using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platforms : MonoBehaviour
{
    private MeshRenderer _meshRender;

    private void Awake()
    {
        _meshRender = GetComponent<MeshRenderer>();
    }

    public void ActiveMeshRender()
    {
        _meshRender.enabled = true;
    }
}
