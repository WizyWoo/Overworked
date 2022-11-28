using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyMachinePlayAnimation : MonoBehaviour
{
    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
        float randomTime = Random.Range(15, 25);
        Invoke("PlayAnim", randomTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PlayAnim()
    {
        anim.SetTrigger("Play");
        float randomTime = Random.Range(15, 25);
        Invoke("PlayAnim", randomTime);
    }
}
