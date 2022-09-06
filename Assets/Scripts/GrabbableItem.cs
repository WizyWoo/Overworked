using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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

        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
    public void UngrabItem()
    {
        // Enable all colliders
        foreach (SphereCollider col in colliders)
            col.enabled = true;

        beingGrabbed = false;
        rb.isKinematic = false;

        // If the player is rotating while throwing this item, put the rotation normally
        transform.DORotate(Vector3.zero, .2f);
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
