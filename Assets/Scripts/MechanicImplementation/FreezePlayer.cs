using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezePlayer : MonoBehaviour
{
    public bool Frozen;
    public GameObject IceCubePNG;
    private float Timer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Timer <= 0)
        {
            Frozen = false;
        }
        if (Frozen == false)
        {
            IceCubePNG.SetActive(false);
            gameObject.GetComponent<PlayerController>().enabled = true;
            Timer = 2;
        }
        if (Frozen == true)
        {
            IceCubePNG.SetActive(true);
            gameObject.GetComponent<PlayerController>().enabled = false;
            Timer -= Time.deltaTime;
        }
    }
}