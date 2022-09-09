using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotDeliverySpot : MonoBehaviour
{
    [SerializeField] ParticleSystem particleSystem;
    public int Total_Assembled_Robots;
    public bool IncrementWinCon, IncrementLoseCon;


    [SerializeField] GameObject arm, wheel;

    [SerializeField] Vector2 throwAngle;
    [SerializeField] float gravity = 9.8f;
    private void OnTriggerEnter(Collider other)
    {
        RobotBody robotDelivered;
        if (other.TryGetComponent<RobotBody>(out robotDelivered))
        {
            if (IsRobotRepaired(robotDelivered))
            {
                Debug.Log("CORRECT");
                Total_Assembled_Robots++;
                IncrementWinCon = true;
            }

            else
            {
                Instantiate(particleSystem, robotDelivered.transform.position, robotDelivered.transform.rotation);
                if (robotDelivered.leftArmAssembled)
                {
                    ThrowItem(arm);
                }
                if (robotDelivered.rightArmAssembled)
                {
                    ThrowItem(arm);
                }
                if (robotDelivered.wheelAssembled)
                {
                    ThrowItem(wheel);
                }
                Debug.Log("INCORRECT");
                IncrementLoseCon = true;
            }


            // Inform the conveyor belt of the robot to remove it
            RobotRail robotRail = robotDelivered.transform.GetComponentInParent<RobotRail>();
            if (robotRail != null)
                robotRail.RemoveRobotFromConveyor(robotDelivered);

            Destroy(robotDelivered.gameObject);
        }
    }

    private void ThrowItem(GameObject item)
    {
        Transform target = FindObjectOfType<PlayerController>().transform;

        if (target != null)
        {
            GameObject clon = Instantiate(item, this.transform);
            clon.transform.position = transform.position;

            float angle = Random.Range(throwAngle.x, throwAngle.y);
            clon.GetComponent<Rigidbody>().velocity = BallisticVel(target, 45);
        }
    }

    Vector3 BallisticVel(Transform target, float angle)
    {
        Vector3 dir = target.position - transform.position;  // get target direction
        float h = dir.y;  // get height difference
        dir.y = 0;  // retain only the horizontal direction
        float dist = dir.magnitude;  // get horizontal distance
        float a = angle * Mathf.Deg2Rad;  // convert angle to radians
        dir.y = dist * Mathf.Tan(a);  // set dir to the elevation angle
        dist += h / Mathf.Tan(a);  // correct for small height differences
                                   // calculate the velocity magnitude
        float vel = Mathf.Sqrt(dist * Physics.gravity.magnitude / Mathf.Sin(2 * a));
        return vel * dir.normalized;
    } 

// Returns true if the robot has all the parts assembled
bool IsRobotRepaired(RobotBody r)
    { return r.leftArmAssembled && r.rightArmAssembled && r.wheelAssembled; }
}
