using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Boss_script_variables : MonoBehaviour
{
    public List<Boss_speech> dialogue       = new List<Boss_speech>();
    public List<Sprite> Boss_expressions    = new List<Sprite>();
    public List<Animation> dog_anim         = new List<Animation>();
 


    public Dictionary<expression, Sprite> expres = new Dictionary<expression, Sprite>();
    public Dictionary<dog_animation, string> D_a    = new Dictionary<dog_animation, string>();

    public SpriteRenderer boss;
    public Animator[] Dogs;

  
    private void OnEnable()
    {
       // boss = GameObject.Find("boss").GetComponent<SpriteRenderer>();
       // Dogs = GameObject.Find("dogs").GetComponent<Animator>();

        
        D_a.Add(dog_animation.normal    , "normal");
        D_a.Add(dog_animation.panic     , "panic");
        D_a.Add(dog_animation.worried   , "worried");
        
        expres.Add(expression.angry,    Boss_expressions[0]);
        expres.Add(expression.sad,      Boss_expressions[1]);
        expres.Add(expression.normal,   Boss_expressions[2]);

        Debug.Log(expres.ContainsKey(expression.normal));

    }

    private void Start()
    {
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
        dialogue[i].ex.Add(expression.normal);
        dialogue[i].da.Add(dog_animation.normal);
    }
    #endregion

    #region Boss emotions

    public expression get_expressions(int a,int b)
    {
        if (dialogue[a].ex.Count == dialogue[a].S.Count)
        { return dialogue[a].ex[b]; }

        return expression.normal;
    }

    public void set_expression(int a, int b, expression i)
    {
        //exp.Add(expression[a,b])
        if (dialogue[a].ex.Count == dialogue[a].S.Count)    
        {dialogue[a].ex[b] = i; return;}
         
        dialogue[a].ex.Add(expression.normal);
    }

    public dog_animation get_animation(int a, int b)
    {
        if (dialogue[a].da.Count == dialogue[a].S.Count)
        { return dialogue[a].da[b]; }

        return dog_animation.normal;
    }

    public void set_animation(int a, int b, dog_animation i)
    {
        //exp.Add(expression[a,b])
        if (dialogue[a].da.Count == dialogue[a].S.Count)
        { dialogue[a].da[b] = i; return; }

        dialogue[a].da.Add(dog_animation.normal);
    }


    public void set_sprite(expression s)
    {
        boss.sprite = expres[s];
    }

  




    #endregion

}

[System.Serializable]
public class Boss_speech
{
    public List<string> S = new List<string>();
    public List<expression> ex = new List<expression>();
    public List<dog_animation> da = new List<dog_animation>();
}

[System.Serializable]
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
