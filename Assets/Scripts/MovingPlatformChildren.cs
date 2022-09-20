using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformChildren : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        PlayerController playerController = collision.transform.GetComponent<PlayerController>();
        GrabbableItem grabbableItem = collision.transform.GetComponent<GrabbableItem>();

        if (playerController == null && grabbableItem == null)
            return;

        if (grabbableItem == null)
            playerController.transform.parent = transform;
        else
            grabbableItem.transform.parent = transform;
    }

    private void OnCollisionExit(Collision collision)
    {
        PlayerController playerController = collision.transform.GetComponent<PlayerController>();
        CraftableItem craftableItem = collision.transform.GetComponent<CraftableItem>();

        if (playerController == null && craftableItem == null)
            return;

        if (craftableItem == null)
            playerController.transform.parent = null;
        else
            craftableItem.transform.parent = null;
    }
}
