using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


#if UNITY_EDITOR
public class Dialogue_tool : EditorWindow
{
            
    #region Variables
    public string[] tab_names = new string[] { "characters", "options", "dialogue" };
    public int tab_number = 0;

    public int Character_int = 0;
    public string[] Character_names = new string[] {"boss", "dogs"};

    public string[] Tool_dropdown = new string[] {"thing", "thing2", "thing3"};
   // public string Tool_dropdown = "hello";

    public GUIContent Content_stuff;

    public List<test> T = new List<test>();
    List<Boss_speech> dialogue = new List<Boss_speech>();
    #endregion

    [MenuItem("Window/Dialogue")]
    public static void ShowWindow()
    {
        
        GetWindow<Dialogue_tool>("Dialogue options");

    }



    private void OnGUI()
    {
        Tool_tab();
        display_selected_tab();
       

        void Tool_tab()
        {
            tab_number = GUILayout.Toolbar(tab_number, tab_names);
        }
        void display_selected_tab()
        {
            switch (tab_number)
            {
                case 0: character();    break;
                case 1: options ();     break;
                case 2: dialogue_m();   break;
            }   
        }
    }



    public void character()
    {
       GUILayout.Label("characters");
       Character_int = GUILayout.Toolbar(Character_int, Character_names);

    }
    

    public void options()
    {
        test thing = new test();


        GUILayout.Label("options");
        display_options();

        add_option_button();
        remove_option_button();


        void display_options()
        {

            foreach (test Test in T)
            {
                GUILayout.Label("option");

            }
        }
        void add_option_button()
        {
            if (GUILayout.Button("add option"))
                T.Add(thing);
        }
        void remove_option_button()
        {
            if (GUILayout.Button("remove option"))
                T.Remove(T[T.Count - 1]);
        }

    }

        Vector2 Scroll_position = new Vector2(0,0);
    public void dialogue_m()
    {
        
        int get_length_of_dialogue = dialogue.Count;


        Object a_thing = null;
        string sentence = "";

        int pop = 0;


        Scroll_position = EditorGUILayout.BeginScrollView(Scroll_position,false,true, GUILayout.Height(200));

        display_dialogue();
        add_sentence();
        add_more_dialogue();


        display_writing_field();

        GUILayout.Space(20);
        

        EditorGUILayout.EndScrollView();

        void animation_insertion()
        {
            a_thing = EditorGUILayout.ObjectField(a_thing,typeof(Animation));
        }

        void boss_sprite_change()
        {
            Object sp = null;
            sp = EditorGUILayout.ObjectField(sp, typeof(Sprite));
        }
      

        void display_dialogue()
        {
            for (int a = 0; a < dialogue.Count; a++)
            {
                int length = dialogue[a].S.Count;
                string st = "";

                GUILayout.Label("dialogue" + a);

                for (int i = 0; i < length; i++)
                {
                    dialogue[a].S[i] = EditorGUILayout.TextArea(dialogue[a].S[i], GUILayout.Height(50));
                    boss_sprite_change();
                    animation_insertion();
                }

                if (GUILayout.Button("add sentence"))
                    dialogue[a].S.Add(st);
                
                GUILayout.Space(10);
            }
            Debug.Log(dialogue.Count);

        }


        void add_sentence()
        {
            Boss_speech b = new Boss_speech();
            
            if (GUILayout.Button("add dialogue"))
            {
                dialogue.Add(b);
                Debug.Log("pressed");
            }
        }
        void add_more_dialogue()
        {

        }

        void display_writing_field()
        {
            sentence = EditorGUILayout.TextArea(sentence, GUILayout.MaxHeight(50));
        }
        void select_expression()
        {
            pop = EditorGUILayout.Popup(pop, Tool_dropdown);
        }
    }
}
#endif

public class test
{
    public string   N;
    public int      I;
    public string[] S;

}

public class Boss_speech
{
    public List<string> S = new List<string>();
}

public class Personality
{
    Animation A;
}

