using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotDeliverySpot : MonoBehaviour
{
    [SerializeField] ParticleSystem explosionParticleSystem;
    public int Total_Assembled_Robots;
    public bool IncrementWinCon, IncrementLoseCon;

    [SerializeField] GameObject armToThrow, wheelToThrow;

    [SerializeField] Ceiling ceiling;

    Level01_Manager level01Manager;

    private void Awake()
    {
        level01Manager = GetComponentInParent<Level01_Manager>();
    }

    private void OnTriggerEnter(Collider other)
    {

        RobotBody robotDelivered;
        if (other.TryGetComponent<RobotBody>(out robotDelivered))
        {
            if (IsRobotRepaired(robotDelivered))
            {
                Total_Assembled_Robots++;
                IncrementWinCon = true;

                level01Manager.CorrectRobot();
            }

            else
            {
                level01Manager.IncorrectRobot();

                Instantiate(explosionParticleSystem, robotDelivered.transform.position, robotDelivered.transform.rotation);
                // if (robotDelivered.leftArmAssembled)
                // {
                //     ceiling.ThrowItem(arm);
                //    // ThrowItem(arm);
                // }
                // if (robotDelivered.rightArmAssembled)
                // {
                //     ceiling.ThrowItem(arm);
                //     // ThrowItem(arm);
                // }
                // if (robotDelivered.wheelAssembled)
                // {
                //     ceiling.ThrowItem(wheel);
                //     // ThrowItem(wheel);
                // }

                IncrementLoseCon = true;
            }


            // Inform the conveyor belt of the robot to remove it
            RobotRail robotRail = robotDelivered.transform.GetComponentInParent<RobotRail>();
            if (robotRail != null)
                robotRail.RemoveRobotFromConveyor(robotDelivered);

            // Disable collisions
            BoxCollider[] coll = robotDelivered.GetComponents<BoxCollider>();
            foreach (BoxCollider box in coll)
                box.enabled = false;

            Destroy(robotDelivered.gameObject);
        }
    }


    //Parabolic movement
    //Vector3 BallisticVel(Transform target, float angle)
    //{
    //    Vector3 dir = target.position - transform.position;  // get target direction
    //    float h = dir.y;  // get height difference
    //    dir.y = 0;  // retain only the horizontal direction
    //    float dist = dir.magnitude;  // get horizontal distance
    //    float a = angle * Mathf.Deg2Rad;  // convert angle to radians
    //    dir.y = dist * Mathf.Tan(a);  // set dir to the elevation angle
    //    dist += h / Mathf.Tan(a);  // correct for small height differences
    //                               // calculate the velocity magnitude
    //    float vel = Mathf.Sqrt(dist * Physics.gravity.magnitude / Mathf.Sin(2 * a));
    //    return vel * dir.normalized;
    //} 

// Returns true if the robot has all the parts assembled
bool IsRobotRepaired(RobotBody r)
    { return r.leftArmAssembled && r.rightArmAssembled && r.wheelAssembled; }
}
