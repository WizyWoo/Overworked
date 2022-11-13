using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class EggCheck : MonoBehaviour
{
    public GameObject Egg, Dog;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.KonamiCode == true)
        {
            Egg.SetActive(true);
            Dog.SetActive(false);
        }
        if(GameManager.instance.KonamiCode == false)
        {
            Egg.SetActive(false);
            Dog.SetActive(true);
        }
    }
}
