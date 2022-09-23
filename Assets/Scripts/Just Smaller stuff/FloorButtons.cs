using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorButtons : MonoBehaviour
{
    public bool isPressed;
    // Start is called before the first frame update
   
    private void OnTriggerEnter(Collider other)
    {if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            isPressed = true;
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            isPressed = false;
        }
          
    }
}
