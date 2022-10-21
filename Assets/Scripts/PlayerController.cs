using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using System.Linq;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UI;
using FMODUnity;

public class PlayerController : MonoBehaviour
{
    [HideInInspector] public int playerIndex;

    // Movement
    [Header("DESIGNER VARIABLES")]
    [SerializeField] float speed;
    [SerializeField] float weakThrowForce;
    [SerializeField] float strongThrowForce;
    float horInput, verInput;
    // The direction the character is facing
    Vector2 dir;

    // Stamina
    [Header("Stamina")]
    [SerializeField] float maxStamina;
    float currentStamina;
    [SerializeField] float staminaCooldown;
    [SerializeField] float regainStaminaWhenExhausted;
    [SerializeField] float staminaReducedWhenExhausted;
    [HideInInspector] public bool exhausted;
    [SerializeField] float regainStaminaSpeed;
    [SerializeField] Color zeroStamina;
    [SerializeField] Color midStamina;
    [SerializeField] Color fullStamina;
    [SerializeField] float maxWorkCapacity;
    [SerializeField] Slider workCapacitySlider;
    float currentWorkCapacity;

    // References
    Rigidbody rb;
    SpriteRenderer sr;
    [Header("REFERENCES")]
    [SerializeField] SpriteRenderer gfx;
    [SerializeField] Animator movementAnimator;
    [SerializeField] Transform grabSpot;
    [SerializeField] Sprite[] playerSprites;
    [SerializeField] RuntimeAnimatorController[] animatorControllers;
    [SerializeField] Animator flipAnimator;
    [SerializeField] bool Dazed;
    [SerializeField] bool goingRight;
    [SerializeField] Image staminaUI;
    [SerializeField] Image staminaUI_back;
    [SerializeField] ParticleSystem sweatParticleSystem;
    [SerializeField] GameObject arrow;
    
    [SerializeField] EventReference FallingSound;
    // Grabbing
    // The current item that the player is grabbing,
    // If it is null, the player is not grabbing anything
    [HideInInspector] public GrabbableItem itemGrabbed;
    public IInteractable InteractingWith;
    // The time it takes the character to grab an item before he can moves
    float grabTime = .5f;
    // Returns true if this character is grabbing an item right now
    bool currentlyGrabbingAnItem, falling;
    IInteractable workingOnStation;
    bool inGenerator;
    Generator generator;
    public FMODUnity.EventReference exhaustedSound, playerHitted, grabItemSound, throwItemSound, dropItemSound;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        sr = GetComponentInChildren<SpriteRenderer>();

        // PLAYER INDEX SETUP
        movementAnimator.runtimeAnimatorController = animatorControllers[playerIndex % 2];

        // Starting stamina
        currentStamina = maxStamina;
        currentWorkCapacity = maxWorkCapacity;
        workCapacitySlider.maxValue = maxWorkCapacity;
        workCapacitySlider.value = maxWorkCapacity;

        sweatParticleSystem.Stop();
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

        FlipAnimAndRotateArrow();

