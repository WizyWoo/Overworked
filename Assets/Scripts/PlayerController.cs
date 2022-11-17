using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using DG.Tweening;
using System.Linq;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UI;
using FMODUnity;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [HideInInspector] public int playerIndex;

    // Movement
    [Header("DESIGNER VARIABLES")]
    [SerializeField] float speed;
    [SerializeField] float burningSpeed;
    [SerializeField] public float weakThrowForce;
    [SerializeField] float strongThrowForce;
    [SerializeField] float timeBurning;
    float horInput, verInput;
    // The direction the character is facing
    public Vector2 Dir { get; private set; }

    // Stamina
    [Header("Stamina")]
    [SerializeField] float maxStamina;
    float currentStamina;
    [SerializeField] float staminaCooldown;
    [SerializeField] float regainStaminaWhenExhausted;
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
    
    [SerializeField] bool goingRight;
    [SerializeField] Image staminaUI;
    [SerializeField] Image staminaUI_back;
    [SerializeField] ParticleSystem sweatParticleSystem, StarParticleSystem;
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

    //Variables for burning movement
    [SerializeField] LayerMask limits;
    [SerializeField] float limitDetectionDistance;
    bool burned, onBridge;
    int lastDir, burnDir;
    float timeTochangeDir, timerChangeDir, timerUnburn;


    //Boolean for reversing movement and float to turn it off
    public bool Dazed, DazedOnce;
    public float DazedTimer;

    public bool electrocuted;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        sr = GetComponentInChildren<SpriteRenderer>();

        // PLAYER INDEX SETUP
        movementAnimator.runtimeAnimatorController = animatorControllers[playerIndex];

        // Starting stamina
        currentStamina = maxStamina;
        currentWorkCapacity = maxWorkCapacity;
        workCapacitySlider.maxValue = maxWorkCapacity;
        workCapacitySlider.value = maxWorkCapacity;

        sweatParticleSystem.Stop();
        StarParticleSystem.Stop();
        burned = false;

        timeTochangeDir = UnityEngine.Random.Range(0.3f, 0.5f);
        timerChangeDir = 0f;
        timerUnburn = 0f;
        lastDir = -1;
    }

    private void Start()
    {

    }

    private void Update()
    {if(DazedOnce == true)
        {
            DazedTimer -= Time.deltaTime;
        }
        if(Dazed == true )
        {
            StarParticleSystem.Play();
        }
        if (DazedTimer < 0)
        {
            Dazed = false;
            DazedOnce = false;
        }
        if(Dazed == false)
        {
            StarParticleSystem.Stop();
        }
        // Update player GFX
        movementAnimator.SetFloat("CurrentVelocity", rb.velocity.magnitude);

        // Update stamina variables and stamina gfx
        StaminaSystem();

        FlipAnimAndRotateArrow();

        // Quick bug fix, need to change in the future
        if (rb.velocity.y > 0)
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
    }

    public void RefillStamina()
    {

        currentStamina = maxStamina;

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
            maxStamina -= Mathf.RoundToInt(maxStamina * 0.25f);
            currentWorkCapacity--;
            workCapacitySlider.value = currentWorkCapacity;

            if (currentWorkCapacity <= 0) FindObjectOfType<LevelManager>().LoseExhausted();

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

    public bool IsNearestItem(GrabbableItem grabbableItem)
    {
        //Putting in the list all the possible items
        //Clean the list
        ItemsInRangeForGrabbing.Clear();
        //Takes all the colliders that are in a radiusRangeOfSphere 
        Collider[] collidersHit = Physics.OverlapSphere(transform.position, radiusRangeOfSphere, layerMask);

        //If there's at least one radius add them to the list of grabbable items 
        if (collidersHit.Length > 0)
        {
            for (int j = 0; j < collidersHit.Length; j++)
            {
                GrabbableItem grabbableItemOverlaped = collidersHit[j].GetComponent<GrabbableItem>();
                if (grabbableItemOverlaped && !grabbableItemOverlaped.GetComponent<CraftableItem>().delivered)
                {
                    ItemsInRangeForGrabbing.Add(grabbableItemOverlaped);
                }
            }
        }
        //Checking if there's some item closer to the player than the given item
        int i = 0;
        bool itemInRange = false;
        //We check the distance from grabbableItem and the rest of them
        //If the distance of another item is bigger just put the boolean itemInRange at true
        //And get out of the while, if not the while continue, it returns itemInRange,
        //If there's no another item closer returns false, else return true
        while (i < ItemsInRangeForGrabbing.Count && !itemInRange)
        {
            //If the item is null
            if (ItemsInRangeForGrabbing[i] == null) continue;
            //Calculating the two distances
            float shortestDistance = Vector3.Distance(grabbableItem.transform.position, transform.position);
            float newDistance = Vector3.Distance(ItemsInRangeForGrabbing[i].transform.position, transform.position);
            
            if (ItemsInRangeForGrabbing[i] != grabbableItem && newDistance < shortestDistance)
                itemInRange = true;
            i++;
        }
        return itemInRange;
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

    [SerializeField] LayerMask layerMask;
    [SerializeField] float radiusRangeOfSphere = 1.0f;

    private void OnDrawGizmos()
    {
        //Draw the range where the items are detected 
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radiusRangeOfSphere);
    }
    public void PressGrab(InputAction.CallbackContext context)
    {
        if (generator != null && inGenerator && !generator.working) generator.SwitchOn();

        if (exhausted || grabDelay) return;

        //The list is cleaned each time we pressGrab
        ItemsInRangeForGrabbing.Clear();
        //Takes all the colliders that are in a radiusRangeOfSphere 
        Collider[] collidersHit = Physics.OverlapSphere(transform.position, radiusRangeOfSphere, layerMask);

        //If there's at least one radius add them to the list of grabbable items 
        if(collidersHit.Length > 0)
        {
            for (int i = 0; i < collidersHit.Length; i++)
            {
                GrabbableItem grabbableItem = collidersHit[i].GetComponent<GrabbableItem>();
                if (grabbableItem && !grabbableItem.GetComponent<CraftableItem>().delivered)
                {
                    ItemsInRangeForGrabbing.Add(grabbableItem);
                } 
                    
            }
        }


        if (context.started)
        {
            // Try grab item
            if (itemGrabbed == null)
            {
                //// If there are any null references destroy them
                /// error needs checking
                foreach (GrabbableItem item in ItemsInRangeForGrabbing)
                    if (item.GetComponent<CraftableItem>().delivered) ItemsInRangeForGrabbing.Remove(item);

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

                SoundManager.Instance.PlaySound(grabItemSound, gameObject);

                itemGrabbed = nearestItem;
                StartCoroutine("GrabItem");

                if(SceneManager.GetActiveScene().name == "Level_01")
                {

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
                if(SceneManager.GetActiveScene().name == "Level_01")
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

                }
                movementAnimator.SetBool("Throwing", true);
                Invoke("setMovementAnimatorFalse", 0.03f);
                DropItem(strongThrowForce);

                SoundManager.Instance.PlaySound(throwItemSound, gameObject);
            }
    }


    void setMovementAnimatorFalse()
    {
        movementAnimator.SetBool("Throwing", false);
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
   public void DropItem(float throwForce)
    {
        Debug.Log("DropItem");
        grabDelay = true;
        Invoke("GrabDelayFinished", .8f);

        // RemoveItem(itemGrabbed);

        itemGrabbed.transform.SetParent(null);
        itemGrabbed.UngrabItem();

        itemGrabbed.GetComponent<Rigidbody>().velocity =
            new Vector3(Dir.x * throwForce, throwForce, Dir.y * throwForce);

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
    {
        if (!burned) Movement();
        else BurntMovement();

        if (transform.position.y < -1 && !falling)
        {
            falling = true;
            burned = false;
            SoundManager.Instance.PlaySound(FallingSound, gameObject);
            if (itemGrabbed) DropItem(weakThrowForce);
        }
        else if (transform.position.y > -1)
        {
            falling = false;
        }
    }

    private void Movement()
    {

        if (electrocuted) { horInput = verInput = 0; return; }

        // Update dir
        if (Mathf.Abs(verInput) >= .5f)
            Dir = new Vector2(0, Mathf.Sign(verInput));
        else if (Mathf.Abs(horInput) >= .5f)
            Dir = new Vector2(Mathf.Sign(horInput), 0);

        Debug.Log("Dir = " + Dir);


        //if (verInput >= .5f)
        //    Dir = new Vector2(0, Mathf.Sign(verInput));

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

    private void BurntMovement()
    {
        timerUnburn += Time.deltaTime;
        if (timerUnburn >= timeBurning)
        {
            timerUnburn = 0;
            Unburn();
            return;
        }

        timerChangeDir += Time.deltaTime;
        if(Physics.Raycast(transform.position, Dir, limitDetectionDistance, limits))
        {
            timerChangeDir = timeTochangeDir;
            if (lastDir > 1) burnDir -= 2;
            else burnDir += 2;
        }
        if(timerChangeDir >= timeTochangeDir)
        {
            while (lastDir != -1 &&lastDir == burnDir)
            {
               if(!onBridge) burnDir = UnityEngine.Random.Range(0, 4);
               else
                {
                    if (UnityEngine.Random.Range(0, 2) == 0) burnDir = 1;
                    else burnDir = 3;
                }
            }
            lastDir = burnDir;
            Vector2 direction = Vector2.zero;
            switch (burnDir)
            {
                case 0:
                    direction = new Vector2(0, -1);
                    break;
                case 1:
                    direction = new Vector2(1, 0);
                    break;
                case 2:
                    direction = new Vector2(0, 1);
                    break;
                case 3:
                    direction = new Vector2(-1, 0);
                    break;
            }

            rb.velocity = new Vector3(direction.x, 0, direction.y) * burningSpeed;
            timeTochangeDir = UnityEngine.Random.Range(0.5f, 0.75f);
            timerChangeDir = 0;
        }
    }

    // INPUT
    public void GetMoveInput(InputAction.CallbackContext context)
    {
        if (electrocuted) return; 

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
        if ((goingRight && Dir.x < -.01f) || (!goingRight && Dir.x > .01f))
        {
            flipAnimator.SetTrigger("Flip");
        }
       
        if (Dir.x > 0.1f)
        {
            arrow.transform.localRotation = Quaternion.Euler(arrow.transform.rotation.x, 0.0f, arrow.transform.rotation.z);
            goingRight = true;
        }
        else if (Dir.x < -0.1f)
        {
            arrow.transform.localRotation = Quaternion.Euler(arrow.transform.rotation.x, 180.0f, arrow.transform.rotation.z);
            goingRight = false;
        }
        else if (Dir.y > 0.1f)
        {
            arrow.transform.localRotation = Quaternion.Euler(arrow.transform.rotation.x, 0.0f, 90.0f);
        }
        else if (Dir.y < -0.1f)
        {
            arrow.transform.localRotation = Quaternion.Euler(arrow.transform.rotation.x, 0.0f, -90.0f);
        }      
    }

    private void Unburn()
    {
        rb.velocity = Vector2.zero;
        burned = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Lava"))
        {
            timerUnburn = 0;
            burned = true;
            if (UnityEngine.Random.Range(0, 2) == 0) burnDir = 1;
            else burnDir = 3;
            lastDir = -1;
            if (currentlyGrabbingAnItem) DropItem(weakThrowForce);
        }

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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Bridge"))
        {
            onBridge = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.CompareTag("Bridge"))
        {
            onBridge = false;
        }
    }
}
