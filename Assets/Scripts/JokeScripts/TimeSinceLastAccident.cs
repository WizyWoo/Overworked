using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSinceLastAccident : MonoBehaviour
{
    public float Time_Since_Last_Respawn;
    public Respawn Respawningcheck;
    private bool WaitingbeforeTicking;
    private int WaitTimer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    wait3seconds:;
        if(WaitTimer >= 3)
        {
            Respawningcheck.Respawning = false;
            WaitTimer = 0;
        }
        if (Respawningcheck.Respawning == true)
        {
            Time_Since_Last_Respawn = 0;
            
            WaitTimer++;
            goto wait3seconds;
        }
       
            Time_Since_Last_Respawn += Time.deltaTime;
        
    }
}
