using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Level02_Manager : LevelManager
{
    [Header("This Level Variables")]

    [SerializeField] Image[] strikes;
    [SerializeField] int moneyToWin1Star1, moneyToWin2Star1, moneyToWin3Star1;
    int correctRobots, incorrectRobots;
    private void Start()
    {
        foreach (Image strike in strikes)
            strike.enabled = false;
        moneyToWin1Star = moneyToWin1Star1;
        moneyToWin2Star = moneyToWin2Star1;
        moneyToWin3Star = moneyToWin3Star1;
    }

    public void CorrectRobot()
    {
        UpdateMoney(moneyCorrectRobot*MoneyMultiplier);
        if(MoneyMultiplier >= 3)
        {
            goto JustResetOnce;
        }
        if (MoneyMultiplier > 0)
        {
            MoneyMultiplier = 0;
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
        if (MoneyMultiplier > 0)
        {
            MoneyMultiplier++;
            goto NoMore;
        }
        MoneyMultiplier = 0;
        MoneyMultiplier++;
    NoMore:;
    }



    protected override bool WinCondition()
    {
        return money >= moneyToWin1Star;
    }

    
}
