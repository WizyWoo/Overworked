using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PartReceiver : MonoBehaviour
{

    [SerializeField] Transform platform;
    [SerializeField] Transform robotSpot;

    [SerializeField] Transform pointA;
    [SerializeField] Transform pointB;
    [SerializeField] Level02_Manager level02_Manager;
    [SerializeField] private CraftableItem.TypeOfRepairableItem deliveryItemType;

    CraftableItem currentItem;

    private void OnTriggerEnter(Collider other)
    {
        CraftableItem item = other.GetComponent<CraftableItem>();

        if (item != null && item.typeOfItem == deliveryItemType && currentItem == null && item.Assembled)
        {
            Debug.Log(item.Assembled);
            PlayerController pl = item.transform.GetComponentInParent<PlayerController>();
            if (pl != null)
            {
                StartCoroutine(pl.TryRemoveGrabbableItemFromList(item));
                pl.RemoveItem(item);
                pl.itemGrabbed = null;
            }
            level02_Manager.CorrectRobot();
            item.GrabItem();
            item.transform.SetParent(robotSpot);
            item.transform.DOMove(robotSpot.position, .5f).OnComplete(TakeAwayRobot);

            currentItem = item;

            item.GetComponent<CraftableItem>().delivered = true;

            item.enabled = false;
        }
    }


    void TakeAwayRobot()
    {
        
        platform.transform.DOMove(pointB.position, 1).OnComplete(PreparePlatfomrForNextRobot);
    }

    void PreparePlatfomrForNextRobot()
    {
        Destroy(currentItem.gameObject);
        currentItem = null;
        platform.transform.DOMove(pointA.position, 1);
    }

}
