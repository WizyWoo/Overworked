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
    private void OnCollisionEnter(Collision _col)
    {
        if (_col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _col.gameObject.GetComponent<PlayerController>().Dazed = true;
            _col.gameObject.GetComponent<PlayerController>().DazedOnce = true;
            _col.gameObject.GetComponent<PlayerController>().DazedTimer = 5;
        }
    }
}
