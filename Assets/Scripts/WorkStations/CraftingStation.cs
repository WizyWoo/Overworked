using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingStation : WorkStation
{

    [Space]
    private CraftableItem recipeItem1, recipeItem2;
    [Header("Crafting"), Tooltip("The prefab that should spawn when the recipe items are combined")]
    public GameObject Result;
    [Tooltip("Whether the result is finished by default or needs to be crafted")]
    public bool ResultIsAssembled;
    public FMODUnity.EventReference CraftingSoundEvent;
    [Tooltip("The ItemType for what items can be used for crafting. Order doesn't matter :) Use this if you don't want to input RecipeItem 1 and 2"), SerializeField]
    private CraftableItem.TypeOfRepairableItem recipeID1, recipeID2;

    private void Start()
    {

        if(recipeItem1 || recipeItem2)
        {

            recipeID1 = recipeItem1.typeOfItem;
            recipeID2 = recipeItem2.typeOfItem;
            recipeItem1 = null;
            recipeItem2 = null;

        }
        
    }

    //This shite happens when the player interacts with the station
    public override void Activate(Transform _player = null, bool _buttonDown = true)
    {

        if(!_player)
        {

            InUse = false;
            UsedBy = null;
            return;

        }

        if(ItemOnStation)
        {

            if(CraftingItem)
            {

                InUse = _buttonDown;
                UsedBy = _player.GetComponent<PlayerController>();

            }
            else
            {
                
                InUse = false;
                UsedBy = null;

            }

        }

        if(_player.GetComponent<PlayerController>().itemGrabbed)
        {

            PlayerController _pC = _player.GetComponent<PlayerController>();

            if(PlaceItem(_pC.itemGrabbed))
            {

                _pC.itemGrabbed = null;

            }

        }

    }

    //Checks if the input item is allowed on this station and if all nessecary items are present, it will output the result item which can then be crafted :)
    public override bool PlaceItem(GrabbableItem _item)
    {

        bool _stopPlz = false;

        if(_item.OnWorkstation || ItemOnStation && !recipeItem1 && !recipeItem2)
            return false;

        if(!_item.TryGetComponent<CraftableItem>(out CraftingItem))
        {

            RemoveItem(_item);
            return false;

        }
        else if(!CraftingItem.Assembled && !CraftingItem.NeedsCrafting)
        {

            RemoveItem(_item);
            return false;

        }
        else if(CraftingItem.NeedsCrafting && !recipeItem1 && !recipeItem2)
        {

            ItemOnStation = _item;

            ItemOnStation.UngrabItem();
            ItemOnStation.transform.SetParent(null);
            ItemOnStation.transform.position = DisplayPoint.position;
            ItemOnStation.OnWorkstation = this;
            _stopPlz = true;

            SoundManager.Instance.PlaySound(SoundEvent, gameObject);

        }
        else
        {

            if(CraftingItem.typeOfItem == recipeID1 && !recipeItem1)
            {

                ItemOnStation = _item;

                recipeItem1 = CraftingItem;
                ItemOnStation.UngrabItem();
                ItemOnStation.transform.SetParent(null);
                ItemOnStation.transform.position = DisplayPoint.position;
                ItemOnStation.OnWorkstation = this;

                SoundManager.Instance.PlaySound(SoundEvent, gameObject);

            }
            else if(CraftingItem.typeOfItem == recipeID2 && !recipeItem2)
            {

                ItemOnStation = _item;

                recipeItem2 = CraftingItem;
                ItemOnStation.UngrabItem();
                ItemOnStation.transform.SetParent(null);
                ItemOnStation.transform.position = DisplayPoint.position;
                ItemOnStation.OnWorkstation = this;

                SoundManager.Instance.PlaySound(SoundEvent, gameObject);

            }
            else
            {

                RemoveItem(_item);
                return false;

            }

        }

        if(recipeItem1 && recipeItem2 && !_stopPlz)
        {
            
            Destroy(recipeItem1.gameObject);
            Destroy(recipeItem2.gameObject);
            recipeItem1 = null;
            recipeItem2 = null;
            GameObject _tempGO = Instantiate(Result, DisplayPoint.position, Quaternion.identity);
            GrabbableItem _tempGI = _tempGO.GetComponent<GrabbableItem>();
            _tempGI.OnWorkstation = this;
            ItemOnStation = _tempGI;
            CraftingItem = _tempGO.GetComponent<CraftableItem>();

            if(ResultIsAssembled)
            {

                CraftingItem.Assembled = true;
                CraftingItem.NeedsCrafting = false;
                SoundManager.Instance.PlaySound(CompletedSoundEvent, gameObject);

            }
            else
            {

                CraftingItem.Assembled = false;
                CraftingItem.NeedsCrafting = true;

            }

        }

        ItemOnStation.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        return true;

    }

    //Some extra removing stuff so the recipe items arend stored
    public override void RemoveItem(GrabbableItem _item)
    {

        if(_item)
        {

            if(_item == recipeItem1)
                recipeItem1 = null;
            else if(_item == recipeItem2)
                recipeItem2 = null;

        }
        
        base.RemoveItem(_item);
        
    }

    protected override void Update()
    {
        base.Update();

        foreach (PlayerController _pC in localMultiplayer.allPlayers)
        {

            if(_pC.itemGrabbed && _pC.itemGrabbed.TryGetComponent<CraftableItem>(out CraftableItem _cI))
            {

                if((_cI.typeOfItem == recipeID1 && !recipeItem1) || (_cI.typeOfItem == recipeID2 && !recipeItem2))
                    outlineScript.enabled = true;

            }
            
        }

        if(!CraftingItem)
        {

            Crafting = false;
            OverCrafting = false;
            SoundManager.Instance.StopSound(overcraftSound, gameObject);
            SoundManager.Instance.StopSound(CraftingSoundEvent, gameObject);
            return;

        }

        if(CraftingItem.NeedsCrafting)
        {

            SoundManager.Instance.PlaySound(CraftingSoundEvent, gameObject, SoundManager.SoundType.Loop);
            Crafting = true;

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
                SoundManager.Instance.StopSound(CraftingSoundEvent, gameObject);
                SoundManager.Instance.PlaySound(overcraftSound, gameObject, SoundManager.SoundType.Loop);
            }

        }
        else
        {

            Crafting = false;
            OverCrafting = false;
            SoundManager.Instance.StopSound(overcraftSound, gameObject);
            SoundManager.Instance.StopSound(CraftingSoundEvent, gameObject);
        }


        /*if(!UsedBy || OutOfPower || UsedBy.exhausted || CraftingItem.Assembled)
        {

            SoundManager.Instance.StopSound(CraftingSoundEvent, gameObject);
            return;

        }

        if(Vector3.Distance(UsedBy.transform.position, transform.position) > UseRange)
        {

            InUse = false;
            return;

        }

        if(InUse && CraftingItem.NeedsCrafting)
        {

            UsedBy.DoingWork(WorkIntensity);
            CraftingItem.Progress += CraftingSpeed * Time.deltaTime;

            if(CraftingItem.Progress >= 100)
            {
                
                SoundManager.Instance.PlaySound(CompletedSoundEvent, gameObject);
                SoundManager.Instance.StopSound(CraftingSoundEvent, gameObject);

            }

            SoundManager.Instance.PlaySound(CraftingSoundEvent, gameObject, SoundManager.SoundType.Loop);

        }*/

    }

}
