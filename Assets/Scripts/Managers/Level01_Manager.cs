using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Level01_Manager : LevelManager
{
    [Header("This Level Variables")]

    [SerializeField] Image[] strikes;

    [SerializeField] TextMeshProUGUI moneyText, addMoneyText;
    [SerializeField] Image addMoneyImg;

    [SerializeField] Color addColor, subsColor;

    int correctRobots, incorrectRobots;

    [SerializeField] int money = 0, moneyCorrectRobot, moneyWrongRobot, moneyToWin;
    public int moneyWhenFall = 25;

    private void Start()
    {
        foreach (Image strike in strikes)
            strike.enabled = false;

        //addMoneyImg.enabled = false;
        //addMoneyText.enabled = false;
    }

    protected override void Update()
    {
        base.Update();

        //moneyText.text = money.ToString();
    }

    public void CorrectRobot()
    {
        UpdateMoney(moneyCorrectRobot);

        Debug.Log("Correct Robot");
        //correctRobots++;
    }

    public void IncorrectRobot()
    {
        UpdateMoney(moneyWrongRobot);

        Debug.Log("Incorrect Robot");
        //incorrectRobots++;

        // Update UI
        //(strikes[incorrectRobots - 1].enabled = true;

        if (incorrectRobots >= 3)
            Lose();
    }

    public void UpdateMoney(int amount)
    {
        money += amount;
        StartCoroutine(AddMoneyVisuals(amount));
    }

    protected override bool WinCondition()
    {
        return money >= moneyToWin;
    }

    IEnumerator AddMoneyVisuals(int amount)
    {
        addMoneyImg.enabled = true;
        addMoneyText.enabled = true;

        if (amount > 0)
            addMoneyImg.color = addColor;
        else addMoneyImg.color = subsColor;

        addMoneyText.color = new Vector4(addMoneyText.color.r, addMoneyText.color.g, addMoneyText.color.b, 255);

        addMoneyText.text = amount.ToString();

        float iniY = addMoneyImg.rectTransform.position.y;
        float y = iniY;
        float alpha = 255;

        while (y < iniY + 100)
        {
            y += 100 * Time.deltaTime;
            addMoneyImg.rectTransform.position = new Vector3 (addMoneyImg.rectTransform.position.x, y, addMoneyImg.rectTransform.position.z);

            alpha -= 255 * Time.deltaTime;
            addMoneyImg.color = new Vector4(addMoneyImg.color.r, addMoneyImg.color.g, addMoneyImg.color.b, alpha);
            addMoneyText.color = new Vector4(addMoneyText.color.r, addMoneyText.color.g, addMoneyText.color.b, alpha);
            yield return 0;
        }

        addMoneyImg.rectTransform.position = new Vector3(addMoneyImg.rectTransform.position.x, iniY, addMoneyImg.rectTransform.position.z);

        addMoneyImg.enabled = false;
        addMoneyText.enabled = false;
    }
}
