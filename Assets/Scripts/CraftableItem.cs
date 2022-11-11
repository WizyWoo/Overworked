using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
namespace CustomStuffPog
{

    [CustomEditor(typeof(CraftableItem)), CanEditMultipleObjects]
    public class CraftableItemEditor : Editor
    {
        public override void OnInspectorGUI()
        {

            base.OnInspectorGUI();

            GUILayout.Label("Touch me :3");

            if (GUILayout.Button("Pog"))
            {

                Debug.LogError("You got pogged :)");

            }

        }

    }

}
#endif

public class CraftableItem : GrabbableItem
{
    //Regular stuff
    public enum TypeOfRepairableItem { arm, wheel, head, body, robot, battery, bucket, armOutline, wheelOutline }

    [Header("Change me if you want :)")]
    public TypeOfRepairableItem typeOfItem;
    public bool NeedsCrafting;
    [SerializeField, Tooltip("Recolours the item when it is assembled")]
    protected bool recolorWhenDone;
    public bool ResizeWhenDone;
    public Vector2 NewSize;
    public GameObject TurnOffWhenDone;
    [SerializeField]
    protected Color newColor;
    [SerializeField, Tooltip("Colors for the progress bar :)))")]
    protected Color overCraftingColor = Color.red, normalColor = Color.white;
    [SerializeField]
    protected float ProgressBarWidth = 10;
    [Space, Header("plz give reference")]
    public SpriteRenderer ProgressIndicator;
    public SpriteRenderer ItemSprite;
    public Sprite AssembledItemSprite;
    [SerializeField] protected bool assembled;
    public bool delivered;
    virtual public bool Assembled
    {
        get
        {
            return assembled;
        }
        set
        {
            //It's all good ;)
            assembled = value;
            if (value) ItemAssembled();
        }
    }

    protected float progress;
    virtual public float Progress
    {
        get
        {
            return progress;
        }
        set
        {
            //Don't worry about it :)
            progress = value;
            if(progress < 100)
            {

                ProgressIndicator.size = new Vector2(((progress / 100) * ProgressBarWidth) / 2, ProgressIndicator.size.y);


            }
            else if(progress >= 100 && !assembled)
            {//I moved the tutorial thingy

                assembled = true;
                ItemAssembled();

            }
            else if(progress < 200 && progress > 100)
            {

                ProgressIndicator.size = new Vector2((((progress - 100) / 100) * ProgressBarWidth) / 2, ProgressIndicator.size.y);
                ProgressIndicator.color = overCraftingColor;

            }
            else if(progress >= 200)
            {

                ItemOvercrafted();

            }
        }
    }

    public void ItemOvercrafted()
    {



    }

    //Called when Progress is set to 100 or Assembled is set to true. Just puts on new sprite and removes indicator :))))
    public void ItemAssembled()
    {
        ItemSprite.sprite = AssembledItemSprite;
        outline.sprite = AssembledItemSprite;
        NeedsCrafting = false;

        if(recolorWhenDone)
            ItemSprite.color = newColor;
        if(TurnOffWhenDone)
            TurnOffWhenDone.SetActive(false);
        if(ResizeWhenDone)
            ItemSprite.transform.localScale = NewSize;

        if (typeOfItem == TypeOfRepairableItem.arm)
            TutorialManager.GetInstance().TryToChangePhase(TutorialManager.tutorialPhase.repairArm);
        else if (typeOfItem == TypeOfRepairableItem.wheel)
            TutorialManager.GetInstance().TryToChangePhase(TutorialManager.tutorialPhase.repairWheel);
    }

    public void ItemAssembled(Sprite finalSprite = null)
    {
        if (finalSprite == null) ItemSprite.sprite = AssembledItemSprite;
        else ItemSprite.sprite = finalSprite;

        ProgressIndicator.size = Vector2.zero;
        NeedsCrafting = false;

        if (recolorWhenDone)
        {
            ItemSprite.color = newColor;
        }
    }
}
