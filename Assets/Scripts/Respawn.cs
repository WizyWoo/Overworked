using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Respawn : MonoBehaviour
{

    //as there is a proper respawn for the player I am changing the respawn to a fake one.
    // just to check and update the death counter
    // public Transform[] RespawnPoint;
    public bool Respawning;
    public int TotalRespawns;
    public Text Deathcount;
    private void Start()
    {
        Deathcount = GameObject.Find("DeathCount").GetComponentInChildren<Text>();
    }
    private void OnTriggerEnter(Collider other)
    {
        //other.transform.position = RespawnPoint[Random.Range(0, RespawnPoint.Length)].position;
        
        if(other.name == "Player(Clone)")
        {
            Respawning = true;
            TotalRespawns++;
            Deathcount.text = TotalRespawns.ToString() + " accident";
            if (TotalRespawns > 1)
            {
                Deathcount.text = TotalRespawns.ToString() + " accidents";
            }
        }
        
    }
}
