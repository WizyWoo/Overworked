using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private void Update()
    {
        if(!GetComponent<ParticleSystem>().IsAlive()) Destroy(this.gameObject);
    }
}
