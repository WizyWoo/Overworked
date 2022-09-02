using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseCondition : MonoBehaviour
{
    public bool Game_Over;
    public GameObject LoserHud;
    // Start is called before the first frame update
    void Start()
    {
        LoserHud.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // if Game_Over is true, sets timescale to 0.001   making the game slower     
        // turning on the LoserHud object containing the Game Over "hud"
        // as a result of this physics will be kinda slow until reset back to 1


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
