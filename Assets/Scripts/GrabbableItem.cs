using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GrabbableItem : MonoBehaviour
{

    public WorkStation OnWorkstation;
    bool beingGrabbed = false;

    Rigidbody rb;

    [SerializeField] SpriteRenderer outline;

    SphereCollider[] colliders;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        
        if (outline != null) 
            outline.enabled = false;

        colliders = GetComponents<SphereCollider>();
    }
    [SerializeField] LayerMask layerMask;
    [SerializeField] float radiusRangeOfSphere = 1.0f;
    private void Update()
    {
        Collider[] collidersHit = Physics.OverlapSphere(transform.position, radiusRangeOfSphere, layerMask);

        //If the player is in range from the item, it is not being grabbed, 
        //And it is not flying, that means that Y velocity it's 0, put the outline active 
        if (collidersHit.Length > 0 && !beingGrabbed && GetComponent<CraftableItem>()
            && GetComponent<Rigidbody>().velocity.y == 0 && !GetComponent<CraftableItem>().delivered)
        {
            CraftableItem craftableItem = GetComponent<CraftableItem>();
            if (outline != null)
            {
                //If it's a craftable item check if it is not delivered
                if (!craftableItem || (craftableItem && !craftableItem.delivered))
                   outline.enabled = true;
            }
        }    
        else
        {
            if (outline != null)
                outline.enabled = false;
        }
           

        if (transform.position.y < -50)
            Destroy(this.gameObject);
    }


    // This is called when the player grab this item
    // Update parameters when the item is grabbed/ungrabbed
    public void GrabItem()
    {
        //Disable all colliders
        foreach (SphereCollider col in colliders)
            col.enabled = false;

        beingGrabbed = true;

        if (OnWorkstation)
            OnWorkstation.RemoveItem(this);

        rb.isKinematic = true;

        CraftableItem craftableItem = GetComponent<CraftableItem>();
        if (craftableItem != null && craftableItem.Assembled)
        {
            Debug.Log("REMOVE ITEM");

            if (GetComponent<CraftableItem>().typeOfItem == CraftableItem.TypeOfRepairableItem.arm)
                TutorialManager.GetInstance().TryToChangePhase(TutorialManager.tutorialPhase.grabArmFromRepairTable);
            else if (GetComponent<CraftableItem>().typeOfItem == CraftableItem.TypeOfRepairableItem.wheel)
                TutorialManager.GetInstance().TryToChangePhase(TutorialManager.tutorialPhase.grabWheelFromRepairTable);
        }
    }


    public void UngrabItem()
    {
        // Enable all colliders
        foreach (SphereCollider col in colliders)
            col.enabled = true;

        beingGrabbed = false;
        rb.isKinematic = false;
        rb.velocity = Vector3.zero;

        // If the player is rotating while throwing this item, put the rotation normally
        transform.DORotate(Vector3.zero, .2f);
    }


    private void OnTriggerEnter(Collider other)
    {
        //PlayerController player = other.GetComponent<PlayerController>();

        //if (player != null)
        //{
            //if (!GetComponent<CraftableItem>().delivered)
            //    player.AddItem(this);
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        //PlayerController player = other.GetComponent<PlayerController>();

        //if (player != null)
        //{
        //    player.RemoveItem(this);
        //}
    }

    private void OnDestroy()
    {

    }
}
