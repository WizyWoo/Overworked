using System;
using System.Collections;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    // Si esta a true, haces el tutorial
    [SerializeField] public bool doTutorial;

    [HideInInspector] public bool duringTutorial;

    // Returns true when the tutorial of a panel is hiding 
    [HideInInspector] public bool changingPhase;


    [SerializeField] Transform tutorialRobotSpawn_Left;
    [SerializeField] Transform tutorialRobotSpawn_Right;

    [SerializeField] GameObject bodyRobotPrefab;
    [SerializeField] GameObject repairedArmPrefab;
    [SerializeField] GameObject repairedWheelPrefab;

    [SerializeField] GameObject outlineArmPrefab;
    [SerializeField] GameObject outlineWheelPrefab;


    static TutorialManager instance;
    public static TutorialManager GetInstance()
    { return instance; }


    // Anti messing up gameobjects : invisible collisions preventing the player from messing up the tutorial
    [Header("ANTI MESSING UP OBJ")]

    [SerializeField] RepairStation antiMessUp_repairStation_Arm;
    [SerializeField] RepairStation antiMessUp_repairStation_Wheel;
    [SerializeField] GameObject antiMessUp_firstRobot;

    // Tutorial phases
    public enum tutorialPhase
    {
        grabArmFromConveyor, throwArm_p1, grabArmFromFloor_p2, repairArm, grabArmFromRepairTable,
        throwArm_p2, grabArmFromFloor_p1, assembleArm,
        grabWheelFromConveyor, throwWheel_p1, grabWheelFromFloor_p2, repairWheel, grabWheelFromRepairTable,
        throwWheel_p2, grabWheelFromFloor_p1, assembleOtherRobot, tutorialDone
    }

    tutorialPhase[] tutPhasesMultiplayer =
    {
        //tutorialPhase.grabArmFromConveyor, tutorialPhase.throwArm_p1, tutorialPhase.grabArmFromFloor_p2, tutorialPhase.repairArm,
        //tutorialPhase.grabArmFromRepairTable, tutorialPhase.throwArm_p2, tutorialPhase.grabArmFromFloor_p1, tutorialPhase.assembleArm,

        //tutorialPhase.grabWheelFromConveyor,
        //tutorialPhase.assembleOtherRobot, tutorialPhase.tutorialDone

                tutorialPhase.grabArmFromConveyor, tutorialPhase.throwArm_p1, tutorialPhase.grabArmFromFloor_p2, tutorialPhase.repairArm, tutorialPhase.grabArmFromRepairTable,
        tutorialPhase.throwArm_p2, tutorialPhase.grabArmFromFloor_p1, tutorialPhase.assembleArm,
        tutorialPhase.grabWheelFromConveyor, tutorialPhase.throwWheel_p1, tutorialPhase.grabWheelFromFloor_p2, tutorialPhase.repairWheel, tutorialPhase.grabWheelFromRepairTable,
        tutorialPhase.throwWheel_p2, tutorialPhase.grabWheelFromFloor_p1, tutorialPhase.assembleOtherRobot, tutorialPhase.tutorialDone
    };

    tutorialPhase[] tutPhasesOneplayer =
    {
        tutorialPhase.grabArmFromConveyor, tutorialPhase.repairArm,
        tutorialPhase.grabArmFromRepairTable, tutorialPhase.assembleArm,
        tutorialPhase.assembleOtherRobot, tutorialPhase.tutorialDone
    };


    tutorialPhase[] usingThisTutorialPhases;

    // The currentPhase variable, show what the player must do
    public tutorialPhase currentPhase;

    int currentPhaseIndex = 0;

    // Tutorial elements are gameobjects that store tutorial info for each phase
    [SerializeField] TutorialItem[] tutorialItems;


    [SerializeField] RobotRail leftRobotRail;
    [SerializeField] RobotRail rightRobotRail;
    [SerializeField] ItemSpawner armSpawner;
    [SerializeField] ItemSpawner wheelSpawner;
    [SerializeField] ItemSpawner[] leftRobotSpawner;
    [SerializeField] ItemSpawner[] rightRobotSpawner;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        InizializeTutorialElements();

        if (doTutorial)
        {
            StartTutorial();
        }
        else
        {
            EndTutorial();
        }
    }

    void InizializeTutorialElements()
    {
        transform.GetChild(0).gameObject.SetActive(true);

        foreach (TutorialItem tutorialElement in tutorialItems)
        {
            tutorialElement.AssignReferences();
            tutorialElement.HideAllElements();
        }
    }

    void StartTutorial()
    {
        duringTutorial = true;

        if (GameManager.instance.onlyOnePlayer)
            usingThisTutorialPhases = tutPhasesOneplayer;
        else
            usingThisTutorialPhases = tutPhasesMultiplayer;

        currentPhase = usingThisTutorialPhases[0];
        // Show first phase
        ShowTutorialItem(currentPhase);

        leftRobotRail.functional = false;
        rightRobotRail.functional = false;
        armSpawner.functional = true;
        wheelSpawner.functional = false;


        foreach (var spawner in leftRobotSpawner)
            spawner.functional = false;

        foreach (var spawner in rightRobotSpawner)
            spawner.functional = false;
    }

    public void TryToChangePhase(tutorialPhase phaseDone)
    {
        if (duringTutorial)
            StartCoroutine(TryToChangePhase_IEnumerator(phaseDone));
    }

    IEnumerator TryToChangePhase_IEnumerator(tutorialPhase phaseDone)
    {
        // Wait until the previous panel has already finished
        yield return new WaitUntil(() => !changingPhase);

        // If the player just perform the action that he was supposed to do right now according to the tutorial
        if (currentPhase == phaseDone)
        {
            // Hide the previous tutorial item
            HideTutorialItem(currentPhase);

            currentPhaseIndex++;
            currentPhase = usingThisTutorialPhases[currentPhaseIndex];

            changingPhase = true;
            yield return new WaitForSeconds(.5f);
            changingPhase = false;


            // Spawn robot body when neccesary

            if (!GameManager.instance.onlyOnePlayer)
            {
                if (currentPhase == tutorialPhase.assembleArm - 1)
                    SpawnFirstRobot();
            }
            else
            {
                if (currentPhase == tutorialPhase.repairArm)
                    SpawnFirstRobot();
            }

            if (!GameManager.instance.onlyOnePlayer)
            {
                if (currentPhase == tutorialPhase.grabWheelFromConveyor)
                    SpawnSecondRobot();
            }
            else
            {
                if (currentPhase == tutorialPhase.assembleOtherRobot)
                    SpawnSecondRobot();
            }

            // Anti messing up collisions

            if (!GameManager.instance.onlyOnePlayer)
            {
                // FIRST ROBOT

                // Player 1 grabbed Arm
                if (currentPhase == tutorialPhase.throwArm_p1)
                {
                    // Activate collisions of repair stations
                    antiMessUp_repairStation_Arm.GetComponent<CapsuleCollider>().enabled = false;
                }

                else if (currentPhase == tutorialPhase.repairWheel)
                    antiMessUp_repairStation_Arm.GetComponent<CapsuleCollider>().enabled = true;

                else if (currentPhase == tutorialPhase.grabArmFromFloor_p1)
                {
                    yield return new WaitForSeconds(.5f);

                    firstRobotBody.GetComponents<BoxCollider>()[1].enabled = false;
                }

                else if (currentPhase == tutorialPhase.assembleArm)
                    firstRobotBody.GetComponents<BoxCollider>()[1].enabled = true;




                // SECOND ROBOT


                else if (currentPhase == tutorialPhase.throwWheel_p1)
                    antiMessUp_repairStation_Wheel.GetComponent<CapsuleCollider>().enabled = false;

                else if (currentPhase == tutorialPhase.grabWheelFromFloor_p2)
                    antiMessUp_repairStation_Wheel.GetComponent<CapsuleCollider>().enabled = false;


                else if (currentPhase == tutorialPhase.grabArmFromFloor_p1)
                {
                    yield return new WaitForSeconds(.5f);

                    secondRobotBody.GetComponents<BoxCollider>()[1].enabled = false;
                }

                else if (currentPhase == tutorialPhase.assembleArm)
                    secondRobotBody.GetComponents<BoxCollider>()[1].enabled = true;

                //antiMessUp_repairStation_Wheel
            }
            else
            {
                if (currentPhase == tutorialPhase.grabArmFromConveyor)
                    Debug.Log("grabArmFromConveyor");
            }


            // Finish the tutorial
            if (currentPhase == tutorialPhase.tutorialDone)
            {
                duringTutorial = false;
                EndTutorial();
                yield break;
            }

            // Show the next tutorial element
            ShowTutorialItem(currentPhase);
        }
    }

    RobotBody firstRobotBody;
    RobotBody secondRobotBody;

    void SpawnFirstRobot()
    {
        firstRobotBody = Instantiate(bodyRobotPrefab, tutorialRobotSpawn_Left.position, Quaternion.identity).GetComponent<RobotBody>();
        Instantiate(repairedArmPrefab, tutorialRobotSpawn_Left.position, Quaternion.identity);
        Instantiate(repairedWheelPrefab, tutorialRobotSpawn_Left.position, Quaternion.identity);
        Instantiate(outlineArmPrefab, tutorialRobotSpawn_Left.position, Quaternion.identity);
    }

    void SpawnSecondRobot()
    {
        leftRobotRail.functional = true;
        wheelSpawner.functional = true;
        armSpawner.functional = false;

        secondRobotBody = Instantiate(bodyRobotPrefab, tutorialRobotSpawn_Right.position, Quaternion.identity).GetComponent<RobotBody>();
        Instantiate(repairedArmPrefab, tutorialRobotSpawn_Right.position, Quaternion.identity);
        Instantiate(repairedArmPrefab, tutorialRobotSpawn_Right.position, Quaternion.identity);
        Instantiate(outlineWheelPrefab, tutorialRobotSpawn_Right.position, Quaternion.identity);
    }


    void ShowTutorialItem(tutorialPhase phase)
    {
        // Find the adecuate tutorial element for the phase
        TutorialItem tutorialItem = Array.Find(tutorialItems, tutorialItem => tutorialItem.phase == phase);
        StartCoroutine(tutorialItem.Show());
    }

    void HideTutorialItem(tutorialPhase phase)
    {
        // Find the adecuate tutorial element for the phase
        TutorialItem tutorialItem = Array.Find(tutorialItems, tutorialItem => tutorialItem.phase == phase);
        StartCoroutine(tutorialItem.Hide());
    }

    void EndTutorial()
    {
        duringTutorial = false;

        leftRobotRail.functional = true;
        rightRobotRail.functional = true;
        armSpawner.functional = true;
        wheelSpawner.functional = true;

        foreach (var spawner in leftRobotSpawner)
            spawner.functional = true;

        foreach (var spawner in rightRobotSpawner)
            spawner.functional = true;
    }
}
