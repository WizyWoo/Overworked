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

        if(ItemOnStation || _item.OnWorkstation)
            return false;

        if(!_item.TryGetComponent<CraftableItem>(out CraftingItem))
            RemoveItem(_item);
        else if(CraftingItem.Assembled)
            RemoveItem(_item);
        else if(CraftingItem.typeOfItem != canRepairThisItem)
            RemoveItem(_item);
        else
        {

            ItemOnStation = _item;

            ItemOnStation.UngrabItem();
            ItemOnStation.transform.SetParent(null);
            ItemOnStation.transform.position = DisplayPoint.position;
            ItemOnStation.OnWorkstation = this;
            ItemOnStation.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            SoundManager.Instance.PlaySound(SoundEvent, gameObject);

        }

        //TutorialManager.

        return true;

    }

    //repair :)
    protected override void Update()
    {
        base.Update();

        foreach (PlayerController _pC in localMultiplayer.allPlayers)
        {

            if(_pC.itemGrabbed && _pC.itemGrabbed.TryGetComponent<CraftableItem>(out CraftableItem _cI))
            {

                if(_cI.typeOfItem == canRepairThisItem && !_cI.Assembled)
                    outlineScript.enabled = true;

            }
            
        }

        if(CraftingItem && CraftingItem.typeOfItem == canRepairThisItem)
        {

            SoundManager.Instance.PlaySound(RepairSoundEvent, gameObject, SoundManager.SoundType.Loop);

            if(CraftingItem.Progress < 100)
                CraftingItem.Progress += CraftingSpeed * Time.deltaTime;
            else
                CraftingItem.Progress += OverCraftingSpeed * Time.deltaTime;

            if(CraftingItem.Progress >= 100 && !CraftingItem.Assembled)
            {

                CraftingItem.Assembled = true;
                SoundManager.Instance.PlaySound(CompletedSoundEvent, gameObject);

            }

        }
        else
        {

            SoundManager.Instance.StopSound(RepairSoundEvent, gameObject);

        }

        /*if(!UsedBy && !AutoRepair || OutOfPower)
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

                if(CraftingItem.Progress < 100)
                    CraftingItem.Progress += CraftingSpeed * Time.deltaTime;
                else
                    CraftingItem.Progress += OverCraftingSpeed * Time.deltaTime;

                
                if(CraftingItem.Progress >= 100 && !CraftingItem.Assembled)
                {

                    CraftingItem.Assembled = true;
                    SoundManager.Instance.PlaySound(CompletedSoundEvent, gameObject);
 
                }
                else
                    SoundManager.Instance.PlaySound(RepairSoundEvent, gameObject, SoundManager.SoundType.Loop);

            }
            else
                SoundManager.Instance.StopSound(RepairSoundEvent, gameObject);

        }*/

    }

}
