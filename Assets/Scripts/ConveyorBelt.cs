using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    [SerializeField] public float speed;
    [SerializeField] Vector2 direction;
    Rigidbody rb;

    List<Rigidbody> itemsInConveyor = new List<Rigidbody>();

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (itemsInConveyor.Count != 0)
            foreach (Rigidbody itemRb in itemsInConveyor)
            {
                //Rigidbody itemRb = item.GetComponent<Rigidbody>();
                itemRb.velocity = speed * new Vector3(direction.x, 0, direction.y).normalized;
            }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GrabbableItem g;
        RobotBody r;

        Transform selectedObject = null;

        if (collision.rigidbody.TryGetComponent<GrabbableItem>(out g))
            selectedObject = g.transform;
        //else if (collision.rigidbody.TryGetComponent<RobotBody>(out r))
        //    selectedObject = r.transform;

        if (selectedObject != null)
        {
            itemsInConveyor.Add(selectedObject.GetComponent<Rigidbody>());
            selectedObject.SetParent(transform, true);
        }
    }

    // This method is called from the player when he takes an item from this conveyor belt
    public void RemoveItemFromConveyor(GrabbableItem g)
    {
        itemsInConveyor.Remove(g.GetComponent<Rigidbody>());
    }

    private void OnCollisionExit(Collision collision)
    {
        GrabbableItem g;
        RobotBody r;

        Transform selectedObject = null;

        if (collision.rigidbody.TryGetComponent<GrabbableItem>(out g))
            selectedObject = g.transform;
        //else if (collision.rigidbody.TryGetComponent<RobotBody>(out r))
        //    selectedObject = r.transform;

        if (selectedObject != null)
        {
            itemsInConveyor.Remove(selectedObject.GetComponent<Rigidbody>());
            selectedObject.SetParent(null, true);
        }
    }
}
