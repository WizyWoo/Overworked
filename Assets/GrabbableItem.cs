using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableItem : MonoBehaviour
{
    bool beingGrabbed = false;

    Rigidbody rb;

    SphereCollider[] colliders;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        colliders = GetComponents<SphereCollider>();
    }

    // Update parameters when the item is grabbed/ungrabbed
    public void GrabItem()
    {
        // Disable all colliders
        foreach (SphereCollider col in colliders)
            col.enabled = false;

        beingGrabbed = true;
        rb.isKinematic = true;
    }
    public void UngrabItem()
    {
        // Enable all colliders
        foreach (SphereCollider col in colliders)
            col.enabled = true;

        beingGrabbed = false;
        rb.isKinematic = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if (player != null)
        {
            //if (assembled) return;

            player.AddItem(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if (player != null)
        {
            player.RemoveItem(this);
        }
    }
}
