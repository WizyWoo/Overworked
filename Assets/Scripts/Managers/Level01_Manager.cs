using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class Level01_Manager : LevelManager
{
    [Header("\n" + "\n" + "PRIVATE VARIABLES OF THIS LEVEL")]

    [Header(stringLine + "RobotRails" + stringLine + "\n")]

    [SerializeField] float slow_robotRailSpeed;
    [SerializeField] float fast_robotRailSpeed;
    [Header("References")]
    [SerializeField] RobotRail[] slow_robotRail;
    [SerializeField] RobotRail[] fast_robotRail;

    [Header(stringLine + "Spawners" + stringLine + "\n")]
    [SerializeField] float robotSpawner_rate;
    [Header("References")]
    [SerializeField] ItemSpawner left_RobotSpawner;
    [SerializeField] ItemSpawner right_RobotSpawner;

    [Header("\n" + stringLine + stringLine + stringLine + "\n")]

    [Header("This Level References")]

    [SerializeField] SpriteRenderer tickImage;
    [SerializeField] int moneyToWin1Star1, moneyToWin2Star1, moneyToWin3Star1;


    [SerializeField] GameObject middleFloor;

    [SerializeField] GameObject relaxingZone_1Players;
    [SerializeField] GameObject[] relaxingZones_2Players;


    private void Awake()
    {
        base.Awake();

        moneyToWin1Star = moneyToWin1Star1;
        moneyToWin2Star = moneyToWin2Star1;
        moneyToWin3Star = moneyToWin3Star1;

        // Save the tick image position
        initialTickPosition = tickImage.transform.position;

        CorrectRobot();

        // Setup level for 1 or 2 players
        if (GameManager.instance.onlyOnePlayer)
        {
            middleFloor.SetActive(true);

            relaxingZone_1Players.SetActive(true);
            foreach (GameObject relaxZone in relaxingZones_2Players)
                relaxZone.SetActive(false);
        }
        else
        {
            middleFloor.SetActive(false);

            relaxingZone_1Players.SetActive(false);
            foreach (GameObject relaxZone in relaxingZones_2Players)
                relaxZone.SetActive(true);
        }



        // ADJUST LEVEL DIFFICULTY VARIABLES

        for (int i = 0; i < slow_robotRail.Length; i++)
            slow_robotRail[i].speed = slow_robotRailSpeed;

        for (int i = 0; i < fast_robotRail.Length; i++)
            fast_robotRail[i].speed = fast_robotRailSpeed;


        // SPAWNERS
        // Robot Spawners
        ItemSpawner[] leftRobotSpawners = left_RobotSpawner.GetComponentsInChildren<ItemSpawner>();
        foreach (ItemSpawner spawner in leftRobotSpawners)
        {
            spawner.repeatRate = robotSpawner_rate;
            spawner.initialOffset = 0;
        }

        ItemSpawner[] rightRobotSpawners = right_RobotSpawner.GetComponentsInChildren<ItemSpawner>();
        foreach (ItemSpawner spawner in rightRobotSpawners)
        {
            spawner.repeatRate = robotSpawner_rate;
            spawner.initialOffset = (int)robotSpawner_rate / 2;
        }
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
