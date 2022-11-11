using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KonamiCode : MonoBehaviour
{
   private KeyCode[] konamiCode;
    int currentKeyIndex = 0;
    public int step;
    public bool KC;
    void Start()
    {
        
    }
    private void Update()
    {
        switch (step)
        {
            case 0:
                if (Input.GetKeyUp(KeyCode.UpArrow)) step++;
                break;
            case 1:
                if (Input.GetKeyUp(KeyCode.UpArrow)) step++;
                break;
            case 2:
                if (Input.GetKeyUp(KeyCode.DownArrow)) step++;
                break;
            case 3:
                if (Input.GetKeyUp(KeyCode.DownArrow)) step++;
                break;
            case 4:
                if (Input.GetKeyUp(KeyCode.LeftArrow)) step++;
                break;
            case 5:
                if (Input.GetKeyUp(KeyCode.RightArrow)) step++;
                break;
            case 6:
                if (Input.GetKeyUp(KeyCode.LeftArrow)) step++;
                break;
            case 7:
                if (Input.GetKeyUp(KeyCode.RightArrow)) step++;
                break;
            case 8:
                if (Input.GetKeyUp(KeyCode.B)) step++;
                break;
            case 9:
                if (Input.GetKeyUp(KeyCode.A)) step++;
                break;
            case 10:
                KC = true;
                break;
        }
    }
    
}
