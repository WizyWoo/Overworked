using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class megaphone : MonoBehaviour
{
    public AnimationCurve scaling;

    public float i = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {

        if (i > 1)
        {
            i = 0;
        }    

        i = i + Time.deltaTime;
        transform.localScale = new Vector3(scaling.Evaluate(i) + 1, 1, scaling.Evaluate(i) + 1);
       

    }
}
