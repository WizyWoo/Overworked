using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingStation : WorkStation
{

    public CraftableItem RecipeItem1, RecipeItem2;
    public GameObject Result;
    public bool ResultIsAssembled;
    private int recipeID1, recipeID2;
    private bool part1Ready, part2Ready;
    private GameObject part1, part2;

    private void Start()
    {

        recipeID1 = RecipeItem1.ID;
        recipeID2 = RecipeItem2.ID;

    }

    public override void Activate(Transform _player = null, bool _buttonDown = true)
    {

        if(!_player)
        {

            InUse = false;
            UsedBy = null;
            return;

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

    public override bool PlaceItem(GrabbableItem _item)
    {

        if(_item.OnWorkstation)
            return false;

        ItemOnStaion = _item;

        if(!ItemOnStaion.TryGetComponent<CraftableItem>(out CraftingItem))
        {

            RemoveItem();
            return false;

        }
        else if(!CraftingItem.Assembled)
        {

            RemoveItem();
            return false;

        }
        else
        {

            if(CraftingItem.ID == recipeID1 && !part1Ready)
            {

                part1Ready = true;
                part1 = CraftingItem.gameObject;
                ItemOnStaion.UngrabItem();
                ItemOnStaion.transform.SetParent(null);
                ItemOnStaion.transform.position = DisplayPoint.position;
                ItemOnStaion.OnWorkstation = this;

            }
            else if(CraftingItem.ID == recipeID2 && !part2Ready)
            {

                part2Ready = true;
                part2 = CraftingItem.gameObject;
                ItemOnStaion.UngrabItem();
                ItemOnStaion.transform.SetParent(null);
                ItemOnStaion.transform.position = DisplayPoint.position;
                ItemOnStaion.OnWorkstation = this;

            }
            else
            {

                RemoveItem();
                return false;

            }

        }

        if(part1Ready && part2Ready)
        {
            
            part1.transform.position = Vector3.down * 10;
            part2.transform.position = Vector3.down * 10;
            //Destroy(part1);
            //Destroy(part2);
            part1Ready = false;
            part2Ready = false;
            GameObject _tempGO = Instantiate(Result, DisplayPoint.position, Quaternion.identity);

            if(ResultIsAssembled)
                _tempGO.GetComponent<CraftableItem>().Assembled = true;

        }

        return true;

    }

    public override void RemoveItem()
    {

        if(ItemOnStaion.gameObject == part1)
            part1Ready = false;
        else if(ItemOnStaion.gameObject == part2)
            part2Ready = false;
        
        base.RemoveItem();
        
    }

}
