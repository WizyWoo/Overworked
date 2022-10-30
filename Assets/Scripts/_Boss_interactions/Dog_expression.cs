using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog_expression : MonoBehaviour
{
    public Sprite[] emotions;
    public Animator effects;
    public SpriteRenderer Sprite_renderer;

    private void Start()
    {
        Sprite_renderer = GetComponent<SpriteRenderer>();
        effects = GetComponent<Animator>();
    }

    public void Switch_emotion(Sprite sprite_emotion)
    {
        Sprite_renderer.sprite = sprite_emotion;
    }
    public void Switch_effect(string animation_effect)
    {
        effects.Play(animation_effect);
    }
}
