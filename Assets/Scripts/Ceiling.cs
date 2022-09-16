using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ceiling : MonoBehaviour
{
    [SerializeField] Vector2 size = new Vector2(7, 4), force = new Vector2(2, 5), delay = new Vector2(0.5f, 2);
    [SerializeField] LayerMask floorLayer;

    [SerializeField] GameObject shadow;

    [SerializeField] float offset = 0.1f;

    [SerializeField] float initialScale = 0.2f, scaleGrowPerSec = 3f, finalScale = 1f, destroyTime = 2f;

    private void Update()
    {

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
        StartCoroutine(Destroy(clon, destroyTime));

        //if raycast from pos touches floor create shadow
        if (Physics.Raycast(pos, Vector3.down, out RaycastHit raycastHit, floorLayer))
        {
            //create shadow
            GameObject go = Instantiate(shadow);

            //reference to shadow so it can be destroyed earlier
            clon.GetComponent<ThrowableItems>().shadow = go;

            //initialize position and scale
            go.transform.position = new Vector3(pos.x, raycastHit.transform.position.y + raycastHit.collider.bounds.extents.y + offset, pos.z);
            go.transform.localScale = new Vector3(initialScale, initialScale, 1f);

            //set up shadow destruction
            StartCoroutine(Destroy(go, destroyTime));

            //scale animation
            while (go != null && go.transform.localScale.x <= finalScale)
            {                   
                go.transform.localScale += new Vector3(scaleGrowPerSec, scaleGrowPerSec, 0f) * Time.deltaTime;
                yield return 0;
            }
        }
    }

    IEnumerator Destroy(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);

        if(obj != null)
        {
            Destroy(obj);
        }
    }
}
