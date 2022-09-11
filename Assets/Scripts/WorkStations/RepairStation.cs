using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairStation : WorkStation
{

    [Header("Repairing")]
    [Tooltip("Specifies what items can be repaired at this Station")]
    [SerializeField] CraftableItem.TypeOfRepairableItem canRepairThisItem;

    public override bool PlaceItem(GrabbableItem _item)
    {

        if(ItemOnStaion || _item.OnWorkstation)
            return false;

        ItemOnStaion = _item;

        if(!ItemOnStaion.TryGetComponent<CraftableItem>(out CraftingItem))
            RemoveItem(ItemOnStaion);
        else if(CraftingItem.Assembled)
            RemoveItem(ItemOnStaion);
        else if(CraftingItem.typeOfItem != canRepairThisItem)
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

    private void Update()
    {

        if(!UsedBy)
            return;

        if(Vector3.Distance(UsedBy.transform.position, transform.position) > UseRange)
        {

            InUse = false;
            return;

        }

        if(InUse && CraftingItem.typeOfItem == canRepairThisItem)
        {

            UsedBy.DoingWork(WorkIntensity);
            CraftingItem.Progress += CraftingSpeed * Time.deltaTime;
            if(CraftingItem.Progress >= 100)
            {

                RemoveItem(CraftingItem);

            }

        }

    }

}
