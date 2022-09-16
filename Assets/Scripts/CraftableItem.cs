using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
namespace CustomStuffPog
{

    [CustomEditor(typeof(CraftableItem))]
    public class CraftableItemEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            CraftableItem _cI = (CraftableItem)target;

            if(GUILayout.Button("Pog"))
            {

                Debug.Log("Poggers :3");

            }
            
        }

    }
    
}
#endif

public class CraftableItem : GrabbableItem
{

    //Custom editor values


    //Regular stuff
    public enum TypeOfRepairableItem { arm, wheel, head, body, robot, battery }
    public TypeOfRepairableItem typeOfItem;
    public bool NeedsCrafting;
    public SpriteRenderer ProgressIndicator, ItemSprite;
    public Sprite AssembledItemSprite;
    [SerializeField]
    private bool assembled;
    public bool Assembled
    {
        get
        {
            return assembled;
        }
        set
        {
            //It's all good ;)
            assembled = value;
            if(value)
                ItemAssembled();
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
            //Don't worry about it :)
            progress = value;
            ProgressIndicator.size = new Vector2((progress / 100) * 20, ProgressIndicator.size.y);
            if(progress >= 100)
            {
                assembled = true;
                ItemAssembled();
            }
        }
    }

    //Called when Progress is set to 100 or Assembled is set to true. Just puts on new sprite and removes indicator :))))
    public void ItemAssembled()
    {

        ItemSprite.sprite = AssembledItemSprite;
        ProgressIndicator.size = Vector2.zero;
        NeedsCrafting = false;

    }

}
