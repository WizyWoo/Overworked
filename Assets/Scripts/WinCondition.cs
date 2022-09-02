using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCondition : MonoBehaviour
{
    public bool Win;
    public GameObject WinnerHud;
    // Start is called before the first frame update
    void Start()
    {
        WinnerHud.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // if Win is true, sets timescale to 0.001 making the game slower
        // turning on the WinnerHud object containing  winning "hud"
        // as a result of this physics will be kinda slow until reset back to 1


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
