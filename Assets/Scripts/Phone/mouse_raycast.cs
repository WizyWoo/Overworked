using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouse_raycast : MonoBehaviour
{
    public Camera main;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    

    // Update is called once per frame
    void Update()
    {
       if (Input.anyKey)
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray,out hit,100/*,LayerMask.GetMask("UI")*/))    
            {
                Debug.Log(hit.transform.gameObject.name);       
            }

        }
    }
}
