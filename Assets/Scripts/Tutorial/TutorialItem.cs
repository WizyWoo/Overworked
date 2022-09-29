using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using TMPro;

[Serializable]
public class TutorialItem
{
    public TutorialManager.tutorialPhase phase;
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


    // Smooth transitions for showing or hiding this tutorial item
    public IEnumerator Show()
    {
        float panel_showTime = .5f;
        float info_showTime = .5f;


        // PANEL
        // Show panel
        panel.DOFade(1, panel_showTime);
        // Position
        Vector3 panelPosition = panel.transform.position;
        panel.transform.position =
            new Vector3(panelPosition.x, panelPosition.y - 1);
        panel.transform.DOMoveY(panelPosition.y, panel_showTime);

        yield return new WaitForSeconds(panel_showTime);


        // INFO IN THE PANEL
        foreach (SpriteRenderer infoImage in infoImages)
            infoImage.DOFade(1, info_showTime);
        foreach (TMP_Text infoText in infoTexts)
            infoText.DOFade(1, info_showTime);
    }

    public void Hide()
    {
        float panel_hideTime = .5f;
        float info_hideTime = .5f;


        // INFO IN THE PANEL
        foreach (SpriteRenderer infoImage in infoImages)
            infoImage.DOFade(0, info_hideTime);
        foreach (TMP_Text infoText in infoTexts)
            infoText.DOFade(0, info_hideTime);

        yield return new WaitForSeconds(info_hideTime);


        // PANEL
        // Show panel
        panel.DOFade(0, panel_hideTime);
        // Position
        Vector3 panelPosition = panel.transform.position;
        panel.transform.DOMoveY(panelPosition.y - 1, panel_hideTime);

        yield return new WaitForSeconds(panel_hideTime);

        panel.transform.position = panelPosition;
    }

    public void HideAllElements()
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
