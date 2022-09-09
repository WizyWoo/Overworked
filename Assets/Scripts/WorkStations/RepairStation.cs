using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairStation : WorkStation
{

    [Header("Repairing")]
    [Tooltip("Specifies what items can be repaired at this Station")]
    [SerializeField] CraftableItem.TypeOfRepairableItem canRepairThisItem;

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

                RemoveItem();

            }

        }

    }

}
