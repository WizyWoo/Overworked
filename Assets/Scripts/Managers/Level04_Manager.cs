using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level04_Manager : LevelManager
{
    [Header("\n" + "\n" + "PRIVATE VARIABLES OF THIS LEVEL")]

    [Header(stringLine + "RobotRails" + stringLine + "\n")]

    [SerializeField] float slow_conveyorSpeed;
    [SerializeField] float fast_conveyorSpeed;
    [Header("References")]
    [SerializeField] FailSafeConveyor[] slow_conveyor;
    [SerializeField] FailSafeConveyor[] fast_conveyor;


    [Header("This Level Variables")]

    public int moneyToWin1Star1, moneyToWin2Star1, moneyToWin3Star1;

    private void Awake()
    {
        base.Awake();

        // ADJUST LEVEL DIFFICULTY VARIABLES

        for (int i = 0; i < slow_conveyor.Length; i++)
            slow_conveyor[i].transportDirection = slow_conveyorSpeed;

        for (int i = 0; i < fast_conveyor.Length; i++)
            fast_conveyor[i].transportDirection = fast_conveyorSpeed;
    }


    private void Start()
    {
        moneyToWin1Star = moneyToWin1Star1;
        moneyToWin2Star = moneyToWin2Star1;
        moneyToWin3Star = moneyToWin3Star1;
    }

    public void CorrectRobot()
    {
        UpdateMoney(moneyCorrectRobot * MoneyMultiplier);
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
        if (MoneyMultiplier >= 3)
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
