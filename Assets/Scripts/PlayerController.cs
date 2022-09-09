using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using System.Linq;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public int playerIndex;

    // Movement
    [Header("DESIGNER VARIABLES")]
    [SerializeField] float speed;
    [SerializeField] float weakThrowForce;
    [SerializeField] float strongThrowForce;
    float horInput, verInput;

    // Stamina

    [Header("Stamina")]
    [SerializeField] float maxStamina;
    float currentStamina;
    [SerializeField] float staminaCooldown;
    bool exhausted;
    [SerializeField] float regainStaminaSpeed;

    // References
    Rigidbody rb;
    SpriteRenderer sr;
    [Header("REFERENCES")]
    [SerializeField] Animator movementAnimator;
    [SerializeField] Transform grabSpot;
    [SerializeField] Sprite[] playerSprites;
    [SerializeField] RuntimeAnimatorController[] animatorControllers;
    [SerializeField] Animator flipAnimator;
    [SerializeField] bool goingRight;
    [SerializeField] Image staminaUI;


    // Grabbing

    // The current item that the player is grabbing,
    // If it is null, the player is not grabbing anything
    [HideInInspector] public GrabbableItem itemGrabbed;

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
        movementAnimator.runtimeAnimatorController = animatorControllers[playerIndex % 2];

        // Starting stamina
        currentStamina = maxStamina;
    }

    private void Start()
    {
    }

    private void Update()
    {
        // Update player GFX
        movementAnimator.SetFloat("CurrentVelocity", rb.velocity.magnitude);

        // Update stamina variables and stamina gfx
        StaminaSystem();


        FlipAnim();



        // Quick bug fix, need to change in the future
        if (rb.velocity.y > 0)
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
    }

    public void DoingWork(float _intensity)
    {

        currentStamina -= (_intensity * Time.deltaTime) + (Time.deltaTime * regainStaminaSpeed);

    }

    void StaminaSystem()
    {
        if (exhausted)
        {
            currentStamina += Time.deltaTime;

            if (currentStamina >= maxStamina)
                exhausted = false;
        }
        else
        {
            // Lose stamina if moving
            if (horInput != 0 || verInput != 0)
                currentStamina -= Time.deltaTime;
            else
                currentStamina += Time.deltaTime * regainStaminaSpeed;
        }

        // Clamp min and max values
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

        // Update stamina UI
        float newAmount = (currentStamina / maxStamina) / 2;
        staminaUI.fillAmount = newAmount;
        if (currentStamina < maxStamina / 2)
        {
            float f = (currentStamina / (maxStamina / 2));
            staminaUI.color = Color.Lerp(Color.red, Color.yellow, f);
        }
        else
        {
            float f = (currentStamina - maxStamina / 2) / (maxStamina / 2);
            staminaUI.color = Color.Lerp(Color.yellow, Color.green, f);
        }

        // Exhausted ?
        if (currentStamina == 0)
        {
            exhausted = true;
            // Drop the
            if (itemGrabbed != null)
                DropItem(weakThrowForce);
        }
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

    // Called when the grab button is pressed
    // Checks if there are any near objects, if so pick the nearest one
    float time;
    public void PressGrab(InputAction.CallbackContext context)
    {
        if (exhausted) return;

        if (context.started)
        {
            // Try grab item
            if (itemGrabbed == null)
            {
                // If there are any null references destroy them
                foreach (GrabbableItem item in ItemsInRangeForGrabbing)
                    if (item == null) ItemsInRangeForGrabbing.Remove(item);

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
                DropItem(weakThrowForce);

                // take into account this object for grabbing
                //AddItem(itemGrabbed);
            }
        }
    }

    public void PressThrow(InputAction.CallbackContext context)
    {
        if (exhausted) return;

        if (context.started)
            if (itemGrabbed != null)
                DropItem(strongThrowForce);
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
        //RemoveItem(itemGrabbed);

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


        if (exhausted)
            rb.velocity *= 0;
        else
            rb.velocity *= Mathf.Lerp(.5f, 1, currentStamina / maxStamina);
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
