using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class WorkStation : MonoBehaviour , IInteractable
{

    [Tooltip("Where the item lands on the table")]
    public Transform DisplayPoint;
    [Header("Workstation Settings")]
    [Tooltip("How far away the player can interact with the table from")]
    public float UseRange;
    [Tooltip("The speed at which you do work at this station")]
    public float CraftingSpeed;
    [Tooltip("A multiplier for how much stamina you use at this station, 0 = no stamina used")]
    public float WorkIntensity;
    public bool AutoRepair;
    public EventReference SoundEvent, CompletedSoundEvent;
    [HideInInspector]
    public bool InUse, OutOfPower;
    [HideInInspector]
    public GrabbableItem ItemOnStaion;
    [HideInInspector]
    public CraftableItem CraftingItem;
    [HideInInspector]
    public PlayerController UsedBy;

    //This shite happens when the player interacts with the station
    public virtual void Activate(Transform _player = null, bool _buttonDown = true)
    {

        if(!_player)
        {

            InUse = false;
            UsedBy = null;
            return;

        }

        if(ItemOnStaion)
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

            if(ItemOnStaion == _pC.itemGrabbed)
                _pC.itemGrabbed = null;
            
        }

    }

    //Places item on station if it hits the trigger :)
    private void OnTriggerEnter(Collider _col)
    {

        if(_col.TryGetComponent<GrabbableItem>(out GrabbableItem _item))
        {

            PlaceItem(_item);

        }

    }

    //Checks if the input item is allowed on this station. If not, it will be removed :)))
    public virtual bool PlaceItem(GrabbableItem _item)
    {

        if(ItemOnStaion || _item.OnWorkstation)
            return false;

        ItemOnStaion = _item;

        if(!ItemOnStaion.TryGetComponent<CraftableItem>(out CraftingItem))
            RemoveItem(ItemOnStaion);
        else if(CraftingItem.Assembled)
            RemoveItem(ItemOnStaion);
        else
        {

            ItemOnStaion.UngrabItem();
            ItemOnStaion.transform.SetParent(null);
            ItemOnStaion.transform.position = DisplayPoint.position;
            ItemOnStaion.OnWorkstation = this;

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
        if(_item = ItemOnStaion)
        {

            ItemOnStaion = null;
            CraftingItem = null;
        
        }
        UsedBy = null;
        InUse = false;
        
    }

}
