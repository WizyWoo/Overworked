using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Boss_script_variables : MonoBehaviour
{
    [HideInInspector]
    public List<Boss_speech> dialogue = new List<Boss_speech>();
    public List<Sprite> Boss_expressions = new List<Sprite>();

    public Dictionary<expression, Sprite> expres = new Dictionary<expression, Sprite>();
    public Dictionary<dog_animation, Animation> D_a = new Dictionary<dog_animation, Animation>();

    public SpriteRenderer boss;
    public Animator Dogs;


    private void OnEnable()
    {
        boss = GameObject.Find("boss").GetComponent<SpriteRenderer>();
        Dogs = GameObject.Find("dogs").GetComponent<Animator>();

        expres.Add(expression.angry , Boss_expressions[0]);
        expres.Add(expression.sad   , Boss_expressions[1]);
        expres.Add(expression.normal, Boss_expressions[2]);
    }



    #region Dialogue
    public List<Boss_speech> get_Dialogue()
    {
        return dialogue;
    }
    public void add_itnem()
    {

        dialogue.Add(new Boss_speech());
    }
    public void set_sentence(int a,int b, string s)
    {
        dialogue[a].S[b] = s;
    }
    public string get_sentence(int a,int b)
    {
        return dialogue[a].S[b];
    }

    public void Clear_list()
    {
        dialogue.Clear();
    }

    public int get_count()
    {
        return dialogue.Count;
    }
    public int get_sentences_count(int i)
    {
        return dialogue[i].S.Count;
    }
    public void add_sentence(int i)
    {
        dialogue[i].S.Add("");
    }
    #endregion

    #region Boss emotions

    public void set_sprite(expression s)
    {
        boss.sprite = expres[s];
    }

    public void angry()
    {
        boss.sprite = Boss_expressions[1];
    }

    public void set_dog_animation()
    {

    }


    #endregion

}

[System.Serializable]
public class Boss_speech
{
    public List<string> S = new List<string>();
}

public class Boss_expressions
{
    
}

public enum expression
{
    normal,
    sad,
    angry
}
public enum dog_animation
{
    normal,
    worried,
    panic
}
