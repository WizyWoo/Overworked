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
    
    
    



    public string[] tab_names = new string[] { "characters", "options", "dialogue" };
    public int tab_number = 0;

    public int Character_int = 0;
    public string[] Character_names = new string[] {"boss", "dogs"};

    public string[] Tool_dropdown = new string[] {"thing", "thing2", "thing3"};
    
   // public string Tool_dropdown = "hello";

    private GUIContent Content_stuff;

    public  List<test> T = new List<test>();
    public int dialogue_int;
    // public List<Boss_speech> dialogue = new List<Boss_speech>();
    

    private void OnEnable()
    {


    }

    
    #endregion

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
            B = GameObject.Find("ok").GetComponent<Boss_script_variables>();
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




        //Scroll_position = EditorGUILayout.BeginScrollView(Scroll_position,false,true);
       

        display_dialogue();
        add_sentence();
        add_more_dialogue();
        clear_dialogues();




        Space(20);
        

       //EditorGUILayout.EndScrollView();


        void display_dialogue()
        {
            string[] enum_array;
            string st = "";
            int d = B.get_count();
           
            

            enum_array = System.Enum.GetNames(typeof(dog_animation));

            display_all_sentences();
           
            for (int a = 0; a < B.get_count(); a++)
            {
                int length = B.get_sentences_count(a);

                GUILayout.Label("dialogue" + a);

                for (int i = 0; i < length; i++)
                {
                    set_string(st,i);
                    B.set_sentence(a, i, st);
                    show_animation_options();
                }

                if (GUILayout.Button("add sentence"))
                {
                    B.add_sentence(a);
                }

                Space(10);
                void set_string(string st,int i)
                {
                    st = EditorGUILayout.TextArea(B.get_sentence(a, i), GUILayout.Height(50));

                }
            }

            

            void show_animation_options()
            {
                dog_animation anim = dog_animation.panic;
                anim = (dog_animation)EditorGUILayout.EnumPopup(anim);
                
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
              
               // hasUnsavedChanges = true;
            }
        }
       


        void boss_sprite_change()
        {
            Object sp = null;
            sp = EditorGUILayout.ObjectField(sp, typeof(Sprite));
        }
        void animation_insertion()
        {
            a_thing = EditorGUILayout.ObjectField(a_thing,typeof(Animation));
        }
        void add_more_dialogue()
        {

        }

        void clear_dialogues()
        {
            if (GUILayout.Button("clear"))
                B.Clear_list();
        }

      
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

public class Boss_speech
{
    public List<string> S = new List<string>();
}

public class Personality
{
    Animation A;
}

