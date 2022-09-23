using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class SoundEventController : StudioEventEmitter
{

    private SoundManager soundManager;


    private void pepp()
    {

        if(!soundManager)
            soundManager = SoundManager.Main;
        
        soundManager.SoundEventsInScene.Add(this);

    }

}
