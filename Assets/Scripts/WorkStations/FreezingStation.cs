using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezingStation : WorkStation
{

    [Header("Repairing")]
    [Tooltip("Specifies what items can be repaired at this Station")]
    [SerializeField] CraftableItem.TypeOfRepairableItem itemToFreeze;
    [SerializeField] Sprite itemItCreates;
    public FMODUnity.EventReference FreezeSoundEvent;

    //Yup, it does the same as workstation
    public override bool PlaceItem(GrabbableItem _item)
    {
        if (ItemOnStation || _item.OnWorkstation)
            return false;

        ItemOnStation = _item;

        MoldItem moldItem = _item.GetComponent<MoldItem>();
        if (moldItem != null) moldItem.finalSprite = itemItCreates;

        if (!ItemOnStation.TryGetComponent<CraftableItem>(out CraftingItem)) RemoveItem(ItemOnStation);
        else if (CraftingItem.Assembled) RemoveItem(ItemOnStation);
        else if (CraftingItem.typeOfItem != itemToFreeze) RemoveItem(ItemOnStation);
        else
        {
            ItemOnStation.UngrabItem();
            ItemOnStation.transform.SetParent(null);
            ItemOnStation.transform.position = DisplayPoint.position;
            ItemOnStation.OnWorkstation = this;
            SoundManager.Instance.PlaySound(SoundEvent, gameObject);
        }
        return true;
    }

    //repair :)
    private void Update()
    {
        if (!UsedBy && !AutoRepair || OutOfPower)
        {
            SoundManager.Instance.StopSound(FreezeSoundEvent, gameObject);
            return;
        }
        else if (UsedBy && !AutoRepair)
        {
            if (Vector3.Distance(UsedBy.transform.position, transform.position) > UseRange && !AutoRepair)
            {
                InUse = false;
                SoundManager.Instance.StopSound(FreezeSoundEvent, gameObject);
                return;
            }
            if (UsedBy.exhausted)
            {
                SoundManager.Instance.StopSound(FreezeSoundEvent, gameObject);
                return;
            }
            if (InUse && CraftingItem.typeOfItem == itemToFreeze && !AutoRepair)
            {
                UsedBy.DoingWork(WorkIntensity);
                CraftingItem.Progress += CraftingSpeed * Time.deltaTime;
                if (CraftingItem.Progress >= 100)
                {
                    SoundManager.Instance.PlaySound(CompletedSoundEvent, gameObject);
                    RemoveItem(CraftingItem);
                }
                else SoundManager.Instance.PlaySound(FreezeSoundEvent, gameObject, SoundManager.SoundType.Loop);
            }
            else SoundManager.Instance.StopSound(FreezeSoundEvent, gameObject);
        }
        else if (AutoRepair && CraftingItem)
        {
            if (AutoRepair && CraftingItem.typeOfItem == itemToFreeze)
            {
                CraftingItem.Progress += CraftingSpeed * Time.deltaTime;
                if (CraftingItem.Progress >= 100)
                {
                    SoundManager.Instance.PlaySound(CompletedSoundEvent, gameObject);
                    RemoveItem(CraftingItem);
                }
                else SoundManager.Instance.PlaySound(FreezeSoundEvent, gameObject, SoundManager.SoundType.Loop);
            }
            else SoundManager.Instance.StopSound(FreezeSoundEvent, gameObject);
        }
    }
}
