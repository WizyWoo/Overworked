using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    // Si esta a true, haces el tutorial
    [SerializeField] bool doTutorial;

    [HideInInspector] public bool duringTutorial;

    // Returns true when the tutorial of a panel is hiding 
    [HideInInspector] public bool changingPhase;


    [SerializeField] Transform tutorialRobotSpawn_Left;

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
        throwArm_p2, grabArmFromFloor_p1, assembleArm, robotFinished
    }

    // The currentPhase variable, show what the player must do
    tutorialPhase currentPhase = (tutorialPhase)0;

    // Tutorial elements are gameobjects that store tutorial info for each phase
    [SerializeField] TutorialItem[] tutorialItems;


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
        foreach (TutorialItem tutorialElement in tutorialItems)
        {
            tutorialElement.AssignReferences();
            tutorialElement.HideAllElements();
        }
    }

    void StartTutorial()
    {
        duringTutorial = true;

        // Show first phase
        ShowTutorialItem((tutorialPhase)0);
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

            // Spawn robot body when neccesary
            if (currentPhase == tutorialPhase.assembleArm-1)
            {
                yield return new WaitForSeconds(.3f);
                Debug.Log("Instantiate robot");
                Instantiate(bodyRobotPrefab, tutorialRobotSpawn_Left.position, Quaternion.identity);
                Instantiate(repairedArmPrefab, tutorialRobotSpawn_Left.position, Quaternion.identity);
                Instantiate(repairedWheelPrefab, tutorialRobotSpawn_Left.position, Quaternion.identity);
            }
            currentPhase++;

            changingPhase = true;
            yield return new WaitForSeconds(1);
            changingPhase = false;

            // Finish the tutorial
            if (currentPhase == tutorialPhase.robotFinished)
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
    }
}
