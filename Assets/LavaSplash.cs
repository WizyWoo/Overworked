using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaSplash : MonoBehaviour
{
    ParticleSystem particleSystem;
    int particleNumber;
    public FMODUnity.EventReference lavaSplashSound;
    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        particleNumber = 0;
    }

    void Update()
    {
        if (particleSystem.particleCount > particleNumber && particleSystem.particleCount != 0)
        {
            SoundManager.Instance.PlaySound(lavaSplashSound ,gameObject);
            particleNumber = particleSystem.particleCount;
        }
        else if (particleSystem.particleCount == 0)
            particleNumber = 0;
    }
}
