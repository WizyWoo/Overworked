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

            GUILayout.Label("Click me :)");
            if(GUILayout.Button("Press plz"))
            {

                Debug.Log("why you press me :(");

            }

        }

    }
    
}

#endif

public class SoundManager : MonoBehaviour
{

    public static SoundManager Instance;
    public enum SoundType
    {

        Loop,
        NoLoop

    }
    private Dictionary<(EventReference, GameObject), EventInstance> eventInstances;
    private List<EventInstance> eventInstanceList;
    private SoundSettings settings;
    private VCA SFX_VCA, Ambiance_VCA, UI_VCA, MUSIC_VCA, Master_VCA;

    private void Awake()
    {

        Instance = this;

    }

    private void Start()
    {

        eventInstances = new Dictionary<(EventReference, GameObject), EventInstance>();
        eventInstanceList = new List<EventInstance>();

        settings = SoundSettingsManager.LoadVolumeSettings();
        if(settings == null)
            settings = SoundSettingsManager.SaveVolumeSettings();

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

        if(eventInstances.ContainsKey((_soundEvent, _go)))
        {

            eventInstances[(_soundEvent, _go)].stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

        }

    }

    public void PlaySound(EventReference _soundEvent, GameObject _go, SoundType _type = SoundType.NoLoop)
    {

        EventInstance _tempEvent;
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
        
    }

}
