using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UnlockAll : MonoBehaviour
{
    public Button[] LockedButtons;
    public GameObject[] LockedLevels;
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {if(GameManager.instance.UnlockEverything == true)
        {
            foreach (GameObject Levels in LockedLevels)
            {
                Levels.SetActive(false);
            }
            foreach(Button Buttons in LockedButtons)
            {
                Buttons.enabled = true;
            }
        }
        
    }
}
