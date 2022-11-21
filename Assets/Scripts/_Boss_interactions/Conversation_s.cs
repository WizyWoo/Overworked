using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class Conversation_s : MonoBehaviour
{
    #region Variables
    /// <summary> Boss_script_variable</summary>
    private Boss_script_variables B;
   /// <summary> TMP_Text </summary>
    public TMP_Text           Speech_text;

    
    private Transform         Boss_transfrom;
    private SpriteRenderer    Boss_sprite;


    public  AnimationCurve   bob_up_and_down;
    public  PlayerInput      convo;
    public  Animator         Black_screen;   
    
    private float            t = 0;
    private int              i = 0;
    private int              r;

    private bool             boss_is_here;
    #endregion


    #region Methods
    private void Awake()
    {

        B = GetComponent<Boss_script_variables>();

        Set_boss_variables();
        get_dialogue_variables();

        convo = GetComponent<PlayerInput>();

        
        void Set_boss_variables()
        { 
            GameObject Boss;

            if (Boss_is_in_the_scene())
            {
                Boss            = Boss_object();
                Boss_sprite     = Boss.GetComponent<SpriteRenderer>();
                Boss_transfrom  = Boss.transform;
                boss_is_here    = true;

            }
            else boss_is_here = false;
        }

        GameObject Boss_object()
        {
            return GameObject.Find("boss").transform.GetChild(0).gameObject;
        }
        

        void get_dialogue_variables()
        {
            r = Random.Range(0, B.dialogue.Count);
            Debug.Log(B.dialogue.Count);
            Debug.Log(r);
        }

        bool Boss_is_in_the_scene()
        {
            return GameObject.Find("boss") != null;
        }
    }
    private void Start()
    {

        Set_text();
        set_boss_sprite();
        set_dog_animation();

        increase();

        
        Fade_in();
        
        void Fade_in()
        {
            Black_screen.Play("fade_in");
        }
        
    }
    void Update()
    {
        if(boss_is_here)
            Boss_bob();

        
        void Boss_bob()
        {
            Bob_time(ref t);
            Boss_transfrom.position = bobing();

            Vector3 bobing()
            {
                return new Vector3(x(), y(), 6);
            }
            float x()
            {
                return Boss_transfrom.position.x;
            }
            float y()
            {
                return bob_up_and_down.Evaluate(t) + 1;
            }
    
        }
        
        void Bob_time(ref float t)
        {
            if (t >= bob_up_and_down.keys[2].time)
                t = 0;

                t += Time.deltaTime;        
        }
    }

    public void Continue_thing(InputAction.CallbackContext context) 
    {
        if (context.started)
        {
            if (I_is_bellow_sentence_count())
            {
                Debug.Log(B.dialogue[r].S.Count);
                Set_text();
                set_boss_sprite();
                set_dog_animation();
                Debug.Log(i);
            }

            if (I_is_above_sentence_count())
            {
                Fade_out();
                StartCoroutine(wait());
                
            }
            
            increase();
        }


        

        bool I_is_bellow_sentence_count()
        {
            return i < B.dialogue[r].S.Count;
        }
        bool I_is_above_sentence_count()
        {
            return i > B.dialogue[r].S.Count - 1;
        }
        void Fade_out()
        {
            Black_screen.Play("fade_out");
        }
        

    }


    public void increase()
    {
        i++;
    }

    public void Set_text()
    {
        

        Speech_text.text = B.dialogue[r].S[i];
    }

    public void set_boss_sprite()
    {
        if (boss_is_here)
            Boss_sprite.sprite = B.expres[B.get_expressions(r, i)];
    }

    public void set_dog_animation()
    {
        B.Dogs[0].Play(B.D_a[B.dialogue[r].da[i]]);
        B.Dogs[1].Play(B.D_a[B.dialogue[r].da[i]]);
    }
    
    public void Load_level()
    {
        GameManager.instance.LoadScene("MainMenu");  
    }   
    #endregion


    #region IEnumerator
    IEnumerator wait()
    {
        yield return new WaitForSeconds(2);
        Load_level();
    }
    #endregion
}

