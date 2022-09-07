using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    [SerializeField] Transform a, b;

    [SerializeField] bool goLeftAtStart;

    [SerializeField] float velocity;
    // Start is called before the first frame update
    void Start()
    {
        if (goLeftAtStart) 
        {
            transform.position = b.position;
            StartCoroutine(GoLeft());
        }
        else
        {
            transform.position = a.position;
            GoRight();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator GoLeft()
    {   
        float c = 0;
        while(Vector3.Distance(transform.position, a.position) > 0.1f)
        {
            yield return 0;
            c += velocity * Time.deltaTime;
            Vector3.Lerp(transform.position, a.position, c);
        }
    }

    IEnumerator GoRight()
    {
        yield return 0;
        if (Vector3.Distance(transform.position, b.position) > 0.1f)
        {
            Vector3.Lerp(transform.position, b.position, velocity * Time.deltaTime);
        }
        else
        {

        }
    }
}
