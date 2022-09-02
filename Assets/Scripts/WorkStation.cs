using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkStation : MonoBehaviour , IInteractable
{

    public float UseRange, CraftingSpeed;
    public bool InUse;
    public GrabbableItem ItemOnStaion;
    public Transform OccupiedBy;
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

            if(ItemOnStaion.TryGetComponent<CraftableItem>(out craftingItem))
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

            ItemOnStaion = _player.GetComponent<PlayerController>().itemGrabbed;

        }

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

        }

    }

}
