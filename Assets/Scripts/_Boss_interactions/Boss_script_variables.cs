using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Boss_script_variables : MonoBehaviour
{
    [HideInInspector]
    public List<Boss_speech> dialogue       = new List<Boss_speech>();
    public List<Sprite> Boss_expressions    = new List<Sprite>();
    public List<Animation> dog_anim         = new List<Animation>();

    
    public Dictionary<expression, Sprite> expres    = new Dictionary<expression, Sprite>();
    public Dictionary<dog_animation, string> D_a    = new Dictionary<dog_animation, string>();

    public SpriteRenderer boss;
    public Animator Dogs;


    private void OnEnable()
    {
        boss = GameObject.Find("boss").GetComponent<SpriteRenderer>();
        Dogs = GameObject.Find("dogs").GetComponent<Animator>();

        expres.Add(expression.angry , Boss_expressions[0]);
        expres.Add(expression.sad   , Boss_expressions[1]);
        expres.Add(expression.normal, Boss_expressions[2]);

        D_a.Add(dog_animation.normal    , dog_anim[0].ToString());
        D_a.Add(dog_animation.panic     , dog_anim[1].ToString());
        D_a.Add(dog_animation.worried   , dog_anim[2].ToString());
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

    public void set_dog_animation(dog_animation d)
    {
        Dogs.Play(D_a[d]);
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
