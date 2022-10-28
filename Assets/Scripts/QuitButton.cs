using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitButton : MonoBehaviour
{ 
    //Quits the game, only on Standalone Application builds
    public void QuitGame()
    {
        Application.Quit();
    }
}
