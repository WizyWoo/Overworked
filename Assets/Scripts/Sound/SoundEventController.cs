using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class SoundEventController : MonoBehaviour
{

    public EventReference SoundEvent;
    [SerializeField]
    public enum PlayModeType
    {

        OnStart,
        OnTrigger,
        OnCollision

    }
    public PlayModeType PlayMode;
    private SoundManager sm;

    private void Start()
    {

        sm = SoundManager.Instance;

        if(PlayMode == PlayModeType.OnStart)
            sm.PlaySound(SoundEvent, gameObject);

    }

    private void OnCollisionEnter(Collision _col)
    {

        if(PlayMode == PlayModeType.OnCollision)
            sm.PlaySound(SoundEvent, gameObject);

    }

}
