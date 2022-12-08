using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lvl9Unlock : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.UnlockEverything = true;
    }

    // Update is called once per frame
    void Update()
    {
        GameManager.instance.UnlockEverything = true;
    }
}
