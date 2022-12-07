using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMOD.Studio;
using FMODUnity;

#if UNITY_EDITOR

namespace VolumeSliderCustomEditor
{

    using UnityEditor;

    [CustomEditor(typeof(VolumeSlider))]
    public class SoundManagerEditor : Editor
    {

        public override void OnInspectorGUI()
        {

            DrawDefaultInspector();

            SoundManager _sM = (SoundManager)target;
        }

    }
    
}

#endif

public class VolumeSlider : MonoBehaviour
{

    public string BusPath = "vca:/Master";
    private Slider VolSlider;
    private Bus bus;
    private SoundSettings settings;

    private void Start()
    {
        VolSlider = GetComponentInChildren<Slider>();
        bus = RuntimeManager.GetBus(BusPath);
        bus.getVolume(out float _vol);
        VolSlider.value = _vol;

    }

    public void VolumeSliderChange(float _vol)
    {

        bus.setVolume(_vol);

    }

}
