using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    // Si esta a true, haces el tutorial
    [HideInInspector] public bool doTutorial;

    [HideInInspector] public bool duringTutorial;

    static TutorialManager instance;
    public static TutorialManager GetInstance()
    { return instance; }

    // Tutorial phases
    public enum tutorialPhase
    {
        grabArmFromConveyor, throwArm_p1, grabArmFromFloor_p2, craftArm, grabArmFromCraftingTable, 
        throwArm_p2, grabArmFromFloor_p1, assembleArm
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
    }

    void ShowTutorialItems(tutorialPhase phase)
    {
        // Find the adecuate tutorial element for the phase
        TutorialItem tutorialItem = Array.Find(tutorialItems, tutorialItem => tutorialItem.phase == phase);
        //tutorialItem.
    }

    void EndTutorial()
    {
        duringTutorial = false;
    }
}
