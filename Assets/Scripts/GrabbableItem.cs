using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GrabbableItem : MonoBehaviour
{
    public WorkStation OnWorkstation;
    [SerializeField] protected SpriteRenderer outline;
    [SerializeField] LayerMask layerMask;
    [SerializeField] float radiusRangeOfSphere = 1.0f;
    SphereCollider[] colliders;
    bool beingGrabbed = false;
    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (outline != null)
            outline.enabled = false;

        colliders = GetComponents<SphereCollider>();
    }
    private void Update()
    {
        //Get all the colliders that are hitting this item, but just in the player layer
        Collider[] collidersHit = Physics.OverlapSphere(transform.position, radiusRangeOfSphere, layerMask);

        Rigidbody rigidbody = GetComponent<Rigidbody>();
        CraftableItem craftableItem1 = GetComponent<CraftableItem>();
        //If the player is in range from the item, it is not being grabbed, 
        //And it is not flying, that means that Y velocity it's 0, put the outline active 
        //And the item it's not delivered
        if (collidersHit.Length > 0 && !beingGrabbed && GetComponent<CraftableItem>()
            && Mathf.Abs(GetComponent<Rigidbody>().velocity.y) <= 1.5f && !GetComponent<CraftableItem>().delivered)
        {
            if (outline != null)
            {
                bool playerAnotherItemCloser = false;
                int i = 0;
                //Check all the array of players and check if there's an item closer
                while (!playerAnotherItemCloser && i < collidersHit.Length)
                {
                    //Check for that player if there's an item closer than this one
                    playerAnotherItemCloser = collidersHit[i].GetComponent<PlayerController>().IsNearestItem(this);
                    i++;
                }
                CraftableItem craftableItem = GetComponent<CraftableItem>();
                //If it's a craftable item check if it is not delivered

                //If there is another item closer than this one disable the outline,
                //If it is the closer the player enable the outline 
                if (!craftableItem && !playerAnotherItemCloser || (craftableItem &&
                    !craftableItem.delivered && !playerAnotherItemCloser)) outline.enabled = true;
                else if (outline != null)
                    outline.enabled = false;
            }
        }
        //Íf nothing of before happends just disable the outline
        else
        {
            if (outline != null)
                outline.enabled = false;
        }
        //If the item is falling and it's too below destroy it    
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
            {
                if (TutorialManager.GetInstance() != null)
                    TutorialManager.GetInstance().TryToChangePhase(TutorialManager.tutorialPhase.grabArmFromRepairTable);
            }
            else if (GetComponent<CraftableItem>().typeOfItem == CraftableItem.TypeOfRepairableItem.wheel)
                if (TutorialManager.GetInstance() != null)
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
}
