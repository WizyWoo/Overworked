using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    [SerializeField] Transform a, b;

    [SerializeField] bool goAAtStart;

    [SerializeField] float velocity;

    [SerializeField] AnimationCurve animationCurve;

    // Start is called before the first frame update
    void Start()
    {
        if (goAAtStart) 
        {
            transform.position = b.position;
            StartCoroutine(GoA());
        }
        else
        {
            transform.position = a.position;
            StartCoroutine(GoB());
        }
    }

    IEnumerator GoA()
    {
        float c = 0; 
        while (Vector3.Distance(transform.position, a.position) > 0.1f)
        {
            yield return 0;
            transform.position = Vector3.Lerp(b.position, a.position, animationCurve.Evaluate(c));
            c += velocity * Time.deltaTime;
            c = Mathf.Clamp(c, 0, 1);
        }

        StartCoroutine(GoB());
        StopCoroutine(GoA());
    }

    IEnumerator GoB()
    {
        float c = 0;
        while (Vector3.Distance(transform.position, b.position) > 0.1f)
        {
            yield return 0;
            transform.position = Vector3.Lerp(a.position, b.position, animationCurve.Evaluate(c));
            c += velocity * Time.deltaTime;
            c = Mathf.Clamp(c, 0, 1);
        }

        StartCoroutine(GoA());
        StopCoroutine(GoB());
    }

    private void OnCollisionEnter(Collision collision)
    {
        PlayerController playerController = collision.transform.GetComponent<PlayerController>();
        GrabbableItem grabbableItem = collision.transform.GetComponent<GrabbableItem>();

        if (playerController == null && grabbableItem == null)
            return;

        if (grabbableItem == null)
            playerController.transform.parent = transform;
        else
            grabbableItem.transform.parent = transform;
    }

    private void OnCollisionExit(Collision collision)
    {
        PlayerController playerController = collision.transform.GetComponent<PlayerController>();
        CraftableItem craftableItem = collision.transform.GetComponent<CraftableItem>();

        if (playerController == null && craftableItem == null)
            return;

        if (craftableItem == null)
            playerController.transform.parent = null;
        else
            craftableItem.transform.parent = null;
    }
}
