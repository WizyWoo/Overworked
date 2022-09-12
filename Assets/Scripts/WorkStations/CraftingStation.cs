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
    [Tooltip("The ID for what items can be used for crafting, id found on CraftableItems. Order doesn't matter :) Use this if you don't want to input RecipeItem 1 and 2"), SerializeField]
    private int recipeID1, recipeID2;

    private void Start()
    {

        if(recipeItem1 || recipeItem2)
        {

            recipeID1 = recipeItem1.ID;
            recipeID2 = recipeItem2.ID;
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

        if(ItemOnStaion)
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

        if(_item.OnWorkstation || ItemOnStaion && !recipeItem1 && !recipeItem2)
            return false;

        ItemOnStaion = _item;

        if(!ItemOnStaion.TryGetComponent<CraftableItem>(out CraftingItem))
        {

            RemoveItem(ItemOnStaion);
            return false;

        }
        else if(!CraftingItem.Assembled && !CraftingItem.NeedsCrafting)
        {

            RemoveItem(ItemOnStaion);
            return false;

        }
        else if(CraftingItem.NeedsCrafting && !recipeItem1 && !recipeItem2)
        {

            ItemOnStaion.UngrabItem();
            ItemOnStaion.transform.SetParent(null);
            ItemOnStaion.transform.position = DisplayPoint.position;
            ItemOnStaion.OnWorkstation = this;
            _stopPlz = true;

        }
        else
        {

            if(CraftingItem.ID == recipeID1 && !recipeItem1)
            {

                recipeItem1 = CraftingItem;
                ItemOnStaion.UngrabItem();
                ItemOnStaion.transform.SetParent(null);
                ItemOnStaion.transform.position = DisplayPoint.position;
                ItemOnStaion.OnWorkstation = this;

            }
            else if(CraftingItem.ID == recipeID2 && !recipeItem2)
            {

                recipeItem2 = CraftingItem;
                ItemOnStaion.UngrabItem();
                ItemOnStaion.transform.SetParent(null);
                ItemOnStaion.transform.position = DisplayPoint.position;
                ItemOnStaion.OnWorkstation = this;

            }
            else
            {

                RemoveItem(ItemOnStaion);
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
            ItemOnStaion = _tempGI;
            CraftingItem = _tempGO.GetComponent<CraftableItem>();
            CraftingItem.NeedsCrafting = true;

            if(ResultIsAssembled)
                _tempGO.GetComponent<CraftableItem>().Assembled = true;

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

        if(!UsedBy)
            return;

        if(Vector3.Distance(UsedBy.transform.position, transform.position) > UseRange)
        {

            InUse = false;
            return;

        }

        if(InUse && CraftingItem.NeedsCrafting && !UsedBy.exhausted)
        {

            UsedBy.DoingWork(WorkIntensity);
            CraftingItem.Progress += CraftingSpeed * Time.deltaTime;

        }

    }

}
