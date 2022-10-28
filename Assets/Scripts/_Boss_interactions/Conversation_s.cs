using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class Conversation_s : MonoBehaviour
{
    #region Variables
    public TMP_Text          Speech_text;
    public Transform         Boss_transfrom;
    

    [SerializeField]
    private Sentence[]       speeches;
   
    public  AnimationCurve   bob_up_and_down;
    public  PlayerInput      convo;
    public  Animator         Black_screen;   
    public  int              day;
    
    private float            t = 0;
    private int              i = 0;
    #endregion


    #region Methods
    private void Awake()
    {
        PlayerPrefs.SetInt("Day", 2);
        convo = GetComponent<PlayerInput>();
    }
    private void Start()
    {
        Set_text();
        increase();

        wait();
        Fade_in();

        void increase()
        {
            i++;
        }
        void Set_text()
        {
            Speech_text.text = speeches[day].Conversation[i];
        }
        void Fade_in()
        {
            Black_screen.Play("fade_in");
        }
    }
    void Update()
    {
        Boss_bob();
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
            if (i < speeches[day].Conversation.Length)
                Set_text();
        }
        void Continue_to_scene()
        {
            if (i > speeches[day].Conversation.Length)
            {
                Fade_out();
                Load_level();
            }
        }

        void increase()
        {
            i++;
        }
        void Set_text()
        {
            Speech_text.text = speeches[day].Conversation[i];
        }
        void Fade_out()
        {
            Black_screen.Play("fade_out");
        }
    }
    public void Load_level()
    {
        wait();
        SceneManager.LoadScene(3);  
    }
    #endregion


    #region IEnumerator
    IEnumerator wait()
    {
        yield return new WaitForSeconds(2);
    }
    #endregion
}

