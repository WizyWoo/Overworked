using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftableItem : GrabbableItem
{

    private bool assembled;
    public bool Assembled
    {
        get
        {
            return assembled;
        }
        set
        {
            assembled = value;
            if(value)
                progress = 100;
        }
    }
    private float progress;
    public float Progress
    {
        get
        {
            return progress;
        }
        set
        {
            progress = value;
            if(progress >= 100)
            {
                assembled = true;
            }
        }
    }

}
