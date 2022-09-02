using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkStation : MonoBehaviour , IInteractable
{

    public float UseRange, CraftingSpeed;
    public bool InUse;
    public GrabbableItem ItemOnStaion;
    public Transform OccupiedBy, DisplayPoint;
    private CraftableItem craftingItem;

    public void Activate(Transform _player = null, bool _buttonDown = true)
    {

        if(!_player)
        {

            InUse = false;
            OccupiedBy = null;
            return;

        }

        if(ItemOnStaion)
        {

            if(craftingItem)
            {

                InUse = _buttonDown;
                OccupiedBy = _player;

            }
            else
            {
                
                InUse = false;
                OccupiedBy = null;

            }

        }
        else if(_player.GetComponent<PlayerController>().itemGrabbed)
        {

            PlayerController _pC = _player.GetComponent<PlayerController>();
            ItemOnStaion = _pC.itemGrabbed;
            _pC.itemGrabbed = null;
            ItemOnStaion.transform.SetParent(null);
            ItemOnStaion.transform.position = DisplayPoint.position;

            if(!ItemOnStaion.TryGetComponent<CraftableItem>(out craftingItem))
                InvalidItem();
            else if(craftingItem.Assembled)
                InvalidItem();

        }

    }

    public void InvalidItem()
    {

        craftingItem.UngrabItem();
        ItemOnStaion = null;
        OccupiedBy = null;
        InUse = false;
        
    }

    private void Update()
    {

        if(!OccupiedBy)
            return;

        if(Vector3.Distance(OccupiedBy.position, transform.position) > UseRange)
        {

            InUse = false;

        }

        if(InUse)
        {

            craftingItem.Progress += CraftingSpeed * Time.deltaTime;
            if(craftingItem.Progress >= 100)
            {

                InvalidItem();

            }

        }

    }

}
