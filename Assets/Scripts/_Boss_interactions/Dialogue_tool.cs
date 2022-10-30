using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


#if UNITY_EDITOR
public class Dialogue_tool : EditorWindow
{
    public string[] tab_names = new string[] { "characters", "options", "dialogue" };
    public int tab_number = 0;

    public int Character_int;
    public string[] Character_names;



    [MenuItem("Window/Dialogue")]
    public static void ShowWindow()
    {
        GetWindow<Dialogue_tool>("Dialogue options");
    }
    private void OnGUI()
    {
        Tab();

        switch (tab_number)
        {
            case 0: character(); break;
            case 1: options(); break;

        }
    }
    public void Tab()
    {
        tab_number = GUILayout.Toolbar(tab_number, tab_names);
    }


    public void character()
    {
        GUILayout.Label("characters");


        GUILayout.Toolbar(Character_int, Character_names);

    }

    public void options()
    {
        GUILayout.Label("options");
    }
#endif
}
