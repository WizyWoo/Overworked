using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

#if UNITY_EDITOR

namespace SoundManagerCustomEditor
{

    using UnityEditor;

    [CustomEditor(typeof(SoundManager))]
    public class SoundManagerEditor : Editor
    {

        public override void OnInspectorGUI()
        {

            DrawDefaultInspector();

            SoundManager _sM = (SoundManager)target;

            GUILayout.Label("Use this to see if there are any SoundEventControllers in the scene");
            if(GUILayout.Button("Locate all SoundEvents"))
            {

                _sM.LocateSoundEvents();

            }

        }

    }
    
}

#endif

public class SoundManager : MonoBehaviour
{

    //FMODUnity.RuntimeManager.GetVCA("vca:/Master")

    public static SoundManager Instance;
    public enum SoundType
    {

        Ambiance,
        Music,
        SFX,
        UI

    }
    public SoundEventController[] SoundEventsInScene;
    //public SoundEventController SEC_Ambiance, SEC_Music, SEC_SFX, SEC_UI;
    private Dictionary<EventReference, EventInstance> eventInstances;
    private SoundSettings settings;

    public void LocateSoundEvents()
    {

        GameObject[] _found = GameObject.FindGameObjectsWithTag("SoundEvent");
        SoundEventsInScene = new SoundEventController[_found.Length];

        for(int i = 0; i < _found.Length; i++)
        {

            SoundEventsInScene[i] = _found[i].GetComponent<SoundEventController>();

        }

        /*foreach (SoundEventController _sec in SoundEventsInScene)
        {

            if(_sec.gameObject.name.Contains("Ambiance"))
                SEC_Ambiance = _sec;
            else if(_sec.gameObject.name.Contains("Music"))
                SEC_Music = _sec;
            else if(_sec.gameObject.name.Contains("SFX"))
                SEC_SFX = _sec;
            else if(_sec.gameObject.name.Contains("UI"))
                SEC_UI = _sec;
            
        }*/

    }

    private void Awake()
    {

        if(!Instance)
            Instance = this;
        else
            Destroy(this);

        DontDestroyOnLoad(this);

        LocateSoundEvents();

        eventInstances = new Dictionary<EventReference, EventInstance>();

        settings = SoundSettingsManager.LoadVolumeSettings();
        if(settings == null)
            settings = SoundSettingsManager.SaveVolumeSettings();

    }

    public void ApplyVolumeSettings()
    {

        

    }

    private void AllocateInstance(EventReference _soundEvent)
    {



    }

    public void PlaySound(EventReference _soundEvent, SoundType _type)
    {

        EventInstance _tempEvent;

        if(eventInstances.ContainsKey(_soundEvent))
        {

            eventInstances[_soundEvent].start();

        }
        else
        {

            _tempEvent = RuntimeManager.CreateInstance(_soundEvent);

            eventInstances.Add(_soundEvent, _tempEvent);
            _tempEvent.start();

        }

        /*switch (_type)
        {
            
            case SoundType.Ambiance:
            if(!_soundEvent.IsNull)
            {
                SEC_Ambiance.EventReference = _soundEvent;
                SEC_Ambiance.Play();
            }
            else if(!SEC_Ambiance.EventReference.IsNull)
                SEC_Ambiance.Play();
            break;

            case SoundType.Music:
            if(!_soundEvent.IsNull)
            {
                SEC_Music.EventReference = _soundEvent;
                SEC_Music.Play();
            }
            else if(!SEC_Music.EventReference.IsNull)
                SEC_Music.Play();
            break;

            case SoundType.SFX:
            if(!_soundEvent.IsNull)
            {
                SEC_SFX.EventReference = _soundEvent;
                SEC_SFX.Play();
            }
            else if(!SEC_SFX.EventReference.IsNull)
                SEC_SFX.Play();
            break;

            case SoundType.UI:
            if(!_soundEvent.IsNull)
            {
                SEC_UI.EventReference = _soundEvent;
                SEC_UI.Play();
            }
            else if(!SEC_UI.EventReference.IsNull)
                SEC_UI.Play();
            break;

        }*/
        
    }

}
