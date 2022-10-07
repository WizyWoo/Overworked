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


    static TutorialManager instance;
    public static TutorialManager GetInstance()
    { return instance; }

    // Tutorial phases
    public enum tutorialPhase
    {
        grabArmFromConveyor, throwArm_p1, grabArmFromFloor_p2, repairArm, grabArmFromRepairTable,
        throwArm_p2, grabArmFromFloor_p1, assembleArm, assembleOtherRobot, tutorialDone
    }

    tutorialPhase[] tutPhasesMultiplayer =
    {
        tutorialPhase.grabArmFromConveyor, tutorialPhase.throwArm_p1, tutorialPhase.grabArmFromFloor_p2, tutorialPhase.repairArm,
        tutorialPhase.grabArmFromRepairTable, tutorialPhase.throwArm_p2, tutorialPhase.grabArmFromFloor_p1, tutorialPhase.assembleArm,
        tutorialPhase.assembleOtherRobot, tutorialPhase.tutorialDone
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
        // Wait until the previous panel has already
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
                {
                    Instantiate(bodyRobotPrefab, tutorialRobotSpawn_Left.position, Quaternion.identity);
                    Instantiate(repairedArmPrefab, tutorialRobotSpawn_Left.position, Quaternion.identity);
                    Instantiate(repairedWheelPrefab, tutorialRobotSpawn_Left.position, Quaternion.identity);
                }
            }
            else
            {
                if (currentPhase == tutorialPhase.repairArm)
                {
                    Instantiate(bodyRobotPrefab, tutorialRobotSpawn_Left.position, Quaternion.identity);
                    Instantiate(repairedArmPrefab, tutorialRobotSpawn_Left.position, Quaternion.identity);
                    Instantiate(repairedWheelPrefab, tutorialRobotSpawn_Left.position, Quaternion.identity);
                }
            }

            if (currentPhase == tutorialPhase.assembleOtherRobot)
            {
                leftRobotRail.functional = true;
                wheelSpawner.functional = true;
                armSpawner.functional = false;


                Instantiate(bodyRobotPrefab, tutorialRobotSpawn_Right.position, Quaternion.identity);
                Instantiate(repairedArmPrefab, tutorialRobotSpawn_Right.position, Quaternion.identity);
                Instantiate(repairedArmPrefab, tutorialRobotSpawn_Right.position, Quaternion.identity);
            }

            // Finish the tutorial
            else if (currentPhase == tutorialPhase.tutorialDone)
            {
                duringTutorial = false;
                EndTutorial();
                yield break;
            }

            // Show the next tutorial element
            ShowTutorialItem(currentPhase);
        }
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
