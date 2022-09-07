using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Vector2 direction;
    Rigidbody rb;

    List<GrabbableItem> itemsInConveyor = new List<GrabbableItem>();

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (itemsInConveyor.Count != 0)
            foreach (GrabbableItem item in itemsInConveyor)
            {
                Rigidbody itemRb = item.GetComponent<Rigidbody>();
                itemRb.velocity = speed * new Vector3(direction.x, 0, direction.y).normalized;
            }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GrabbableItem g = collision.rigidbody.GetComponent<GrabbableItem>();

        if (g != null)
        {
            itemsInConveyor.Add(g);
            g.transform.SetParent(transform);
        }
    }

    // This method is called from the player when he takes an item from this conveyor belt
    public void RemoveItemFromConveyor(GrabbableItem g)
    {
        itemsInConveyor.Remove(g);
    }

    private void OnCollisionExit(Collision collision)
    {
        GrabbableItem g = collision.rigidbody.GetComponent<GrabbableItem>();

        if (g != null)
        {
            itemsInConveyor.Remove(g);
            g.transform.SetParent(null);
        }
    }
}
