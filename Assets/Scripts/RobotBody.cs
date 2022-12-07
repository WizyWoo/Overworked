using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RobotBody : MonoBehaviour
{
    [SerializeField] Transform leftArm_Spot, rightArm_Spot, wheel_Spot;

    [HideInInspector] public bool leftArmAssembled, rightArmAssembled, wheelAssembled;

    public FMODUnity.EventReference assembleSound;

    private GameObject wheelOutlineObject, rightArmOutlineObject;

    bool justSpawned = true;
    private void Awake()
    {
        Invoke("TimePassed", 5);
    }

    void TimePassed()
    {
        justSpawned = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        CraftableItem item = other.GetComponent<CraftableItem>();

        if (item != null)
        {
            //// Comprobar si esta crafteado
            if (!item.Assembled) return;

            // Compobar en que lado se implementa
            Transform newSpot = null;

            if(item.typeOfItem == CraftableItem.TypeOfRepairableItem.armOutline)
            {
                //if (!leftArmAssembled)
                //{
                //    newSpot = leftArm_Spot;
                //    item.transform.GetComponentInChildren<SpriteRenderer>().flipX = true;
                //}
                //else if (!rightArmAssembled)
                //{

                //For the moment it just spawns and assemble in the right part,
                //if we need uncomment de code of this if
                newSpot = rightArm_Spot;
                rightArmOutlineObject = item.gameObject;
                //}
            }
            else if (item.typeOfItem == CraftableItem.TypeOfRepairableItem.wheelOutline)
            {
                wheelOutlineObject = item.gameObject;
                newSpot = wheel_Spot;
            }
            if (item.typeOfItem == CraftableItem.TypeOfRepairableItem.arm)
            {
                //if (item.GetComponentInParent<PlayerController>() != null
                //    && TutorialManager.GetInstance().currentPhase != TutorialManager.tutorialPhase.assembleArm)
                //    return;
                
                //Just to bypass the code incase we need to use the emergency fix
                if (TutorialManager.GetInstance() == null)
                {
                    goto wecouldntfixit;
                }
                // Inform tutorial manager
                TutorialManager.GetInstance().TryToChangePhase(TutorialManager.tutorialPhase.assembleArm);
            wecouldntfixit:
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

                    if (rightArmOutlineObject) rightArmOutlineObject.SetActive(false);
                }
                else return;
            }
            else if (item.typeOfItem == CraftableItem.TypeOfRepairableItem.wheel)
            {if(TutorialManager.GetInstance() == null)
                {
                    goto wecouldntfixit;
                }
                TutorialManager.GetInstance().TryToChangePhase(TutorialManager.tutorialPhase.assembleOtherRobot);
                wecouldntfixit:
                if (!wheelAssembled)
                {
                    newSpot = wheel_Spot;
                    wheelAssembled = true;

                    if (wheelOutlineObject) wheelOutlineObject.SetActive(false);
                }
                else return;
            }

            // Set the rotation normal
            item.transform.GetChild(0).rotation = Quaternion.Euler(90, 0, 0);


            PlayerController pl = item.transform.GetComponentInParent<PlayerController>();
            if (pl != null)
            {
                pl.itemGrabbed = null;
                pl.RemoveItem(item);
            }

            if (!justSpawned)
                SoundManager.Instance.PlaySound(assembleSound, gameObject);

            item.GrabItem();
            item.transform.SetParent(newSpot);
            StartCoroutine(MoveItemToAssembledSpot(item.transform, newSpot.transform, .5f));

            item.GetComponent<CraftableItem>().delivered = true;

            if (item.outline != null) item.outline.enabled = false;

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
