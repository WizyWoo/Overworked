using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotRail : MonoBehaviour
{
    [SerializeField] public float speed;
    [SerializeField] Vector2 direction;
    public bool functional = true;
    Rigidbody rb;
    List<RobotBody> itemsInConveyor = new List<RobotBody>();

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (!functional) return;

        if (itemsInConveyor.Count != 0)
            foreach (RobotBody robot in itemsInConveyor)
            {
                //Rigidbody robotRB = robot.GetComponent<Rigidbody>();
                robot.transform.position += new Vector3(direction.x, 0, direction.y) * (speed / 60);
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
