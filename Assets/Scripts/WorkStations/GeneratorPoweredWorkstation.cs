using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorPoweredWorkstation : MonoBehaviour
{
    private int PressedButtons;
    public GameObject[] Workstations, Buttons;
    public bool Working;
    public float RandomTimer;
    // Start is called before the first frame update
    void Start()
    {
        Workstations = GameObject.FindGameObjectsWithTag("RepairStation");
        Buttons = GameObject.FindGameObjectsWithTag("Buttons");
    }

    // Update is called once per frame
    void Update()
    {
        if(Working == true)
        {
            foreach(GameObject w in Workstations)
            {
                w.GetComponent<RepairStation>().enabled = true;
            }
        }
        if(Working == false)
        {
            foreach (GameObject w in Workstations)
            {
                w.GetComponent<RepairStation>().enabled = false;
            }
            foreach(GameObject b in Buttons)
            {
                if(b.GetComponent<FloorButtons>().isPressed == true)
                {
                    if (PressedButtons == 2)
                    {
                        Working = true;
                        RandomTimer = Random.Range(5, 10);
                        PressedButtons = 0;
                    }
                    PressedButtons++;
                    
                }
            }
        }

        RandomTimer -= Time.deltaTime;
        if (RandomTimer < 0 && Working == true)
        {
            Working = false;
        }
    }
}
