using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{

    public float DestroyTimer;

    private void Start()
    {

        Destroy(gameObject, DestroyTimer);

    }

}
