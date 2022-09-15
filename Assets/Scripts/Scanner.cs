using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    [SerializeField] ParticleSystem explosionParticleSystem;
    public int Total_Assembled_Robots;
    public bool IncrementWinCon, IncrementLoseCon;

    [SerializeField] GameObject armToThrow, wheelToThrow;

    [SerializeField] Ceiling ceiling;

    Level01_Manager level01Manager;

    [SerializeField] [ColorUsage(true, true)] Color rightCol, wrongCol, defaultCol;
    [SerializeField] Material glassMat;
    [SerializeField] float timeBulbOn;
    [SerializeField] Renderer bulbRenderer;
    [SerializeField] Transform okayRobotPos;

    int cont;
    private void Awake()
    {
        level01Manager = GetComponentInParent<Level01_Manager>();

        ReturnBulbToDefault();
    }

    private void OnTriggerEnter(Collider other)
    {
        cont++;
        Debug.Log(cont);

        RobotBody robotDelivered;
        if (other.TryGetComponent<RobotBody>(out robotDelivered))
        {
            if (IsRobotRepaired(robotDelivered))
            {
                //change color
                glassMat.color = rightCol;
                glassMat.EnableKeyword("_EMISSION");
                glassMat.globalIlluminationFlags = MaterialGlobalIlluminationFlags.EmissiveIsBlack;
                glassMat.SetColor("_EmissionColor", rightCol);

                Total_Assembled_Robots++;
                IncrementWinCon = true;

                robotDelivered.GetComponentInParent<RobotRail>().RemoveRobotFromConveyor(robotDelivered);
                other.transform.SetParent(null);
                other.transform.position = okayRobotPos.position;

                level01Manager.CorrectRobot();
            }

            else
            {
                StartCoroutine(WrongRobot(robotDelivered));

                //change color
                glassMat.color = wrongCol;
                glassMat.EnableKeyword("_EMISSION");
                glassMat.globalIlluminationFlags = MaterialGlobalIlluminationFlags.EmissiveIsBlack;
                glassMat.SetColor("_EmissionColor", new Color(wrongCol.r, wrongCol.g, wrongCol.b));

                level01Manager.IncorrectRobot();

                if (robotDelivered.leftArmAssembled)
                {
                    ceiling.ThrowItem(armToThrow);
                }
                if (robotDelivered.rightArmAssembled)
                {
                    ceiling.ThrowItem(armToThrow);
                    // ThrowItem(arm);
                }
                if (robotDelivered.wheelAssembled)
                {
                    ceiling.ThrowItem(wheelToThrow);
                }

                IncrementLoseCon = true;
            }

            Invoke("ReturnBulbToDefault", timeBulbOn);
        }
    }


    void ReturnBulbToDefault()
    {
        glassMat.color = defaultCol;
        glassMat.SetColor("_EmissionColor", defaultCol);
        glassMat.DisableKeyword("_EMISSION");
        glassMat.globalIlluminationFlags = MaterialGlobalIlluminationFlags.EmissiveIsBlack;
    }

    IEnumerator WrongRobot(RobotBody rb)
    {
        rb.GetComponentInParent<RobotRail>().RemoveRobotFromConveyor(rb);

        yield return new WaitForSeconds(0.5f);
        Instantiate(explosionParticleSystem, rb.transform.position, rb.transform.rotation);

        Destroy(rb.gameObject);
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
