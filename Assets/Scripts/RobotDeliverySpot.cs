using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotDeliverySpot : MonoBehaviour
{
    [SerializeField] ParticleSystem particleSystem;
    public int Total_Assembled_Robots;
    public bool IncrementWinCon, IncrementLoseCon;
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
                Debug.Log("INCORRECT");
                IncrementLoseCon = true;
            } 

            // Inform the conveyor belt of the robot to remove it
            robotDelivered.transform.GetComponentInParent<RobotRail>().RemoveRobotFromConveyor(robotDelivered);
            Destroy(robotDelivered.gameObject);
        }       
    }

    // Returns true if the robot has all the parts assembled
    bool IsRobotRepaired(RobotBody r)
    { return r.leftArmAssembled && r.rightArmAssembled && r.wheelAssembled; }
}
