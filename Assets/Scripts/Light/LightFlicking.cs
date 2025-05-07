using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicking : MonoBehaviour
{
    Light myLight;
    void Start() 
    { 
        myLight = GetComponent<Light>(); 
    }
    void Update() 
    { 
        myLight.intensity = Random.Range(0.5f, 1.5f); 
    }

}
