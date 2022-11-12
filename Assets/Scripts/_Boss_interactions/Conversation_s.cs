using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class Conversation_s : MonoBehaviour
{
    #region Variables
    public Boss_script_variables B;
    public TMP_Text          Speech_text;
    public Transform         Boss_transfrom;
    
    [SerializeField]
    private Sentence[]       speeches;
    public Sprite            Boss_sprite;


    public  AnimationCurve   bob_up_and_down;
    public  PlayerInput      convo;
    public  Animator         Black_screen;   
    
    private float            t = 0;
    private int              i = 0;
    private int              r;
    #endregion


    #region Methods
    private void Awake()
    {
        PlayerPrefs.SetInt("Day", 2);
        convo = GetComponent<PlayerInput>();

        r = Random.Range(0, B.dialogue.Count);
        Debug.Log(B.dialogue.Count);
        Debug.Log(r);
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
        Boss_bob();
        Debug.Log(B.expres.ContainsKey(expression.normal));

        Debug.Log(Random.Range(0, 3));
    }

    
    public void Boss_bob()
    {
        Bob_time();
        Boss_transfrom.position = new Vector3(Boss_transfrom.position.x, bob_up_and_down.Evaluate(t) + 1, 6);

        void Bob_time()
        {
            if (t >= 1)
                t = 0;

            t += Time.deltaTime;
        }
    }
    public void Continue_thing(InputAction.CallbackContext context) 
    {
        if (context.started)
        {
            Continue_conversation();
            Continue_to_scene();

            increase();
        }

        void Continue_conversation()
        {
            if (i < B.dialogue[r].S.Count)
            {
                Set_text();
                set_boss_sprite();
                set_dog_animation();
                Debug.Log(i);
            }
        }
        void Continue_to_scene()
        {
            if (i > B.dialogue[r].S.Count - 1)
            {
                Debug.Log(i);
                Fade_out();
                Load_level();
            }
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
        B.boss.sprite = B.expres[B.get_expressions(r, i)];
    }
    public void set_dog_animation()
    {
        B.Dogs[0].Play(B.D_a[B.dialogue[r].da[i]]);
        B.Dogs[1].Play(B.D_a[B.dialogue[r].da[i]]);
    }
    
    public void Load_level()
    {
        wait();
        GameManager.instance.LoadScene("MainMenu");  
    }   
    #endregion


    #region IEnumerator
    IEnumerator wait()
    {
        yield return new WaitForSeconds(2);
    }
    #endregion
}

