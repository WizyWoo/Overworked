using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager Main;



    public List<SoundEventController> SoundEventsInScene;

    [ExecuteAlways]
    private void Awake()
    {

        if(!Main)
            Main = this;
        else
            Destroy(this);

    }

}
