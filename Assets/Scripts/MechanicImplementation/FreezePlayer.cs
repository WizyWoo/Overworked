using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezePlayer : MonoBehaviour
{
    public bool Frozen, doOnce;
    public GameObject IceCubePNG;
    private float Timer;
    public FMODUnity.EventReference FreezingSound, UnfreezingSound;
    // Start is called before the first frame update
    void Start()
    {
        IceCubePNG.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Timer <= 0 )
        {
            Frozen = false;

        }
        if (Frozen == false && doOnce == false)
        {
            IceCubePNG.SetActive(false);
            gameObject.GetComponent<PlayerController>().enabled = true;
            doOnce = true;
            Timer = 2;
            SoundManager.Instance.PlaySound(UnfreezingSound,gameObject);
        }
        if (Frozen == true)
        {
            IceCubePNG.SetActive(true);
            gameObject.GetComponent<PlayerController>().enabled = false;
            Timer -= Time.deltaTime;
            doOnce = false;
            SoundManager.Instance.PlaySound(FreezingSound, gameObject);
        }
    }
}
