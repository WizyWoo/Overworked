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
    [SerializeField]
    private string sfx_BusPath, ambx_BusPath, mus_BusPath, vo_BusPath, master_BusPath;
    private Bus sfx_Bus, ambx_Bus, mus_Bus, vo_Bus, master_Bus;

    private int settingsVer = 0;

    private void Awake()
    {

        Instance = this;

        SettingsVer _settingsVer = SoundSettingsManager.LoadSettingsVersion(settingsVer);
        if(_settingsVer.Version != settingsVer)
        {

            SoundSettingsManager.SaveSettingsVersion(settingsVer);
            SoundSettingsManager.SaveVolumeSettings();

        }


        settings = SoundSettingsManager.LoadVolumeSettings();
        if(settings == null)
            settings = SoundSettingsManager.SaveVolumeSettings();

        ambx_Bus = RuntimeManager.GetBus(ambx_BusPath);
        sfx_Bus = RuntimeManager.GetBus(sfx_BusPath);
        mus_Bus = RuntimeManager.GetBus(mus_BusPath);
        vo_Bus = RuntimeManager.GetBus(vo_BusPath);
        master_Bus = RuntimeManager.GetBus(master_BusPath);

        ApplyVolumeSettings();

    }

    private void Start()
    {

        eventInstances = new Dictionary<(EventReference, GameObject), EventInstance>();
        eventInstanceList = new List<EventInstance>();

    }

    private void OnDestroy()
    {

        foreach (EventInstance _ei in eventInstanceList)
        {

            _ei.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            _ei.release();
            
        }

    }

    public void SaveVolumeSettings()
    {

        float _vol;
        ambx_Bus.getVolume(out _vol);
        settings.AmbianceVolume = _vol;
        sfx_Bus.getVolume(out _vol);
        settings.SFXVolume = _vol;
        mus_Bus.getVolume(out _vol);
        settings.MusicVolume = _vol;
        vo_Bus.getVolume(out _vol);
        settings.UIVolume = _vol;
        master_Bus.getVolume(out _vol);
        settings.MasterVolume = _vol;

        SoundSettingsManager.SaveVolumeSettings(settings);

    }

    public void ApplyVolumeSettings()
    {

        ambx_Bus.setVolume(settings.AmbianceVolume);
        sfx_Bus.setVolume(settings.SFXVolume);
        mus_Bus.setVolume(settings.MusicVolume);
        vo_Bus.setVolume(settings.UIVolume);
        master_Bus.setVolume(settings.MasterVolume);

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
        
        if (eventInstances.ContainsKey((_soundEvent, _go)))
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

        _tempEvent.set3DAttributes(RuntimeUtils.To3DAttributes(_go));
        
    }

}
