using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ceiling : MonoBehaviour
{
    [SerializeField] Vector2 size = new Vector2(7, 4), force = new Vector2(2, 5), delay = new Vector2(0.5f, 2);
    [SerializeField] LayerMask layer;

    [SerializeField] GameObject shadow;

    [SerializeField] float offset = 0.1f;

    [SerializeField] GameObject arm, wheel;

    float initialScale = 0.2f, scaleGrowPerSec = 3f, finalScale = 1f, destroyTime = 2f;

    private void Update()
    {
        //testing controls
        if (Input.GetKeyDown(KeyCode.T))
        {
            ThrowItem(arm);
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            ThrowItem(wheel);
        }
    }

    public void ThrowItem(GameObject item)
    {
        float time = Random.Range(delay.x, delay.y);
        StartCoroutine(ActualThrow(item, time));
    }

    IEnumerator ActualThrow(GameObject item, float time)
    {
        //delay so items don't fall right away
        yield return new WaitForSeconds(time);

        //get min and max of the zone where they can fall
        float x = Random.Range(-size.x, size.x);
        float z = Random.Range(-size.y, size.y);

        //create falling item
        GameObject clon = Instantiate(item);

        //set up position
        Vector3 pos = new Vector3(x, transform.position.y, z);
        clon.transform.position = pos;

        //going down
        float _force = Random.Range(force.x, force.y);
        clon.GetComponent<Rigidbody>().AddForce(Vector3.down * _force, ForceMode.Impulse);

        //item destruction
        Destroy(clon, destroyTime);

        //if raycast from pos touches floor create shadow
        if (Physics.Raycast(pos, Vector3.down, out RaycastHit raycastHit, layer))
        {
            //create shadow
            GameObject go = Instantiate(shadow);

            //reference to shadow so it can be destroyed earlier
            clon.GetComponent<ThrowableItems>().shadow = go;

            //initialize position and scale
            go.transform.position = new Vector3(pos.x, raycastHit.transform.position.y + raycastHit.collider.bounds.extents.y + offset, pos.z);
            go.transform.localScale = new Vector3(initialScale, 1f, initialScale);

            //set up shadow destruction
            Destroy(go, destroyTime);

            //scale animation
            while (go.transform.localScale.x <= finalScale)
            {
                go.transform.localScale += new Vector3(scaleGrowPerSec, 0f, scaleGrowPerSec) * Time.deltaTime;
                yield return 0;
            }
        }
    }
}