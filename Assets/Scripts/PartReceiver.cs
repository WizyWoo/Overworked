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
    [SerializeField] LevelManager level_Manager;
    [SerializeField] private CraftableItem.TypeOfRepairableItem deliveryItemType;
    [SerializeField]
    private LocalMultiplayer_Manager localMultiplayer;
    [SerializeField]
    private Outline outlineScript;
    [SerializeField]
    private Material indicatorLightMatCorrect, indicatorLightMatWrong, indicatorLightMatPending;
    [SerializeField]
    private MeshRenderer scannerLights;

    CraftableItem currentItem;

    private void FixedUpdate()
    {

        foreach (PlayerController _pC in localMultiplayer.allPlayers)
        {

            Debug.Log(_pC.gameObject.name);

            if(_pC.itemGrabbed && _pC.itemGrabbed.TryGetComponent<CraftableItem>(out CraftableItem _cI))
            {

                if(_cI.typeOfItem == deliveryItemType && _cI.Assembled)
                    outlineScript.enabled = true;
                else
                    outlineScript.enabled = false;

            }
            else
                outlineScript.enabled = false;
            
        }

    }

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
            level_Manager.CorrectRobot();
            scannerLights.material = indicatorLightMatCorrect;
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
        scannerLights.material = indicatorLightMatPending;
        platform.transform.DOMove(pointA.position, 1);
    }

}
