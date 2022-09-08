using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public Transform[] RespawnPoint;
    public bool Respawning;
    public int TotalRespawns;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        other.transform.position = RespawnPoint[Random.Range(0, RespawnPoint.Length)].position;
        Respawning = true;
        if(other.name == "Player(Clone)")
        {
            TotalRespawns++;
        }
        
    }
}
