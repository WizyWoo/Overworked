using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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


    private void Awake()
    {
        leftArmsAssembled = new bool[3];
        rightArmsAssembled = new bool[3];
        wheelsAssembled = new bool[3];
    }

    enum itemTriggered { leftArm, rightArm, wheel }

    public void LeftArmTrigger(OnTriggerDelegation3D delegation)
    { ItemTrigger(itemTriggered.leftArm, delegation); }

    public void RightArmTrigger(OnTriggerDelegation3D delegation)
    {
        ItemTrigger(itemTriggered.rightArm, delegation);
    }

    public void WheelTrigger(OnTriggerDelegation3D delegation)
    { ItemTrigger(itemTriggered.wheel, delegation); }


    // -1 = Left // 1 = Right
    void ItemTrigger(itemTriggered typeOfItem, OnTriggerDelegation3D delegation)
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
                // Get correct index
                int index = SelectSpotIndex_Outlines(typeOfItem);
                if (index >= 3) return;


                // LEFT
                // Store gameopbject information in the outlinesArray
                if (typeOfItem == itemTriggered.leftArm)
                {
                    leftArmOutlineObject[index] = item.gameObject;
                    // Select spot
                    newSpot = leftArmsSpots[index];
                }
                // RIGHT
                else
                {
                    rightArmOutlineObject[index] = item.gameObject;
                    // Select spot
                    newSpot = rightArmsSpots[index];
                }
            }
            else if (item.typeOfItem == CraftableItem.TypeOfRepairableItem.wheelOutline)
            {
                // Get correct index
                int index = SelectSpotIndex_Outlines(typeOfItem);
                if (index >= 3) return;

                wheelOutlineObject[index] = item.gameObject;
                // Select spot
                newSpot = wheelsSpots[index];
            }
            else if (item.typeOfItem == CraftableItem.TypeOfRepairableItem.arm)
            {
                int index = SelectSpotIndex(typeOfItem);
                if (index >= 3) return;

                if (typeOfItem == itemTriggered.leftArm)
                {
                    newSpot = leftArmsSpots[index];
                    leftArmsAssembled[index] = true;

                    if (leftArmOutlineObject[index] != null) leftArmOutlineObject[index].SetActive(false);
                }
                else
                {
                    newSpot = rightArmsSpots[index];
                    rightArmsAssembled[index] = true;

                    if (rightArmOutlineObject[index] != null) rightArmOutlineObject[index].SetActive(false);
                }

                LevelManager.Instance.CorrectRobot();

                CheckWin();
            }
            else if (item.typeOfItem == CraftableItem.TypeOfRepairableItem.wheel)
            {
                int index = SelectSpotIndex(typeOfItem);
                if (index >= 3) return;

                newSpot = wheelsSpots[index];
                wheelsAssembled[index] = true;

                if (wheelOutlineObject[index] != null) wheelOutlineObject[index].SetActive(false);

                LevelManager.Instance.CorrectRobot();

                CheckWin();
            }

            item.transform.DOKill();

            // Set the rotation normal
            item.transform.GetChild(0).rotation = Quaternion.Euler(90, 0, 0);

            float armSize = .35f;
            float wheelSize = .2f;

            int side;
            if (typeOfItem == itemTriggered.leftArm) side = -1;
            else side = 1;

            if (typeOfItem == itemTriggered.leftArm || typeOfItem == itemTriggered.rightArm)
                item.transform.DOScale(new Vector3(side * armSize, armSize, armSize), 1);
            else if (typeOfItem == itemTriggered.wheel)
                item.transform.DOScale(new Vector3(side * wheelSize * 1.6f, wheelSize, wheelSize), 1);

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

    int SelectSpotIndex_Outlines(itemTriggered typeOfItem)
    {
        int i = 0;

        if (typeOfItem == itemTriggered.leftArm)
            while (i < 3 && leftArmOutlineObject[i] != null) i++;
        else if (typeOfItem == itemTriggered.rightArm)
            while (rightArmOutlineObject[i] != null) i++;
        else if (typeOfItem == itemTriggered.wheel)
            while (wheelOutlineObject[i] != null) i++;

        return i;
    }

    int SelectSpotIndex(itemTriggered typeOfItem)
    {
        int i = 0;

        if (typeOfItem == itemTriggered.leftArm)
            while (i < 3 && leftArmsAssembled[i]) i++;
        else if (typeOfItem == itemTriggered.rightArm)
            while (i < 3 && rightArmsAssembled[i]) i++;
        else if (typeOfItem == itemTriggered.wheel)
            while (i < 3 && wheelsAssembled[i]) i++;

        return i;
    }

    void CheckWin()
    {
        bool allLeftArmAssembled = true;
        bool allRightArmAssembled = true;
        bool allWheelsAssembled = true;

        for (int i = 0; i < 3; i++)
            if (!leftArmsAssembled[i]) allLeftArmAssembled = false;

        for (int i = 0; i < 3; i++)
            if (!rightArmsAssembled[i]) allRightArmAssembled = false;

        for (int i = 0; i < 3; i++)
            if (!wheelsAssembled[i]) allWheelsAssembled = false;

        if (allLeftArmAssembled && allRightArmAssembled && allWheelsAssembled)
            LevelManager.Instance.Win();
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
