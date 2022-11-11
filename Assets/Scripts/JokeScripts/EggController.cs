using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggController : MonoBehaviour
{
    public KonamiCode KC;
    public GameObject Egg;
    // Start is called before the first frame update
    void Start()
    {
        Egg = GameObject.Find("EasterEgg");
        KC = GameObject.Find("MenuKC").GetComponent<KonamiCode>();
        Egg.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (KC.KC == true)
        {
            Egg.SetActive(true);
            GameManager.instance.KonamiCode = true;
        }
        if(KC.KC == false)
        {
            Egg.SetActive(false);
            GameManager.instance.KonamiCode = false;
        }
    }
}
