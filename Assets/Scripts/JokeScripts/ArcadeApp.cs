using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcadeApp : MonoBehaviour
{
    public GameObject Easy, Hard, Arcade, easyLight, hardLight, arcadeLight;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.KonamiCode== true)
        {
            Easy.SetActive(true);
            Hard.SetActive(true);
            Arcade.SetActive(true);
        }
        if(GameManager.instance.AMAEasyMode == true)
        
            Easy.SetActive(true);

        if(GameManager.instance.AMAHardMode == true)
            Hard.SetActive(true);

        if(GameManager.instance.ArcadeModeApp == true)
       
                Arcade.SetActive(true);
       
        easyLight.SetActive(GameManager.instance.Easy);
        hardLight.SetActive(GameManager.instance.Hard);
        arcadeLight.SetActive(GameManager.instance.ArcadeMode);

    }
   
}
