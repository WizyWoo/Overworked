using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class Conversation_s : MonoBehaviour
{
    public TMP_Text Speech_text;
    public Transform Boss_transfrom;

    [SerializeField]
    private Sentence[] speeches;
    public PlayerInput convo;
    public Animator Black_screen;   
    public int day;
    public int i;


    private void Awake()
    {
        PlayerPrefs.SetInt("Day", 2);
        convo = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        wait_after_load();
        Black_screen.Play("fade_in");
        day = PlayerPrefs.GetInt("Day");

    }


    void Update()
    {
        
    }

    IEnumerator wait_after_load()
    {
        yield return new WaitForSeconds(2);

    }

    public void Continue_thing(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            
            if (i < speeches[day].Conversation.Length)
                Speech_text.text = speeches[day].Conversation[i];
                increase();

            if (i > speeches[day].Conversation.Length)
            {
                Black_screen.Play("fade_out");
                StartCoroutine(Load_level());
            }
        }
    }
    
    IEnumerator Load_level()
    {
        
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(3);  
    }

    public void increase()
    {
        i++;
    }
}

