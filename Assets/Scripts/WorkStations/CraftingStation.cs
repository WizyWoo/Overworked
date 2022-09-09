using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingStation : WorkStation
{

    [Tooltip("Order doesn't matter :)")]
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

    public override bool PlaceItem(GrabbableItem _item)
    {

        bool _stopPlz = false;

        if(_item.OnWorkstation)
            return false;

        ItemOnStaion = _item;

        if(!ItemOnStaion.TryGetComponent<CraftableItem>(out CraftingItem))
        {

            RemoveItem();
            return false;

        }
        else if(!CraftingItem.Assembled && !CraftingItem.NeedsCrafting)
        {

            RemoveItem();
            return false;

        }
        else if(CraftingItem.NeedsCrafting)
        {

            ItemOnStaion.UngrabItem();
            ItemOnStaion.transform.SetParent(null);
            ItemOnStaion.transform.position = DisplayPoint.position;
            ItemOnStaion.OnWorkstation = this;
            _stopPlz = true;

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

        if(part1Ready && part2Ready && !_stopPlz)
        {
            
            part1.transform.position = Vector3.down * 10;
            part2.transform.position = Vector3.down * 10;
            //Destroy(part1);
            //Destroy(part2);
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

    public override void RemoveItem()
    {

        if(ItemOnStaion)
        {

            if(ItemOnStaion.gameObject == part1)
                part1Ready = false;
            else if(ItemOnStaion.gameObject == part2)
                part2Ready = false;

        }
        
        base.RemoveItem();
        
    }

    private void Update()
    {

        if(!UsedBy)
            return;

        if(Vector3.Distance(UsedBy.transform.position, transform.position) > UseRange)
        {

            InUse = false;
            return;

        }

        if(InUse && part1Ready && part2Ready && CraftingItem.NeedsCrafting)
        {

            UsedBy.DoingWork(WorkIntensity);
            CraftingItem.Progress += CraftingSpeed * Time.deltaTime;
            if(CraftingItem.Progress >= 100)
            {
                
                part1Ready = false;
                part2Ready = false;

            }

        }

    }

}
