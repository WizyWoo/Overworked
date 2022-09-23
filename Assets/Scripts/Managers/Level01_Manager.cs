using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class Level01_Manager : LevelManager
{
    [Header("This Level Variables")]

    [SerializeField] Image[] strikes;

    int correctRobots, incorrectRobots;

    [SerializeField] SpriteRenderer tickImage;
    [SerializeField] int moneyToWin1Star1, moneyToWin2Star1, moneyToWin3Star1;


    private void Start()
    {
        foreach (Image strike in strikes)
            strike.enabled = false;

        moneyToWin1Star = moneyToWin1Star1;
        moneyToWin2Star = moneyToWin2Star1;
        moneyToWin3Star = moneyToWin3Star1;

        // Save the tick image position
        initialTickPosition = tickImage.transform.position;

        CorrectRobot();
    }

    public void CorrectRobot()
    {
        UpdateMoney(moneyCorrectRobot*MoneyMultiplier);
        StartCoroutine(ShowGoodFeedback());

        if (MoneyMultiplier >= 3)
        {
            goto JustResetOnce;
        }
        if (MoneyMultiplier > 1)
        {
            MoneyMultiplier = 1;
            MoneyMultiplier++;
            goto JustResetOnce;
        }
        MoneyMultiplier++;
    JustResetOnce:;
    }

    public void IncorrectRobot()
    {
        UpdateMoney(moneyWrongRobot * MoneyMultiplier);
        if(MoneyMultiplier >= 3)
        {
            goto NoMore;
        }
        if (MoneyMultiplier > 1)
        {
            MoneyMultiplier++;
            goto NoMore;
        }
        MoneyMultiplier = 1;
        MoneyMultiplier++;
    NoMore:;
    }


    Vector3 initialTickPosition;
    IEnumerator ShowGoodFeedback()
    {
        float showDuration = 1;

        tickImage.DOFade(1, showDuration);
        tickImage.transform.position = new Vector3(initialTickPosition.x, initialTickPosition.y - 1, initialTickPosition.z);
        tickImage.transform.DOMoveY(initialTickPosition.y, showDuration);
        tickImage.transform.DORotate(new Vector3(75, 0, 360), showDuration, RotateMode.FastBeyond360);

        yield return new WaitForSeconds(showDuration);

        tickImage.transform.DOMoveY(initialTickPosition.y - 1, showDuration);
        tickImage.DOFade(0, showDuration);
        tickImage.transform.DORotate(new Vector3(75, 0, 360), showDuration, RotateMode.FastBeyond360);
    }

    protected override bool WinCondition()
    {
        return money >= moneyToWin1Star;
    }

    
}
