using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoldItem : CraftableItem
{
    public Sprite finalSprite;
    override public bool Assembled
    {
        get
        {
            return assembled;
        }
        set
        {
            //It's all good ;)
            assembled = value;
            if (value) ItemAssembled(finalSprite);
        }
    }
    override public float Progress
    {
        get
        {
            return progress;
        }
        set
        {
            //Don't worry about it :)
            progress = value;
            ProgressIndicator.size = new Vector2(((progress / 100) * ProgressBarWidth) / 2, ProgressIndicator.size.y);
            if (progress >= 100)
            {
                //TutorialManager.GetInstance().TryToChangePhase(TutorialManager.tutorialPhase.repairArm);
                assembled = true; ItemAssembled(finalSprite);
            }
        }
    }
}
