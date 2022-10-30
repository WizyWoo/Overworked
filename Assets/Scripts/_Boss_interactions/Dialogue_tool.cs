using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Dialogue_tool : EditorWindow
{
    [MenuItem("Window/Dialogue")]
    public static void ShowWindow()
    {
        GetWindow<Dialogue_tool>("Dialogue options");
    }
    private void OnGUI()
    {
        
    }

}
