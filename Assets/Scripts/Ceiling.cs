using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ceiling : MonoBehaviour
{
    [SerializeField] Vector2 size = new Vector2(7, 4), force = new Vector2(2, 5), delay = new Vector2(0.5f, 2);
    [SerializeField] LayerMask layer;

    [SerializeField] GameObject shadowSprite;

    [SerializeField] float offset = 0.1f;

    [SerializeField] GameObject arm, wheel;

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


        if (Physics.Raycast(pos, Vector3.down, out RaycastHit raycastHit, layer))
        {
            GameObject go = Instantiate(shadowSprite);
            go.transform.position = new Vector3(pos.x, raycastHit.transform.position.y + raycastHit.collider.bounds.extents.y + offset, pos.z);
        }
    }
}
