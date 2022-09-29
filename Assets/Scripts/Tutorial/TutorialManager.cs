using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    // Si esta a true, haces el tutorial
    [HideInInspector] public bool doTutorial;

    static TutorialManager instance;
    public static TutorialManager GetInstance()
    { return instance; }

    // Fases del tutorial
    public enum tutorialPhases
    {
        grabArmFromConveyor, throwArm_p1, grabArmFromFloor_p2, craftArm, grabArmFromCraftingTable, 
        throwArm_p2, grabArmFromFloor_p1, assembleArm
    }

    // The currentPhase variable, show what the player must do
    tutorialPhases currentPhase = (tutorialPhases)0;


    // This is where all the information of each tutorial speech is going to be stored
    [SerializeField] TutorialItem[] tutorialElements;


    private void Awake()
    {
        if (instance == null)
            instance = this;

        InizializeTutorialElements();

        //HideEverything();

        //StartCoroutine(DoTutorial());
    }

    void InizializeTutorialElements()
    {
        foreach (TutorialItem tutorialElement in tutorialElements)
        {
            tutorialElement.AssignReferences();
            tutorialElement.HideEverything();
        }
    }
}
