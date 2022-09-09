using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoseCondition : MonoBehaviour
{
    public bool Game_Over;
    public int Failed_Robots, Successful_Robots;
    private WinCondition WC;
    public GameObject LoserHud;
    private Text TimerText;
    public float Timer, seconds;
    [Tooltip("The Objects with the tag delivery spot will show up here, please tag them so this works")]
    public RobotDeliverySpot[] DeliverySpots;
   public GameObject[] DeliveryGameObjects;
    // Start is called before the first frame update
    void Start()
    {
        if (Timer == 0)
            Timer = 60;

        LoserHud.SetActive(false);
        //We find the deliveryspots, usually whatever is tagged deliveryspot
        //then define the length of the array and fill it in
        WC = gameObject.GetComponent<WinCondition>();
        DeliveryGameObjects = GameObject.FindGameObjectsWithTag("DeliverySpot");
        DeliverySpots = new RobotDeliverySpot[DeliveryGameObjects.Length];
        for (int i = 0; i < DeliveryGameObjects.Length; i++)
        {
            DeliverySpots[i] = DeliveryGameObjects[i].GetComponent<RobotDeliverySpot>();
        }
        TimerText = gameObject.transform.Find("TimerAndFails").GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < DeliverySpots.Length; i++)
        {
            if (DeliverySpots[i].IncrementLoseCon == true)
            {
                if (DeliverySpots[i].IncrementWinCon == true)
                {
                    
                    DeliverySpots[i].IncrementWinCon = false;
                    
                }
                Failed_Robots++;
                DeliverySpots[i].IncrementLoseCon = false;
            }
        }
        Successful_Robots = WC.Current_RobotChain;
        // if Game_Over is true, sets timescale to 0.001   making the game slower     
        // turning on the LoserHud object containing the Game Over "hud"
        // as a result of this physics will be kinda slow until reset back to 1
        Timer -= Time.deltaTime;
         seconds = Mathf.FloorToInt(Timer % 60);
        TimerText.text = seconds.ToString();
        if (Successful_Robots >= 4)
        {
            Failed_Robots--;
            Successful_Robots = 0;
        }
        if(Failed_Robots >= 20)
        {
            Game_Over = true;
        }
        if(Timer < 0)
        {
            Game_Over = true;
        }
        if (Game_Over == true)
        {
            Time.timeScale = 0.001f;
            if(Time.timeScale == 0.001f)
            {
               LoserHud.SetActive(true);
            }
        }
    }
}
