using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Dialogue_tool : EditorWindow
{
    public string[] tab_names = new string[] { "characters", "options", "dialogue" };
    public int tab_number = 0;



    [MenuItem("Window/Dialogue")]
    public static void ShowWindow()
    {
        GetWindow<Dialogue_tool>("Dialogue options");
    }
    private void OnGUI()
    {
        tab_number = GUILayout.Toolbar(tab_number, tab_names);

        switch (tab_number)
        {
            case 0: character(); break;
            case 1: options(); break;

        }
    }

    public void character()
    {
        GUILayout.Label("characters");


    }

    public void options()
    {
        GUILayout.Label("options");
    }

}
