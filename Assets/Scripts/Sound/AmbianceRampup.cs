using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AmbianceRampup : MonoBehaviour
{

    public StudioEventEmitter Emitter;
    public string ParamName;
    public float LevelLenght;
    [SerializeField]
    private float timer;

    private void Update()
    {

        timer += Time.deltaTime;
        Emitter.SetParameter(ParamName, timer / LevelLenght);

    }

}
