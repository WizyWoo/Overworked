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

        if(ItemOnStaion.TryGetComponent<CraftableItem>(out craftingItem))
            InUse = _buttonDown;
        else
            InUse = false;
        
        if(!ItemOnStaion)
        {

            ItemOnStaion = _player.GetComponent<PlayerController>().itemGrabbed;

        }

    }

    private void Update()
    {

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
