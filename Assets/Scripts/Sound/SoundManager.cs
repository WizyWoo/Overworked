using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

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

            if(GUILayout.Button("Locate all SoundEvents"))
            {

                _sM.LocateSoundEvents();

            }

        }

    }
    
}

#endif

[ExecuteAlways]
public class SoundManager : MonoBehaviour
{

    public static SoundManager Instance;
    public enum SoundType
    {

        Ambiance,
        Music,
        SFX,
        UI

    }
    public SoundEventController[] SoundEventsInScene;

    public SoundEventController SEC_Ambiance;

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

        if(!Instance)
            Instance = this;
        else
            Destroy(this);

    }

    public void PlaySound(EventReference _soundEvent, SoundType _type)
    {

        switch (_type)
        {
            
            case SoundType.Ambiance:
            SEC_Ambiance.EventReference = _soundEvent;
            SEC_Ambiance.Play();
            break;

        }
        
    }

}
