using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantRobot : MonoBehaviour
{
    [HideInInspector] public bool[] leftArmsAssembled;
    [HideInInspector] public bool[] rightArmsAssembled;
    [HideInInspector] public bool[] wheelsAssembled;

    [SerializeField] Transform[] leftArmsSpots;
    [SerializeField] Transform[] rightArmsSpots;
    [SerializeField] Transform[] wheelsSpots;



    [SerializeField] GameObject[] leftArmOutlineObject;
    [SerializeField] GameObject[] rightArmOutlineObject;
    [SerializeField] GameObject[] wheelOutlineObject;



    public void LeftArmTrigger(OnTriggerDelegation delegation)
    { ArmTrigger(-1, delegation); }

    public void RightArmTrigger(OnTriggerDelegation delegation)
    { ArmTrigger(1, delegation); }


    // -1 = Left // 1 = Right
    void ArmTrigger(int side, OnTriggerDelegation delegation)
    {
        CraftableItem item = delegation.Other.GetComponent<CraftableItem>();

        if (item != null)
        {
            //// Comprobar si esta crafteado
            if (!item.Assembled) return;

            // Compobar en que lado se implementa
            Transform newSpot = null;

            if (item.typeOfItem == CraftableItem.TypeOfRepairableItem.armOutline)
            {
                if (side == -1)
                {

                }
                else
                {

                }

                int index = SelectArmSpotIndex_Outlines(side);

                // Select spot
                newSpot = leftArmsSpots[index];

                // Store gameopbject information in the outlinesArray
                if (side == -1)
                    leftArmOutlineObject[index] = item.gameObject;
                else
                    rightArmOutlineObject[index] = item.gameObject;
            }
            else if (item.typeOfItem == CraftableItem.TypeOfRepairableItem.wheelOutline)
            {
                //wheelOutlineObject = item.gameObject;
                //newSpot = wheel_Spot;
            }
            if (item.typeOfItem == CraftableItem.TypeOfRepairableItem.arm)
            {

            }

            // Set the rotation normal
            item.transform.GetChild(0).rotation = Quaternion.Euler(90, 0, 0);


            PlayerController pl = item.transform.GetComponentInParent<PlayerController>();
            if (pl != null)
            {
                pl.itemGrabbed = null;
                pl.RemoveItem(item);
            }

            //SoundManager.Instance.PlaySound(assembleSound, gameObject);

            item.GrabItem();
            item.transform.SetParent(newSpot);
            StartCoroutine(MoveItemToAssembledSpot(item.transform, newSpot.transform, .5f));

            item.GetComponent<CraftableItem>().delivered = true;

            item.enabled = false;
        }
    }

    public int SelectArmSpotIndex_Outlines(int side)
    {
        int i = 0;

        if (side == -1)
            while (leftArmOutlineObject[i] != null) i++;
        else
            while (rightArmOutlineObject[i] != null) i++;

        return i;
    }



    [SerializeField] AnimationCurve animationCurve;
    float assembleVelocity = 3;
    IEnumerator MoveItemToAssembledSpot(Transform item, Transform assembleSpot, float seconds)
    {
        Vector3 initialPosition = item.position;

        float c = 0;
        while (Vector3.Distance(assembleSpot.position, item.position) > 0.001f)
        {
            item.transform.position = Vector3.Lerp(initialPosition, assembleSpot.position, animationCurve.Evaluate(c));
            //item.transform.position = Vector3.Lerp(initialPosition, assembleSpot.position, c);
            c += (Time.deltaTime / seconds);
            c = Mathf.Clamp(c, 0, 1);
            yield return new WaitForSeconds(0);
        }
    }
}
