using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

[Serializable]
public class TutorialItem
{
    public TutorialManager.tutorialPhases phase;
    public GameObject gameObject;
    [HideInInspector] public SpriteRenderer panel;
    [HideInInspector] public SpriteRenderer[] infoImages;
    [HideInInspector] public TMP_Text[] infoTexts;



    // Assign references to all the variables
    public void AssignReferences()
    {
        panel = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();

        infoImages = gameObject.transform.GetChild(1).GetComponentsInChildren<SpriteRenderer>();
        infoTexts = gameObject.transform.GetChild(2).GetComponentsInChildren<TMP_Text>();
    }

    public void HideEverything()
    {
        Color panelColor = panel.color;
        panel.color = new Color(panelColor.r, panelColor.g, panelColor.b, 0);

        foreach (SpriteRenderer infoImage in infoImages)
        {
            Color infoImageColor = infoImage.color;
            infoImage.color = new Color(infoImageColor.r, infoImageColor.g, infoImageColor.b, 0);
        }

        foreach (TMP_Text infoText in infoTexts)
        {
            Color infoTextColor = infoText.color;
            infoText.color = new Color(infoTextColor.r, infoTextColor.g, infoTextColor.b, 0);
        }
    }
}
