using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairStation : WorkStation
{
    [SerializeField] CraftableItem.TypeOfRepairableItem canRepairThisItem;

    private void Update()
    {

        if(!UsedBy)
            return;

        if(Vector3.Distance(UsedBy.position, transform.position) > UseRange)
        {

            InUse = false;

        }

        if(InUse && CraftingItem.typeOfItem == canRepairThisItem)
        {

            CraftingItem.Progress += CraftingSpeed * Time.deltaTime;
            if(CraftingItem.Progress >= 100)
            {

                RemoveItem();

            }

        }

    }

}
