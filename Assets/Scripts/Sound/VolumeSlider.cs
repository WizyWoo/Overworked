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

            GUILayout.Label("Click me :)");
            if(GUILayout.Button("Press plz"))
            {

                Debug.Log("I am D pressed");

            }

        }

    }
    
}

#endif

public class VolumeSlider : MonoBehaviour
{

    public string BusPath = "vca:/Master";
    public Slider VolSlider;
    private Bus bus;
    private SoundSettings settings;

    private void Start()
    {

        bus = RuntimeManager.GetBus(BusPath);
        bus.getVolume(out float _vol);
        VolSlider.value = _vol;

    }

    public void VolumeSliderChange(float _vol)
    {

        bus.setVolume(_vol);

    }

}
