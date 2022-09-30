using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairStation : WorkStation
{

    [Header("Repairing")]
    [Tooltip("Specifies what items can be repaired at this Station")]
    [SerializeField] CraftableItem.TypeOfRepairableItem canRepairThisItem;
    public FMODUnity.EventReference RepairSoundEvent;

    //Yup, it does the same as workstation
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
            SoundManager.Instance.PlaySound(SoundEvent, gameObject);

        }


        return true;

    }

    //repair :)
    private void Update()
    {

        if(!UsedBy && !AutoRepair || OutOfPower)
        {

            SoundManager.Instance.StopSound(RepairSoundEvent, gameObject);
            return;

        }
        else if(UsedBy && !AutoRepair)
        {

            if(Vector3.Distance(UsedBy.transform.position, transform.position) > UseRange && !AutoRepair)
            {

                InUse = false;
                SoundManager.Instance.StopSound(RepairSoundEvent, gameObject);
                return;

            }

            if(UsedBy.exhausted)
            {
                
                SoundManager.Instance.StopSound(RepairSoundEvent, gameObject);
                return;

            }

            if(InUse && CraftingItem.typeOfItem == canRepairThisItem && !AutoRepair)
            {

                UsedBy.DoingWork(WorkIntensity);
                CraftingItem.Progress += CraftingSpeed * Time.deltaTime;
                if(CraftingItem.Progress >= 100)
                {

                    SoundManager.Instance.PlaySound(CompletedSoundEvent, gameObject);
                    RemoveItem(CraftingItem);

                }
                else
                    SoundManager.Instance.PlaySound(RepairSoundEvent, gameObject, SoundManager.SoundType.Loop);

            }
            else
                SoundManager.Instance.StopSound(RepairSoundEvent, gameObject);

        }
        else if(AutoRepair && CraftingItem)
        {

            if(AutoRepair && CraftingItem.typeOfItem == canRepairThisItem)
            {

                CraftingItem.Progress += CraftingSpeed * Time.deltaTime;
                if(CraftingItem.Progress >= 100)
                {

                    SoundManager.Instance.PlaySound(CompletedSoundEvent, gameObject);
                    RemoveItem(CraftingItem);
 
                }
                else
                    SoundManager.Instance.PlaySound(RepairSoundEvent, gameObject, SoundManager.SoundType.Loop);

            }
            else
                SoundManager.Instance.StopSound(RepairSoundEvent, gameObject);

        }

    }

}
