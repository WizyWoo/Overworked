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
        yield return new WaitForSeconds(time);

        float x = Random.Range(-size.x, size.x);
        float z = Random.Range(-size.y, size.y);

        GameObject clon = Instantiate(item);
        Vector3 pos = new Vector3(x, transform.position.y, z);
        clon.transform.position = pos;

        float _force = Random.Range(force.x, force.y);
        clon.GetComponent<Rigidbody>().AddForce(Vector3.down * _force, ForceMode.Impulse);
        Destroy(clon, destroyTime);

        //if raycast from pos touches floor create shadow
        if (Physics.Raycast(pos, Vector3.down, out RaycastHit raycastHit, layer))
        {
            GameObject go = Instantiate(shadow);
            go.transform.position = new Vector3(pos.x, raycastHit.transform.position.y + raycastHit.collider.bounds.extents.y + offset, pos.z);
            go.transform.localScale = new Vector3(initialScale, 1f, initialScale);
            Destroy(go, destroyTime);
            while (go.transform.localScale.x <= finalScale)
            {
                go.transform.localScale += new Vector3(scaleGrowPerSec, 0f, scaleGrowPerSec) * Time.deltaTime;
                yield return 0;
            }
        }
    }
}
