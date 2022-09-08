using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotDeliverySpot : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        RobotBody robotDelivered;
        if (other.TryGetComponent<RobotBody>(out robotDelivered))
        {
            if (IsRobotRepaired(robotDelivered))
                Debug.Log("CORRECT");
            else Debug.Log("INCORRECT");

            // Inform the conveyor belt of the robot to remove it
            robotDelivered.transform.GetComponentInParent<RobotRail>().RemoveRobotFromConveyor(robotDelivered);
            Destroy(robotDelivered.gameObject);
        }       
    }

    // Returns true if the robot has all the parts assembled
    bool IsRobotRepaired(RobotBody r)
    { return r.leftArmAssembled && r.rightArmAssembled && r.wheelAssembled; }
}
