using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkStation : MonoBehaviour , IInteractable
{

    public float UseRange, CraftingSpeed;
    public bool InUse;
    public GrabbableItem ItemOnStaion;
    public Transform UsedBy, DisplayPoint;
    public CraftableItem CraftingItem;

    public void Activate(Transform _player = null, bool _buttonDown = true)
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
                UsedBy = _player;

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

    private void OnTriggerExit(Collider _col)
    {

        if(_col.TryGetComponent<GrabbableItem>(out GrabbableItem _item) && _item == ItemOnStaion)
            ItemOnStaion = null;

    }

    public virtual void PlaceItem(GrabbableItem _item)
    {

        if(ItemOnStaion)
            return;

        ItemOnStaion = _item;

        if(!ItemOnStaion.TryGetComponent<CraftableItem>(out CraftingItem))
            InvalidItem();
        else if(CraftingItem.Assembled)
            InvalidItem();
        else
        {

            ItemOnStaion.transform.SetParent(null);
            ItemOnStaion.transform.position = DisplayPoint.position;

        }

    }

    public void InvalidItem()
    {

        ItemOnStaion.UngrabItem();
        ItemOnStaion = null;
        UsedBy = null;
        InUse = false;
        
    }

}
