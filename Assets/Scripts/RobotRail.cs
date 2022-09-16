using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotRail : MonoBehaviour
{
    [SerializeField] public float speed;
    [SerializeField] Vector2 direction;
    Rigidbody rb;

    List<RobotBody> itemsInConveyor = new List<RobotBody>();

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (itemsInConveyor.Count != 0)
            foreach (RobotBody robot in itemsInConveyor)
            {
                Rigidbody robotRB = robot.GetComponent<Rigidbody>();
                robotRB.velocity = speed * new Vector3(direction.x, 0, direction.y).normalized;
            }
    }

    private void OnCollisionEnter(Collision collision)
    {
        RobotBody r;

        if (collision.rigidbody.TryGetComponent<RobotBody>(out r))
        {
            itemsInConveyor.Add(r);
            r.transform.SetParent(transform, true);
        }
    }

    // This method is called from a RobotDeliverySpot when the robot is delivered and destroyed
    public void RemoveRobotFromConveyor(RobotBody r)
    {
        itemsInConveyor.Remove(r);
    }

    private void OnCollisionExit(Collision collision)
    {
        RobotBody r;

        if (collision.rigidbody.TryGetComponent<RobotBody>(out r))
        {
            itemsInConveyor.Remove(r);
            r.transform.SetParent(null, true);
        }
    }
}
