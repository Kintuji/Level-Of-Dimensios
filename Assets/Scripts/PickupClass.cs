using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupClass : MonoBehaviour
{
    [SerializeField] private LayerMask pickupLayer;
    [SerializeField] private float pickupRange;
    [SerializeField] private Transform hand;

    [SerializeField] private FlashLight _flashLight;

    private Rigidbody currentObjectRB;
    private Collider currentObjectCollider;
    private Transform currentObject;

    public void PickUp()
    {
        Ray pickupRay = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(pickupRay, out hit, pickupRange, pickupLayer))
        {
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward, Color.red, 2);

            if (currentObject) 
            {
                currentObject.SetParent(null);
                _flashLight.gameObject.SetActive(true);
                currentObjectRB.isKinematic = false;
                currentObjectCollider.enabled = true;
                //currentObjectRB.transform.SetParent(hand, false);
            }
            
            currentObject = hit.transform;
            currentObjectRB = hit.rigidbody;
            currentObjectCollider = hit.collider;
            _flashLight.gameObject.SetActive(false);

            currentObjectRB.isKinematic = true;
            currentObjectCollider.enabled = false;

            currentObject.transform.position = hand.position;
            currentObject.parent = hand;


            //currentObjectRB = hit.rigidbody;
            //currentObjectCollider = hit.collider;

            //currentObjectRB.isKinematic = true;
            //currentObjectCollider.enabled = false;

            
            currentObjectRB.position = hand.position;
            currentObjectRB.rotation = hand.rotation;
            //currentObjectRB.gameObject.transform.SetParent(hand);
            return;
        }

        if (currentObject)
        {
            currentObject.SetParent(null);
            _flashLight.gameObject.SetActive(true);
            currentObjectRB.isKinematic = false;
            currentObjectCollider.enabled = true;
        }
    }
}
