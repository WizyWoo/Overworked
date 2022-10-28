using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfusionCreator : MonoBehaviour
{
    
    
   
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
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            other.GetComponent<PlayerController>().Dazed = true;
            other.GetComponent<PlayerController>().DazedOnce = true;
            other.GetComponent<PlayerController>().DazedTimer = 5;
        }
    }
}