        // Quick bug fix, need to change in the future
        if (rb.velocity.y > 0)
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
    }

    public void DoingWork(float _intensity)
    {
        currentStamina -= (_intensity * Time.deltaTime) + (Time.deltaTime * regainStaminaSpeed);
    }


    // Intermittent colors for when the player runs out of stamina
    float intermitentTime = .5f;
    void gfxRed()
    {
        gfx.DOColor(Color.red, intermitentTime);
        Invoke("gfxNormal", intermitentTime);
    }

    void gfxNormal()
    {
        gfx.DOColor(Color.white, intermitentTime);
        Invoke("gfxRed", intermitentTime);
    }

    void StaminaSystem()
    {
        if (exhausted)
        {
            currentStamina += Time.deltaTime * regainStaminaWhenExhausted;

            if (currentStamina >= maxStamina)
            {
                exhausted = false;
                movementAnimator.SetBool("IsExhausted", false);
                sweatParticleSystem.Stop();

                gfxNormal();
                CancelInvoke();
            }
        }
        else
        {
            // Lose stamina if moving
            if (horInput != 0 || verInput != 0)
                //if (!TutorialManager.GetInstance().doTutorial)
                    currentStamina -= Time.deltaTime;
                else
                    currentStamina += Time.deltaTime * regainStaminaSpeed;
        }

        // Clamp min and max values
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

        // Update stamina UI
        if (currentStamina >= maxStamina)
        {
            staminaUI.enabled = false;
            staminaUI_back.enabled = false;

            return;
        }
        else
        {
            staminaUI.enabled = true;
            staminaUI_back.enabled = true;
        }

        float newAmount = (currentStamina / maxStamina) / 2;
        staminaUI.fillAmount = newAmount;
        if (currentStamina < maxStamina / 2)
        {
            float f = (currentStamina / (maxStamina / 2));
            staminaUI.color = Color.Lerp(zeroStamina, midStamina, f);
        }
        else
        {
            float f = (currentStamina - maxStamina / 2) / (maxStamina / 2);
            staminaUI.color = Color.Lerp(midStamina, fullStamina, f);
        }

        // Exhausted ?
        if (currentStamina <= 0)
        {
            maxStamina -= staminaReducedWhenExhausted;
            currentWorkCapacity--;
            workCapacitySlider.value = currentWorkCapacity;

            if (currentWorkCapacity <= 0) FindObjectOfType<LevelManager>().Lose();

            movementAnimator.SetBool("IsExhausted", true);
            SoundManager.Instance.PlaySound(exhaustedSound, gameObject);
     
            exhausted = true;
            gfxRed();

            // Drop the
            if (itemGrabbed != null)
                DropItem(weakThrowForce);

            sweatParticleSystem.Play();
        }
    }

    public void Relaxing(float relaxSpeed)
    {
        currentStamina += Time.deltaTime * relaxSpeed;
    }

    public void HitOnStamina(float amount)
    {
        SoundManager.Instance.PlaySound(playerHitted, gameObject);
        currentStamina -= amount;
    }

    public void Electrocuted(float amount)
    {
        currentStamina -= amount;
    }

    #region GrabbingSystem

    // All the grabbable items near this player are in this list
    public List<GrabbableItem> ItemsInRangeForGrabbing;

    // If it is true, the player cannot grab
    // Turns true when player throw something, turns false after a second, so
    // the player cannot grab the same item before it falls into the floor
    bool grabDelay;

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
        if (generator != null && inGenerator && !generator.working) generator.SwitchOn();

        if (exhausted || grabDelay) return;

        if (context.started)
        {
            // Try grab item
            if (itemGrabbed == null)
            {
                SoundManager.Instance.PlaySound(grabItemSound, gameObject);

                //// If there are any null references destroy them
                /// error needs checking
                //foreach (GrabbableItem item in ItemsInRangeForGrabbing)
                //    if (item.GetComponent<CraftableItem>().delivered) ItemsInRangeForGrabbing.Remove(item);

                if (ItemsInRangeForGrabbing.Count == 0) return;

                GrabbableItem nearestItem = ItemsInRangeForGrabbing[0];
                if (ItemsInRangeForGrabbing.Count == 1)
                    nearestItem = ItemsInRangeForGrabbing[0];

                else
                {
                    for (int i = 0; i < ItemsInRangeForGrabbing.Count; i++)
                    {
                        if (ItemsInRangeForGrabbing[i] == null) continue;

                        float shortestDistance = Vector3.Distance(nearestItem.transform.position, transform.position);

                        float newDistance = Vector3.Distance(ItemsInRangeForGrabbing[i].transform.position, transform.position);

                        if (newDistance < shortestDistance)
                            nearestItem = ItemsInRangeForGrabbing[i];
                    }
                }

                itemGrabbed = nearestItem;
                StartCoroutine("GrabItem");

                if (itemGrabbed.GetComponent<CraftableItem>().typeOfItem == CraftableItem.TypeOfRepairableItem.arm)
                {
                    // Inform the tutorial manager
                    TutorialManager.GetInstance().TryToChangePhase(TutorialManager.tutorialPhase.grabArmFromConveyor);
                    //if (playerIndex % 2 == 0)
                    TutorialManager.GetInstance().TryToChangePhase(TutorialManager.tutorialPhase.grabArmFromFloor_p1);
                    //else
                    TutorialManager.GetInstance().TryToChangePhase(TutorialManager.tutorialPhase.grabArmFromFloor_p2);
                }

                else if (itemGrabbed.GetComponent<CraftableItem>().typeOfItem == CraftableItem.TypeOfRepairableItem.wheel)
                {
                    // Inform the tutorial manager
                    TutorialManager.GetInstance().TryToChangePhase(TutorialManager.tutorialPhase.grabWheelFromConveyor);
                    //if (playerIndex % 2 == 0)
                    TutorialManager.GetInstance().TryToChangePhase(TutorialManager.tutorialPhase.grabWheelFromFloor_p1);
                    //else
                    TutorialManager.GetInstance().TryToChangePhase(TutorialManager.tutorialPhase.grabWheelFromFloor_p2);
                }

            }

            // Ungrab item
            else if (itemGrabbed != null)
            {
                DropItem(weakThrowForce);
                SoundManager.Instance.PlaySound(dropItemSound, gameObject);

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
            {
                if (itemGrabbed.GetComponent<CraftableItem>().typeOfItem == CraftableItem.TypeOfRepairableItem.arm)
                {
                    //if (playerIndex % 2 == 0)
                    TutorialManager.GetInstance().TryToChangePhase(TutorialManager.tutorialPhase.throwArm_p1);
                    //else
                    TutorialManager.GetInstance().TryToChangePhase(TutorialManager.tutorialPhase.throwArm_p2);
                }
                else if (itemGrabbed.GetComponent<CraftableItem>().typeOfItem == CraftableItem.TypeOfRepairableItem.wheel)
                {
                    //if (playerIndex % 2 == 0)
                    TutorialManager.GetInstance().TryToChangePhase(TutorialManager.tutorialPhase.throwWheel_p1);
                    //else
                    TutorialManager.GetInstance().TryToChangePhase(TutorialManager.tutorialPhase.throwWheel_p2);
                }           
                DropItem(strongThrowForce);
                SoundManager.Instance.PlaySound(throwItemSound, gameObject);
            }
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
        Debug.Log("DropItem");
        grabDelay = true;
        Invoke("GrabDelayFinished", .8f);

        // RemoveItem(itemGrabbed);

        //itemGrabbed.transform.SetParent(null);
        itemGrabbed.UngrabItem();

        itemGrabbed.GetComponent<Rigidbody>().velocity =
            new Vector3(dir.x * throwForce, throwForce, dir.y * throwForce);

        itemGrabbed = null;
    }

    void GrabDelayFinished()
    {
        grabDelay = false;
    }

    public IEnumerator TryRemoveGrabbableItemFromList(GrabbableItem g)
    {
        yield return new WaitForSeconds(0f);
        RemoveItem(g);
    }

    #endregion


    #region Movement

    private void FixedUpdate()
    { Movement(); if(transform.position.y < -1 && !falling)
                {
                    falling = true;
                    SoundManager.Instance.PlaySound(FallingSound, gameObject);
                }else if(transform.position.y > -1)
                {
                    falling = false;
                }}

    private void Movement()
    {
        // Update dir
        if (horInput != 0)
            dir = new Vector2(Mathf.Sign(horInput), 0);
        else if (verInput != 0)
            dir = new Vector2(0, Mathf.Sign(verInput));

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
        if (Dazed == true)
        {
            horInput = -context.ReadValue<Vector2>().x;
            verInput = -context.ReadValue<Vector2>().y;
            return;
        }
        horInput = context.ReadValue<Vector2>().x;
        verInput = context.ReadValue<Vector2>().y;
    }


    public void Interact(InputAction.CallbackContext context)
    {

        if(!this.enabled)
            return;

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
            workingOnStation = null;
        }
        else
        {
            _closestInteractable.Activate(transform, true);
            workingOnStation = _closestInteractable;
        }

    }

    private void OnDisable()
    {
        if(workingOnStation != null)
            workingOnStation.Activate(null, false);
    }

    #endregion
    private void FlipAnimAndRotateArrow()
    {
        if ((goingRight && rb.velocity.x < -.01f) || (!goingRight && rb.velocity.x > .01f))
        {
            flipAnimator.SetTrigger("Flip");
        }
       
        if (rb.velocity.x > 0.1f)
        {
            arrow.transform.localRotation = Quaternion.Euler(arrow.transform.rotation.x, 0.0f, arrow.transform.rotation.z);
            goingRight = true;
        }
        else if (rb.velocity.x < -0.1f)
        {
            arrow.transform.localRotation = Quaternion.Euler(arrow.transform.rotation.x, 180.0f, arrow.transform.rotation.z);
            goingRight = false;
        }
        else if (rb.velocity.z > 0.1f)
        {
            arrow.transform.localRotation = Quaternion.Euler(arrow.transform.rotation.x, 0.0f, 90.0f);
        }
        else if (rb.velocity.z < -0.1f)
        {
            arrow.transform.localRotation = Quaternion.Euler(arrow.transform.rotation.x, 0.0f, -90.0f);
        }
       
    }

    private void OnTriggerEnter(Collider other)
    {
        generator = other.GetComponent<Generator>();

        if (generator == null) return;

        inGenerator = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (generator == null) return;

        inGenerator = false;
        generator = null;
    }
}
