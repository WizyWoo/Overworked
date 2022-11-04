using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] TMP_Text timerText;

    [SerializeField] TextMeshProUGUI moneyText, addMoneyText;
    [SerializeField] Image addMoneyImg;
    [SerializeField] Color addColor, subsColor;
    [SerializeField] protected int money = 0, moneyCorrectRobot, moneyWrongRobot;
    public FMODUnity.EventReference levelCompleted;
    protected int moneyToWin1Star, moneyToWin2Star, moneyToWin3Star;
    public int MoneyMultiplier;
    public int moneyWhenFall = 25;



    protected const string stringLine = "  __-_-_-_-_-__  ";
    [Header("\n" + "\n" + "ADJUSTING DIFFICULTY PARAMETERS")]

    [Header(stringLine + "Time" + stringLine)]
    [SerializeField] float maxTime;
    float currentTime;

    [Header(stringLine + "Conveyor Belts" + stringLine + "\n")]
    [SerializeField] float slow_conveyorBeltSpeed;
    [SerializeField] float fast_conveyorBeltSpeed;
    [Header("References")]
    [SerializeField] ConveyorBelt[] slow_conveyorBelts;
    [SerializeField] ConveyorBelt[] fast_conveyorBelts;

    [Header(stringLine + "Spawners" + stringLine + "\n")]
    [SerializeField] float partSpawner_rate;
    [Header("References")]
    [SerializeField] ItemSpawner[] parts_Spawner;

    protected void Awake()
    {
        currentTime = maxTime;
        addMoneyImg.gameObject.SetActive(false);

        // Set up game objects variables
        for (int i = 0; i < slow_conveyorBelts.Length; i++)
            slow_conveyorBelts[i].speed = slow_conveyorBeltSpeed;

        for (int i = 0; i < fast_conveyorBelts.Length; i++)
            fast_conveyorBelts[i].speed = fast_conveyorBeltSpeed;

        // Part Spawners
        foreach (ItemSpawner spawner in parts_Spawner)
            spawner.repeatRate = partSpawner_rate;
    }

    protected virtual void Update()
    {
        UpdateTimer();
        moneyText.text = money.ToString() + " / " + moneyToWin1Star.ToString();
    }


    void UpdateTimer()
    {
        currentTime -= Time.deltaTime;

        // Check win/lose condition
        if (currentTime <= 0)
        {
            currentTime = 0;

            GameManager.instance.finishedMoneyLevel = money;
            GameManager.instance.TotalMoney += money;
            GameManager.instance.minimumMoney = moneyToWin1Star;

            if (WinCondition())
                Win();
            else
                Lose();
        }

        // Update UI
        int minutes =  Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);

        string secondsString;
        if (seconds < 10) secondsString = "0" + seconds;
        else secondsString = seconds.ToString();

        timerText.text = minutes + " : " + secondsString;
    }

    // When the timer ends, each level can have a winCondition that they can override
    protected virtual bool WinCondition()
    {
        return true;
    }
    

    // It is called when the players loses
    private void Lose()
    {
        Debug.Log("LOSE THIS");

        GameManager.instance.LoadResultsScene(false, GetLevel(), false);
    }

    public void LoseExhausted()
    {
        Debug.Log("LOSE THIS");

        GameManager.instance.LoadResultsScene(false, GetLevel(), true);
    }

    // It is called when the players wins
    protected void Win()
    {
        if (money >= moneyToWin3Star) GameManager.instance.amountOfStars = 3;
        else if (money >= moneyToWin2Star) GameManager.instance.amountOfStars = 2;
        else GameManager.instance.amountOfStars = 1;
        SoundManager.Instance.PlaySound(levelCompleted, gameObject);

        Debug.Log("WIN THIS");
        GameManager.instance.LoadResultsScene(true, GetLevel(), false);
    }

    int GetLevel()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        int levelNumber = int.Parse(sceneName[sceneName.Length - 2].ToString()) * 10 
            + int.Parse(sceneName[sceneName.Length - 1].ToString());

        return levelNumber;
    }
    public void UpdateMoney(int amount)
    {
        money += amount;
        if (money <= 0) money = 0;
        StartCoroutine(AddMoneyVisuals(amount));
    }

    IEnumerator AddMoneyVisuals(int amount)
    {
        addMoneyText.text = amount.ToString();

        addMoneyImg.gameObject.SetActive(true);

        if (amount > 0)
            addMoneyImg.color = addColor;
        else addMoneyImg.color = subsColor;

        addMoneyText.color = new Vector4(addMoneyText.color.r, addMoneyText.color.g, addMoneyText.color.b, 255);

        float iniY = addMoneyImg.rectTransform.position.y;
        float y = iniY;
        float alpha = 255;

        while (y < iniY + 100)
        {
            y += 100 * Time.deltaTime;
            addMoneyImg.rectTransform.position = new Vector3(addMoneyImg.rectTransform.position.x, y, addMoneyImg.rectTransform.position.z);

            alpha -= 255 * Time.deltaTime;
            addMoneyImg.color = new Vector4(addMoneyImg.color.r, addMoneyImg.color.g, addMoneyImg.color.b, alpha);
            addMoneyText.color = new Vector4(addMoneyText.color.r, addMoneyText.color.g, addMoneyText.color.b, alpha);
            yield return 0;
        }

        addMoneyImg.rectTransform.position = new Vector3(addMoneyImg.rectTransform.position.x, iniY, addMoneyImg.rectTransform.position.z);

        addMoneyImg.gameObject.SetActive(false);
    }
}
