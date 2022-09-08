using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingStation : WorkStation
{

    public CraftableItem RecipeItem1, RecipeItem2;
    public GameObject Result;
    private int recipeID1, recipeID2;
    private bool part1, part2;

    private void Start()
    {

        recipeID1 = RecipeItem1.ID;
        recipeID2 = RecipeItem2.ID;

    }

    public override void PlaceItem(GrabbableItem _item)
    {

        ItemOnStaion = _item;

        if(!ItemOnStaion.TryGetComponent<CraftableItem>(out CraftingItem))
            RemoveItem();
        else if(CraftingItem.Assembled)
            RemoveItem();
        else
        {

            ItemOnStaion.transform.SetParent(null);
            ItemOnStaion.transform.position = DisplayPoint.position;

        }

    }

}
