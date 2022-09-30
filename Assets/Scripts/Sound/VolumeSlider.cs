using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class VolumeSlider : MonoBehaviour
{

    public string VCAPath = "vca:/Master";
    private VCA vca;
    private SoundSettings settings;

    private void Start()
    {

        vca = RuntimeManager.GetVCA(VCAPath);

    }

    public void VolumeSliderChange(float _vol)
    {

        vca.setVolume(_vol);

    }

}
