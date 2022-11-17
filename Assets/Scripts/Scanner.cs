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
    [SerializeField] Light spotLight;
    [SerializeField] float timeBulbOn;
    [SerializeField] Renderer bulbRenderer;
    [SerializeField] Material correctColMat, wrongColMat, standbyColMat;
    [SerializeField] Transform okayRobotPos;
    public FMODUnity.EventReference correctRobot, incorrectRobot;

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
                spotLight.color = rightCol;
                bulbRenderer.material = correctColMat;
                SoundManager.Instance.PlaySound(correctRobot, gameObject);
                Total_Assembled_Robots++;
                level01Manager.CorrectRobot();
                IncrementWinCon = true;

                robotDelivered.GetComponentInParent<RobotRail>().RemoveRobotFromConveyor(robotDelivered);
                other.transform.SetParent(null);
                other.transform.position = okayRobotPos.position;
            }
            else
            {
                StartCoroutine(WrongRobot(robotDelivered));
                //change color
                spotLight.color = wrongCol;
                bulbRenderer.material = wrongColMat;
                SoundManager.Instance.PlaySound(incorrectRobot, gameObject);
                level01Manager.IncorrectRobot();
                IncrementLoseCon = true;

                if (robotDelivered.leftArmAssembled)
                {
                    if (throwItemsIfIncorrectRobot)
                        ceiling.ThrowItem(armToThrow);
                }
                if (robotDelivered.rightArmAssembled)
                {
                    if (throwItemsIfIncorrectRobot)
                        ceiling.ThrowItem(armToThrow);
                }
                if (robotDelivered.wheelAssembled)
                {
                    if (throwItemsIfIncorrectRobot)
                        ceiling.ThrowItem(wheelToThrow);
                }
            }
            Invoke("ReturnBulbToDefault", timeBulbOn);
        }
    }

    [SerializeField] bool throwItemsIfIncorrectRobot;


    void ReturnBulbToDefault()
    {
        spotLight.color = defaultCol;
        bulbRenderer.material = standbyColMat;
    }

    IEnumerator WrongRobot(RobotBody rb)
    {
        rb.GetComponentInParent<RobotRail>().RemoveRobotFromConveyor(rb);

        yield return new WaitForSeconds(0.5f);
        Instantiate(explosionParticleSystem, rb.transform.position, rb.transform.rotation);

        Destroy(rb.gameObject);
    }

    // Returns true if the robot has all the parts assembled
    bool IsRobotRepaired(RobotBody r)
    { return r.leftArmAssembled && r.rightArmAssembled && r.wheelAssembled; }
}
