using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class WorkStation : MonoBehaviour , IInteractable
{
    [SerializeField] LayerMask layerMask;
    [SerializeField] float radiusRangeOfSphere = 1.0f;
    protected PlayerController[] playersInScene;
    [SerializeField]
    protected LocalMultiplayer_Manager localMultiplayer;
    public FMODUnity.EventReference overcraftSound;
    private void Start()
    {

        playersInScene = localMultiplayer.allPlayers.ToArray();

    }

    protected virtual void Update()
    {
        Collider[] collidersHit = Physics.OverlapSphere(transform.position, radiusRangeOfSphere, layerMask);

        //If the player is in range from the item, it is not being grabbed, 
        //And it is not flying, that means that Y velocity it's 0, put the outline active 
        if (collidersHit.Length > 0)
        {
            if (outlineScript != null && ItemOnStation)
            {
                CraftableItem craftableItem = ItemOnStation.GetComponent<CraftableItem>();
                //If it's a craftable item check if it is not delivered
                if (craftableItem && !craftableItem.delivered && !craftableItem.Assembled)
                    outlineScript.enabled = true;
                else outlineScript.enabled = false;
            }
            if(InRangePopup)
                InRangePopup.SetActive(true);
        }
        else
        {
            if (outlineScript != null)
                outlineScript.enabled = false;
            
            if(InRangePopup)
                InRangePopup.SetActive(false);
        }

        if(CraftingParticle && OverCraftingParticle)
        {

            if(Crafting && !OverCrafting)
                CraftingParticle.SetActive(true);
            else
                CraftingParticle.SetActive(false);
            
            if(OverCrafting)
                OverCraftingParticle.SetActive(true);
            else
                OverCraftingParticle.SetActive(false);

        }

    }

    [Tooltip("Where the item lands on the table")]
    public Transform DisplayPoint;
    [Tooltip("This will pop up when the player is in range of the station")]
    public GameObject InRangePopup, CraftingParticle, OverCraftingParticle;
    public ParticleSystem FinishedCraftingParticle;
    [Header("Workstation Settings")]
    [Tooltip("How far away the player can interact with the table from")]
    public float UseRange;
    [Tooltip("The speed at which the item is crafted in %, meaning at 50 it will take 2 seconds")]
    public float CraftingSpeed, OverCraftingSpeed;
    [Tooltip("A multiplier for how much stamina you use at this station, 0 = no stamina used")]
    public float WorkIntensity;
    public bool AutoRepair;
    public EventReference SoundEvent, CompletedSoundEvent;
    [HideInInspector]
    public bool InUse, OutOfPower, Crafting, OverCrafting;
    [HideInInspector]
    public GrabbableItem ItemOnStation;
    [HideInInspector]
    public CraftableItem CraftingItem;
    //[HideInInspector]
    public PlayerController UsedBy;

    [SerializeField] protected Outline outlineScript;
    //This shite happens when the player interacts with the station
    public virtual void Activate(Transform _player = null, bool _buttonDown = true)
    {

        if(!_player)
        {

            InUse = false;
            UsedBy = null;
            return;

        }

        if(ItemOnStation)
        {
            
            if(CraftingItem)
            {

                InUse = _buttonDown;
                UsedBy = _player.GetComponent<PlayerController>();

            }
            else
            {

                InUse = false;
                UsedBy = null;
                
            }

        }
        else if(_player.GetComponent<PlayerController>().itemGrabbed)
        {

            PlayerController _pC = _player.GetComponent<PlayerController>();
            PlaceItem(_pC.itemGrabbed);

            if(ItemOnStation == _pC.itemGrabbed)
                _pC.itemGrabbed = null;

        }

    }

    //Places item on station if it hits the trigger :)
    private void OnTriggerEnter(Collider _col)
    {

        if(_col.TryGetComponent<GrabbableItem>(out GrabbableItem _item) && !_col.isTrigger)
        {

            PlaceItem(_item);

        }

    }

    //Checks if the input item is allowed on this station. If not, it will be removed :)))
    public virtual bool PlaceItem(GrabbableItem _item)
    {

        if(ItemOnStation || _item.OnWorkstation)
            return false;

        if(!_item.TryGetComponent<CraftableItem>(out CraftingItem))
            RemoveItem(_item);
        else if(CraftingItem.Assembled)
            RemoveItem(_item);
        else
        {

            ItemOnStation = _item;

            ItemOnStation.UngrabItem();
            ItemOnStation.transform.SetParent(null);
            ItemOnStation.transform.position = DisplayPoint.position;
            ItemOnStation.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            ItemOnStation.OnWorkstation = this;          


        }
        
        return true;

    }

    ///<Summary>
    ///Removes the input item from a station when called
    ///</Summary>

    //Removes the item properly :))))))))))))))
    public virtual void RemoveItem(GrabbableItem _item)
    {

        _item.OnWorkstation = null;

        if(_item = ItemOnStation)
        {

            _item.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            CraftingItem.ItemRemovedFromStation();

        }

        ItemOnStation = null;
        CraftingItem = null;

        UsedBy = null;
        InUse = false;

    }
}
