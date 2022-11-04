using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Level02_Manager : LevelManager
{
    [Header("This Level Variables")]

    public int moneyToWin1Star1, moneyToWin2Star1, moneyToWin3Star1;
    private void Start()
    {
        moneyToWin1Star = moneyToWin1Star1;
        moneyToWin2Star = moneyToWin2Star1;
        moneyToWin3Star = moneyToWin3Star1;
    }

    public override void CorrectRobot()
    {
        UpdateMoney(moneyCorrectRobot * MoneyMultiplier);
        if(MoneyMultiplier >= 3)
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

    public override void IncorrectRobot()
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



    protected override bool WinCondition()
    {
        return money >= moneyToWin1Star;
    }

    
}
