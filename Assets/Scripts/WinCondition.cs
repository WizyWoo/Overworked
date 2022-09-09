using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCondition : MonoBehaviour
{
    public bool Win;
    public int Wincondition_Robotchain, Current_RobotChain;
    public GameObject WinnerHud;
    public LoseCondition LC;
    [Tooltip("The Objects with the tag delivery spot will show up here, please tag them so this works")]
    public RobotDeliverySpot[] DeliverySpots;
    public GameObject[] DeliveryGameObjects;
    // Start is called before the first frame update
    void Start()
    {
        WinnerHud.SetActive(false);

        //We find the deliveryspots, usually whatever is tagged deliveryspot
        //then define the length of the array and fill it in
        DeliveryGameObjects = GameObject.FindGameObjectsWithTag("DeliverySpot");
        DeliverySpots = new RobotDeliverySpot[DeliveryGameObjects.Length];
        for (int i = 0; i < DeliveryGameObjects.Length; i++)
        {
            DeliverySpots[i] = DeliveryGameObjects[i].GetComponent<RobotDeliverySpot>();
        }
        if(Wincondition_Robotchain == 0)
        {
            Wincondition_Robotchain = 10;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //We check every deliveryspot, in our public list, see if they did it right or wrong then increment
        //it correctly, building up a chain of good robots
        for (int i = 0; i < DeliverySpots.Length; i++)
        {
            if (DeliverySpots[i].IncrementWinCon == true)
            {
                if (DeliverySpots[i].IncrementLoseCon == true)
                {
                    Current_RobotChain = 0;
                    DeliverySpots[i].IncrementLoseCon = false;
                    
                }
                Current_RobotChain++;
                DeliverySpots[i].IncrementWinCon = false;
            }
        }
    // if Win is true, sets timescale to 0.001 making the game slower
    // turning on the WinnerHud object containing  winning "hud"
    // as a result of this physics will be kinda slow until reset back to 1

    if(LC.Timer <= 0)
        {
            Win = true;
        }

        if(Wincondition_Robotchain <= Current_RobotChain)
        {
            //Win = true;
        }
        if(Win == true)
        {
            Time.timeScale = 0.001f;
            if(Time.timeScale == 0.001f)
            {
                WinnerHud.SetActive(true);
            }
        }
    }
}
