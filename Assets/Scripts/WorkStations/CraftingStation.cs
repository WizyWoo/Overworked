using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingStation : WorkStation
{

    [Space]
    [Header("Crafting"), Tooltip("The Items needed to craft an item. Order doesn't matter :) Can use this instead of inputting ID below"), SerializeField]
    private CraftableItem recipeItem1;
    [Tooltip("The Items needed to craft an item. Order doesn't matter :) Can use this instead of inputting ID below"), SerializeField]
    private CraftableItem recipeItem2;
    [Tooltip("The prefab that should spawn when the recipe items are combined")]
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

        ItemOnStation = _item;

        if(!ItemOnStation.TryGetComponent<CraftableItem>(out CraftingItem))
        {

            RemoveItem(ItemOnStation);
            return false;

        }
        else if(!CraftingItem.Assembled && !CraftingItem.NeedsCrafting)
        {

            RemoveItem(ItemOnStation);
            return false;

        }
        else if(CraftingItem.NeedsCrafting && !recipeItem1 && !recipeItem2)
        {

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

                recipeItem1 = CraftingItem;
                ItemOnStation.UngrabItem();
                ItemOnStation.transform.SetParent(null);
                ItemOnStation.transform.position = DisplayPoint.position;
                ItemOnStation.OnWorkstation = this;

                SoundManager.Instance.PlaySound(SoundEvent, gameObject);

            }
            else if(CraftingItem.typeOfItem == recipeID2 && !recipeItem2)
            {

                recipeItem2 = CraftingItem;
                ItemOnStation.UngrabItem();
                ItemOnStation.transform.SetParent(null);
                ItemOnStation.transform.position = DisplayPoint.position;
                ItemOnStation.OnWorkstation = this;

                SoundManager.Instance.PlaySound(SoundEvent, gameObject);

            }
            else
            {

                RemoveItem(ItemOnStation);
                return false;

            }

        }

        if(recipeItem1 && recipeItem2 && !_stopPlz)
        {
            
            recipeItem1.transform.position = Vector3.down * 10;
            recipeItem2.transform.position = Vector3.down * 10;
            recipeItem1 = null;
            recipeItem2 = null;
            //Destroy(recipeItem1);
            //Destroy(recipeItem2);
            GameObject _tempGO = Instantiate(Result, DisplayPoint.position, Quaternion.identity);
            GrabbableItem _tempGI = _tempGO.GetComponent<GrabbableItem>();
            _tempGI.OnWorkstation = this;
            ItemOnStation = _tempGI;
            CraftingItem = _tempGO.GetComponent<CraftableItem>();

            if(ResultIsAssembled)
            {

                CraftingItem.Assembled = true;
                CraftingItem.NeedsCrafting = false;

            }
            else
            {

                CraftingItem.Assembled = false;
                CraftingItem.NeedsCrafting = true;

            }

        }

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

    //While UsedBy is set, you can craft shit
    private void Update()
    {

        if(!UsedBy || OutOfPower)
        {

            SoundManager.Instance.StopSound(CraftingSoundEvent, gameObject);
            return;

        }

        if(Vector3.Distance(UsedBy.transform.position, transform.position) > UseRange)
        {

            InUse = false;
            return;

        }

        if(InUse && CraftingItem.NeedsCrafting && !UsedBy.exhausted)
        {

            UsedBy.DoingWork(WorkIntensity);
            CraftingItem.Progress += CraftingSpeed * Time.deltaTime;

            if(CraftingItem.Progress >= 100)
                SoundManager.Instance.PlaySound(CompletedSoundEvent, gameObject);

            SoundManager.Instance.PlaySound(CraftingSoundEvent, gameObject, SoundManager.SoundType.Loop);

        }

    }

}
