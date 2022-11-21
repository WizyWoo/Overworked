using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;


#if UNITY_EDITOR
public class Dialogue_tool : EditorWindow
{

    #region Variables
    Boss_script_variables B;

    public int tab_number = 0;
    public int Character_int = 0;
    public int dialogue_int;


    public string[] tab_names = new string[] { "characters", "options", "dialogue" };
    public string[] Character_names = new string[] {"boss", "dogs"};
    public string[] Tool_dropdown = new string[] {"thing", "thing2", "thing3"};
    public  List<test> T = new List<test>();

    private GUIContent Content_stuff;
    public Vector2 scroll;

    

    // public List<Boss_speech> dialogue = new List<Boss_speech>();
    
    #endregion

    private void OnEnable()
    {
        

    }
    
    

    [MenuItem("Window/Dialogue")]
    public static void ShowWindow()
    {
           
        GetWindow<Dialogue_tool>("Dialogue options");

    }



    private void OnGUI()
    {
        get_boss_dialogue_script();
        Tool_tab();
        display_selected_tab();



        void get_boss_dialogue_script()
        {
            B = GameObject.FindGameObjectWithTag("Dialogue").GetComponent<Boss_script_variables>();
        }
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
        Object a_thing = null;
        string sentence = "";

       scroll = EditorGUILayout.BeginScrollView(scroll);

        display_dialogue();
        add_sentence();
        add_more_dialogue();
        clear_dialogues();

        Space(20);
        

        if(GUILayout.Button("save"))
            EditorUtility.SetDirty(B);


        void display_dialogue()
        {
            string[] enum_array;
            string st = "";
           
            

            enum_array = System.Enum.GetNames(typeof(dog_animation));

            display_all_sentences();
           
            for (int a = 0; a < B.get_count(); a++)
            {
                int length = B.get_sentences_count(a);

                GUILayout.Label("dialogue " + a);

                for (int i = 0; i < length; i++)
                {
                    set_string(i);

                    expression anim = B.get_expressions(a,i);
                    anim = (expression)EditorGUILayout.EnumPopup(anim);
                    B.set_expression(a, i, anim);

                    dog_animation da = B.get_animation(a, i);
                    da = (dog_animation)EditorGUILayout.EnumPopup(da);
                    B.set_animation(a, i, da);
                }

                if (GUILayout.Button("add sentence"))
                {
                    B.add_sentence(a);
                    
                }

                Space(10);
                void set_string(int i)
                {
                    st = EditorGUILayout.TextArea(B.get_sentence(a, i), GUILayout.Height(50));
                    B.set_sentence(a, i, st);
                }
            }

            
      

            void show_expression_option()
            {

            }
                
            void enum_animation()
            {

            }
            void display_all_sentences()
            {

            }

            Space(10);

        }

        void add_sentence()
        {
            if (GUILayout.Button("add dialogue"))
            {
                B.add_itnem();
            }
        }
       
        void add_more_dialogue()
        {

        }

        void clear_dialogues()
        {
            if (GUILayout.Button("clear"))
                B.Clear_list();
        }

        EditorGUILayout.EndScrollView();
      
    }
   

    void Space(int i)
    {
        GUILayout.Space(i);
    }
    

}



#endif

public class test
{
    public string   N;
    public int      I;
    public string[] S;

}



