using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkStation : MonoBehaviour , IInteractable
{

    [Header("Workstation Settings")]
    [Tooltip("How far away the player can interact with the table from")]
    public float UseRange;
    [Tooltip("The speed at which you do work at this station")]
    public float CraftingSpeed;
    [Tooltip("A multiplier for how much stamina you use at this station, 0 = no stamina used")]
    public float WorkIntensity;
    [HideInInspector]
    public bool InUse;
    [Tooltip("Where the item lands on the table")]
    public Transform DisplayPoint;
    [HideInInspector]
    public GrabbableItem ItemOnStaion;
    [HideInInspector]
    public CraftableItem CraftingItem;
    [HideInInspector]
    public PlayerController UsedBy;

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

    private void OnTriggerEnter(Collider _col)
    {

        if(_col.TryGetComponent<GrabbableItem>(out GrabbableItem _item))
        {

            PlaceItem(_item);

        }

    }

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
