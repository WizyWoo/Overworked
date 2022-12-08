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

        if(CraftingItem && CraftingItem.typeOfItem == canRepairThisItem && !CraftingItem.OnCooldown)
        {

            SoundManager.Instance.PlaySound(RepairSoundEvent, gameObject, SoundManager.SoundType.Loop);

            if(CraftingItem.Progress < 100)
            {

                OverCrafting = false;
                CraftingItem.Progress += CraftingSpeed * Time.deltaTime;

            }
            else
            {

                OverCrafting = true;
                CraftingItem.Progress += OverCraftingSpeed * Time.deltaTime;

            }

            if(CraftingItem.Progress >= 100 && !CraftingItem.Assembled)
            {

                CraftingItem.Assembled = true;
                FinishedCraftingParticle.Play();
                SoundManager.Instance.PlaySound(CompletedSoundEvent, gameObject);
                SoundManager.Instance.StopSound(RepairSoundEvent, gameObject);

            }
            else if(CraftingItem.Progress > 100)
                SoundManager.Instance.PlaySound(overcraftSound, gameObject, SoundManager.SoundType.Loop);

        }
        else
        {
            
            Crafting = false;
            OverCrafting = false;
            SoundManager.Instance.StopSound(RepairSoundEvent, gameObject);
            SoundManager.Instance.StopSound(overcraftSound, gameObject);

        }
    }
}
