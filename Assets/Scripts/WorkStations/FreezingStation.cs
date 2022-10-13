using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezingStation : WorkStation
{

    [Header("Repairing")]
    [Tooltip("Specifies what items can be repaired at this Station")]
    [SerializeField] CraftableItem.TypeOfRepairableItem itemToFreeze;
    [SerializeField] Sprite itemItCreates;
    [SerializeField] GameObject freezeRay;
    public FMODUnity.EventReference FreezeSoundEvent;

    [SerializeField] bool freezing;
    [SerializeField] ParticleSystem[] particles;

    private void Start()
    {
        //freezeRay.transform.position = new Vector3((transform.position.x - DisplayPoint.localPosition.x / 2), transform.position.y, transform.position.z);
        //freezeRay.transform.localScale = new Vector3((freezeRay.transform.position.x - transform.position.x), freezeRay.transform.localScale.y, freezeRay.transform.localScale.z);
    }

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
        freezeRay.SetActive(freezing);
        foreach (ParticleSystem p in particles)
        {

            if(!p.isPlaying && freezing)
                p.Play();
            else if(!freezing)
                p.Stop();
            
        }

        if (!UsedBy && !AutoRepair || OutOfPower)
        {
            freezing = false;
            SoundManager.Instance.StopSound(FreezeSoundEvent, gameObject);
            return;
        }
        else if (UsedBy && !AutoRepair)
        {
            if (Vector3.Distance(UsedBy.transform.position, transform.position) > UseRange && !AutoRepair)
            {
                InUse = false;
                freezing = false;
                SoundManager.Instance.StopSound(FreezeSoundEvent, gameObject);
                return;
            }
            if (UsedBy.exhausted)
            {
                freezing = false;
                SoundManager.Instance.StopSound(FreezeSoundEvent, gameObject);
                return;
            }
            if (InUse && CraftingItem.typeOfItem == itemToFreeze && !AutoRepair)
            {
                freezing = true;
                UsedBy.DoingWork(WorkIntensity);
                CraftingItem.Progress += CraftingSpeed * Time.deltaTime;
                if (CraftingItem.Progress >= 100)
                {
                    freezing = false;
                    SoundManager.Instance.PlaySound(CompletedSoundEvent, gameObject);
                    RemoveItem(CraftingItem);
                }
                else SoundManager.Instance.PlaySound(FreezeSoundEvent, gameObject, SoundManager.SoundType.Loop);
            }
            else { SoundManager.Instance.StopSound(FreezeSoundEvent, gameObject); freezing = false; }
        }
        else if (AutoRepair && CraftingItem)
        {
            if (AutoRepair && CraftingItem.typeOfItem == itemToFreeze)
            {
                freezing = true;
                CraftingItem.Progress += CraftingSpeed * Time.deltaTime;
                if (CraftingItem.Progress >= 100)
                {
                    freezing = false;
                    SoundManager.Instance.PlaySound(CompletedSoundEvent, gameObject);
                    SoundManager.Instance.StopSound(FreezeSoundEvent, gameObject);
                    RemoveItem(CraftingItem);
                }
                else SoundManager.Instance.PlaySound(FreezeSoundEvent, gameObject, SoundManager.SoundType.Loop);
            }
            else { SoundManager.Instance.StopSound(FreezeSoundEvent, gameObject); freezing = false; }
        }

    }
}
