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

        Loop,
        NoLoop

    }
    public SoundEventController[] SoundEventsInScene;
    private Dictionary<(EventReference, GameObject), EventInstance> eventInstances;
    private List<EventInstance> eventInstanceList;
    private SoundSettings settings;
    private VCA SFX_VCA, Ambiance_VCA, UI_VCA, VO_VCA;

    public void LocateSoundEvents()
    {

        GameObject[] _found = GameObject.FindGameObjectsWithTag("SoundEvent");
        SoundEventsInScene = new SoundEventController[_found.Length];

        for(int i = 0; i < _found.Length; i++)
        {

            SoundEventsInScene[i] = _found[i].GetComponent<SoundEventController>();

        }

    }

    private void Awake()
    {

        Instance = this;

    }

    private void start()
    {

        eventInstances = new Dictionary<(EventReference, GameObject), EventInstance>();
        eventInstanceList = new List<EventInstance>();

        settings = SoundSettingsManager.LoadVolumeSettings();
        if(settings == null)
            settings = SoundSettingsManager.SaveVolumeSettings();

        //SFX_VCA = RuntimeManager.GetBus("")

    }

    private void OnDestroy()
    {

        foreach (EventInstance _ei in eventInstanceList)
        {

            _ei.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            _ei.release();
            
        }

    }

    /*public bool IsEventPlaying(EventReference _soundEvent)
    {

        if(eventInstances.ContainsKey(_soundEvent))
            if(eventInstances[_soundEvent].getPlaybackState(out PLAYBACK_STATE _pbState))
        else
            return false;

    }*/

    public void StopSound(EventReference _soundEvent, GameObject _go)
    {

        /*if(eventInstances.ContainsKey((_soundEvent, _go)))
        {

            eventInstances[(_soundEvent, _go)].stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

        }
        else
        {

            Debug.Log("This sound has never been played");

        }*/

    }

    public void PlaySound(EventReference _soundEvent, GameObject _go, SoundType _type = SoundType.NoLoop)
    {

        /*EventInstance _tempEvent;
        PLAYBACK_STATE _pbState;

        if(eventInstances.ContainsKey((_soundEvent, _go)))
        {

            _tempEvent = eventInstances[(_soundEvent, _go)];

        }
        else
        {

            _tempEvent = RuntimeManager.CreateInstance(_soundEvent);

            eventInstances.Add((_soundEvent, _go), _tempEvent);
            eventInstanceList.Add(_tempEvent);

        }

        _tempEvent.getPlaybackState(out _pbState);
        
        if(_type == SoundType.Loop)
        {

            if(_pbState != PLAYBACK_STATE.PLAYING)
            {

                _tempEvent.start();

            }
            
        }
        else
        {

            _tempEvent.start();

        }

        switch (_type)
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
