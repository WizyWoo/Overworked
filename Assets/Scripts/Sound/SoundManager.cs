using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public static SoundManager Main;
    public SoundEventController[] SoundEventsInScene;

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

        if(!Main)
            Main = this;
        else
            Destroy(this);

    }

}
