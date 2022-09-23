using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

[ExecuteAlways]
public class SoundEventController : StudioEventEmitter
{

    private SoundManager soundManager;

    private void Awake()
    {

        gameObject.tag = "SoundEvent";

    }

}
