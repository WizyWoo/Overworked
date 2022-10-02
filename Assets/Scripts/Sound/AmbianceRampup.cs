using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AmbianceRampup : MonoBehaviour
{

    public StudioEventEmitter Emitter;
    public float LevelLenght;
    private float timer;

    private void Update()
    {

        timer += Time.deltaTime;
        Emitter.SetParameter("Intensity", timer / LevelLenght);

    }

}
