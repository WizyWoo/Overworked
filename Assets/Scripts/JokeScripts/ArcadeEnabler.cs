using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcadeEnabler : MonoBehaviour
{ public GameObject Arcade;
    // Start is called before the first frame update
    void Start()
    {
        Arcade.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.ArcadeModeApp == true)
            Arcade.SetActive(true);
    }
}
