using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
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
        if(SceneManager.GetActiveScene().name != "MainMenu" && GameManager.instance.KonamiCode == true)
        {
            gameObject.GetComponent<PlayerController>().regainStaminaSpeed = 200;
        }
    }
}
