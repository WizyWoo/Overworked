using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class BossRush : MonoBehaviour
{
    // If is 2, everything will be twice as fast
    [SerializeField] float speedUpMultiplier;

    // Everything will speed up in X seconds
    [SerializeField] int startSpeedingThingsIn;
    [SerializeField] int duration;

    [SerializeField] string sentence;

    [SerializeField] ConveyorBelt[] fasterConveyorBelts;
    [SerializeField] ItemSpawner[] FasterSpawners;

    [SerializeField] RobotRail[] fasterRobotRail;

    [Header("References")]
    [SerializeField] TMP_Text bossText;
    [SerializeField] Transform dialoguePanel;
    [SerializeField] Transform bossFace;
    [SerializeField] Image redScreenEffect;


    void Awake()
    {
        Invoke("StartSpeedingThingsUp", startSpeedingThingsIn);

        // Set up dialogue elements
        redScreenEffect.color = new Color(redScreenEffect.color.r, redScreenEffect.color.g, redScreenEffect.color.b, 0);
        dialoguePanel.localScale = new Vector3(0, 1, 1);
        bossFace.localScale = new Vector3(0, 0, 1);
    }

    void StartSpeedingThingsUp()
    { StartCoroutine(StartSpeedingEverythingIEnumerator()); }

    IEnumerator StartSpeedingEverythingIEnumerator()
    {
        StartCoroutine(ShowBossDialogue());

        // Red Screen Effect
        redScreenEffect.DOFade(.15f, 1);

        SpeedEverything();

        yield return new WaitForSeconds(duration);

        SlowEverything();

        // Red Screen Effect
        redScreenEffect.DOFade(0, 1);
    }


    IEnumerator ShowBossDialogue()
    {
        #region OpenDialogueAnimation

        float dialogueOpenDuration = 1f;
        float bossFaceOpenDuration = 1f;

        dialoguePanel.transform.DOScaleX(1, dialogueOpenDuration).SetEase(Ease.OutBounce);
        yield return new WaitForSeconds(dialogueOpenDuration);

        bossFace.DOScale(new Vector3(1, 1, 1), bossFaceOpenDuration).SetEase(Ease.OutBounce);
        yield return new WaitForSeconds(bossFaceOpenDuration);

        #endregion


        bossText.text = "";

        foreach (char letter in sentence)
        {
            bossText.text += letter;
            yield return new WaitForSeconds(.05f);
        }
        // Duration of the dialogue in the scene
        yield return new WaitForSeconds(1);

        #region CloseDialogueAnimation

        float bossFaceCloseDuration = 1f;
        float dialogueCloseDuration = 1f;

        bossFace.DOScale(new Vector3(0, 0, 1), bossFaceCloseDuration).SetEase(Ease.OutBounce);
        yield return new WaitForSeconds(bossFaceCloseDuration);

        dialoguePanel.transform.DOScaleX(0, dialogueCloseDuration).SetEase(Ease.OutBounce);
        yield return new WaitForSeconds(dialogueCloseDuration);

        #endregion
    }


    void SpeedEverything()
    {
        foreach (ConveyorBelt conveyorBelt in fasterConveyorBelts)
            conveyorBelt.speed *= speedUpMultiplier;

        foreach (ItemSpawner itemSpawner in FasterSpawners)
            itemSpawner.repeatRate /= speedUpMultiplier;

        foreach (RobotRail robotRail in fasterRobotRail)
            robotRail.speed *= speedUpMultiplier;
    }

    void SlowEverything()
    {
        foreach (ConveyorBelt conveyorBelt in fasterConveyorBelts)
            conveyorBelt.speed /= speedUpMultiplier;

        foreach (ItemSpawner itemSpawner in FasterSpawners)
            itemSpawner.repeatRate *= speedUpMultiplier;

        foreach (RobotRail robotRail in fasterRobotRail)
            robotRail.speed /= speedUpMultiplier;
    }
}
