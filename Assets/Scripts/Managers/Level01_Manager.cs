using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class Level01_Manager : LevelManager
{
    [Header("This Level Variables")]

    [SerializeField] SpriteRenderer tickImage;
    [SerializeField] int moneyToWin1Star1, moneyToWin2Star1, moneyToWin3Star1;


    [SerializeField] GameObject middleFloor;

    [SerializeField] GameObject relaxingZone_1Players;
    [SerializeField] GameObject[] relaxingZones_2Players;

    private void Start()
    {
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
    }

    public void CorrectRobot()
    {
        UpdateMoney(moneyCorrectRobot*MoneyMultiplier);
        StartCoroutine(ShowGoodFeedback());

        MoneyMultiplier++;
    }

    public void IncorrectRobot()
    {
        UpdateMoney(moneyWrongRobot * MoneyMultiplier);
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
