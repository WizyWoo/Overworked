using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using System.Linq;
using UnityEngine.InputSystem.Interactions;

public class PlayerController : MonoBehaviour
{
    public int playerIndex;


    // Movement
    [Header("DESIGNER VARIABLES")]
    [SerializeField] float speed, throwForce = 5;
    float horInput, verInput;

    // References
    Rigidbody rb;
    SpriteRenderer sr;
    [SerializeField] Animator movementAnimator;
    [Header("REFERENCES")]
    [SerializeField] Transform grabSpot;
    [SerializeField] Sprite[] playerSprites;
    [SerializeField] RuntimeAnimatorController[] animatorControllers;
    [SerializeField] Animator flipAnimator;

    [SerializeField] bool goingRight;


    // Grabbing

    // The current item that the player is grabbing,
    // If it is null, the player is not grabbing anything
    public GrabbableItem itemGrabbed;

    public IInteractable InteractingWith;

    // The time it takes the character to grab an item before he can moves
    float grabTime = .5f;

    // Returns true if this character is grabbing an item right now
    bool currentlyGrabbingAnItem;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        sr = GetComponentInChildren<SpriteRenderer>();

        // PLAYER INDEX SETUP
        movementAnimator.runtimeAnimatorController = animatorControllers[playerIndex];
    }

    private void Start()
    {
    }

    private void Update()
    {
        // Update GFX
        movementAnimator.SetFloat("CurrentVelocity", rb.velocity.magnitude);

        //if (rb.velocity.x > .2f)
        //    sr.flipX = false;
        //else if (rb.velocity.x < -.2f)
        //    sr.flipX = true;

        FlipAnim();
    }


    #region GrabbingSystem

    // All the grabbable items near this player are in this list
    public List<GrabbableItem> ItemsInRangeForGrabbing;

    // All items in range
    public void AddItem(GrabbableItem g)
    {
        ItemsInRangeForGrabbing.Add(g);
        //Debug.Log("inRangeItem = " + ItemsInRangeForGrabbing.Count);
    }
    public void RemoveItem(GrabbableItem g)
    {
        ItemsInRangeForGrabbing.Remove(g);
        //Debug.Log("inRangeItem = " + ItemsInRangeForGrabbing.Count);
    }

    public void HoldGrab(InputAction.CallbackContext context)
    {
        //if (itemGrabbed == null)
        //    return;

        //if (context.duration <= 0.4f)
        //    return;


        //if(context.canceled)dd
        //{
        //    float duration_Clamped = Mathf.Clamp((float)context.duration, 0.4f, 1);

        //    itemGrabbed.transform.SetParent(null);
        //    itemGrabbed.UngrabItem();

        //    int flipped = 1;
        //    if (!goingRight)
        //        flipped = -1;

        //    //last player input
        //    Vector2 lastPlayerInput = Vector2.zero;

        //    itemGrabbed.GetComponent<Rigidbody>().AddForce(transform.right * throwForce * duration_Clamped * flipped, ForceMode.Impulse);

        //    itemGrabbed = null;
        //}
    }

    // Called when the grab button is pressed
    // Checks if there are any near objects, if so pick the nearest one
    float time;
    public void PressGrab(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            // Try grab item
            if (itemGrabbed == null)
            {
                if (ItemsInRangeForGrabbing.Count == 0) return;

                GrabbableItem nearestItem = ItemsInRangeForGrabbing[0];
                if (ItemsInRangeForGrabbing.Count == 1)
                    nearestItem = ItemsInRangeForGrabbing[0];

                else
                {
                    for (int i = 0; i < ItemsInRangeForGrabbing.Count; i++)
                    {
                        float shortestDistance = Vector3.Distance(nearestItem.transform.position, transform.position);

                        float newDistance = Vector3.Distance(ItemsInRangeForGrabbing[i].transform.position, transform.position);

                        if (newDistance < shortestDistance)
                            nearestItem = ItemsInRangeForGrabbing[i];
                    }
                }

                itemGrabbed = nearestItem;
                StartCoroutine("GrabItem");
            }

            // Ungrab item
            else if (itemGrabbed != null)
            {
                DropItem(5);

                // take into account this object for grabbing
                //AddItem(itemGrabbed);
            }
        }
    }

    public void PressThrow(InputAction.CallbackContext context)
    {
        if (context.started)
            if (itemGrabbed != null)
                DropItem(10);
    }



    IEnumerator GrabItem()
    {
        // Stop in the place
        rb.velocity = Vector3.zero;

        //itemGrabbed.transform.SetParent(grabSpot);
        //itemGrabbed.GrabItem();
        //itemGrabbed.transform.DOMove(grabSpot.position, grabTime);
        //itemGrabbed.transform.DORotate( Vector3.zero, grabTime);

        // If this item has been grabbed from a conveyor belt, inform the conveyor belt
        ConveyorBelt cb = itemGrabbed.GetComponentInParent<ConveyorBelt>();
        if (cb != null)
        {
            cb.RemoveItemFromConveyor(itemGrabbed);
        }

        itemGrabbed.transform.SetParent(grabSpot);
        itemGrabbed.transform.DOMove(grabSpot.position, grabTime);
        itemGrabbed.GrabItem();

        // Take it out of the inrange grabbable items list
        // Do no take into account this object for grabbing
        RemoveItem(itemGrabbed);

        currentlyGrabbingAnItem = true;
        yield return new WaitForSeconds(grabTime);
        currentlyGrabbingAnItem = false;
    }

    void DropItem(float throwForce)
    {
        itemGrabbed.transform.SetParent(null);
        itemGrabbed.UngrabItem();

        float horDropForce;
        if (goingRight)
            horDropForce = throwForce;
        else horDropForce = -throwForce;
        float verDropForce = 5;

        itemGrabbed.GetComponent<Rigidbody>().velocity =
            new Vector3(horDropForce, verDropForce, 0);

        itemGrabbed = null;
    }

    #endregion


    #region Movement

    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        // If the character is in the process of grabbing an item
        // Dont let the player move
        if (currentlyGrabbingAnItem) return;

        Vector3 newVelocity = Vector3.zero;

        newVelocity += new Vector3(horInput, rb.velocity.y, verInput);

        rb.velocity = newVelocity.normalized * speed;
    }

    // INPUT
    public void GetMoveInput(InputAction.CallbackContext context)
    {
        horInput = context.ReadValue<Vector2>().x;
        verInput = context.ReadValue<Vector2>().y;
    }


    public void Interact(InputAction.CallbackContext context)
    {
        Collider[] _interactables = Physics.OverlapSphere(transform.position, 3, 1 << LayerMask.NameToLayer("Interactable"));

        if (_interactables.Length == 0)
            return;

        IInteractable _closestInteractable = null;
        float _dist = 100;
        for (int i = 0; i < _interactables.Length; i++)
        {

            float _new = Vector3.Distance(_interactables[i].transform.position, transform.position);

            if (_new < _dist)
            {

                _dist = _new;
                _closestInteractable = _interactables[i].GetComponent<IInteractable>();

            }

        }

        if (context.canceled)
        {

            _closestInteractable.Activate(null, false);

        }
        else
        {

            _closestInteractable.Activate(transform, true);

        }

    }

    #endregion

    private void FlipAnim()
    {
        if ((goingRight && rb.velocity.x < -.01f) || (!goingRight && rb.velocity.x > .01f))
        {
            flipAnimator.SetTrigger("Flip");
        }

        if (rb.velocity.x > .1f)
        {
            goingRight = true;
        }
        else if (rb.velocity.x < -0.1f)
        {
            goingRight = false;
        }
    }
}
