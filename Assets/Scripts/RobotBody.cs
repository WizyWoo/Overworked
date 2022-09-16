using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RobotBody : MonoBehaviour
{
    [SerializeField] Transform leftArm_Spot, rightArm_Spot, wheel_Spot;

    [HideInInspector] public bool leftArmAssembled, rightArmAssembled, wheelAssembled;

    private void OnTriggerEnter(Collider other)
    {
        CraftableItem item = other.GetComponent<CraftableItem>();

        if (item != null)
        {
            //// Comprobar si esta crafteado
            if (!item.Assembled) return;


            // Compobar en que lado se implementa
            Transform newSpot = null;

            if (item.typeOfItem == CraftableItem.TypeOfRepairableItem.arm)
            {
                if (!leftArmAssembled)
                {
                    newSpot = leftArm_Spot;
                    item.transform.GetComponentInChildren<SpriteRenderer>().flipX = true;

                    leftArmAssembled = true;
                }
                else if (!rightArmAssembled)
                {
                    newSpot = rightArm_Spot;
                    rightArmAssembled = true;
                }
                else return;
            }
            else if (item.typeOfItem == CraftableItem.TypeOfRepairableItem.wheel)
            {
                if (!wheelAssembled)
                {
                    newSpot = wheel_Spot;
                    wheelAssembled = true;
                }
                else return;
            }

            // Set the rotation normal
            item.transform.GetChild(0).rotation = Quaternion.Euler(90, 0, 0);


            PlayerController pl = item.transform.GetComponentInParent<PlayerController>();
            if (pl != null)
            {
                pl.itemGrabbed = null;
                //pl.RemoveItem(item);
            }

            item.GrabItem();
            item.transform.SetParent(newSpot);
            StartCoroutine(MoveItemToAssembledSpot(item.transform, newSpot.transform, .5f));

            item.GetComponent<CraftableItem>().delivered = true;

            item.enabled = false;
        }
    }



    [SerializeField] AnimationCurve animationCurve;
    float assembleVelocity = 2;
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
