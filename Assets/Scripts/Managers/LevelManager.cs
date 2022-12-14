using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;


public class LevelManager : MonoBehaviour
{
    [Header("REFERENCES")]
    public static LevelManager Instance;
    [Header("FMOD SOUNDS")]
    public FMODUnity.EventReference levelCompleted, timerShaking;
    public bool loseMoney, incrementOnce, StarGainOnce;
    public SimpleFade Fadeout;
    [Header("MONEY STUFF")]
    public int CoinGaugeMoney;
    public int MoneyMultiplier = 1;
    public int visualmoneymultiplier;
    public int moneyWhenFall = 25;
    public int moneyToWin1Star = 30, moneyToWin2Star = 60, moneyToWin3Star = 90;
    [SerializeField] TextMeshProUGUI moneyText, addMoneyText;
    [SerializeField] Image addMoneyImg;
    [SerializeField] Color addColor, subsColor;
    [SerializeField] protected int money = 0, moneyCorrectRobot = 5, moneyWrongRobot = 5;

    [Header("TIMER")]
    [SerializeField] TMP_Text timerText;

    protected const string stringLine = "  __-_-_-_-_-__  ";

    [Header("\n" + "\n" + "ADJUSTING DIFFICULTY PARAMETERS")]
    [Header(stringLine + "Time" + stringLine)]
    [SerializeField] float maxTime;
    [SerializeField] Image topTimerPart, bottomTimerPart;
    [SerializeField] Animation timerEndAnimation, timerShakeAnimation;
    public float currentTime;

    [Header(stringLine + "CONVEYOR BELTS SPEEDS" + stringLine + "\n")]
    [SerializeField] float slow_conveyorBeltSpeed;
    [SerializeField] float fast_conveyorBeltSpeed;

    [Header("CONVEYOR BELTS")]
    [SerializeField] ConveyorBelt[] slow_conveyorBelts;
    [SerializeField] ConveyorBelt[] fast_conveyorBelts;

    [Header(stringLine + "SPAWNERS" + stringLine + "\n")]
    [SerializeField] float partSpawner_rate;

    [Header("REFERENCES")]
    [SerializeField] ItemSpawner[] parts_Spawner;

    [Header("TICK IMAGE")]
    private Vector3 initialTickPosition;
    [SerializeField] SpriteRenderer tickImage;

    private bool decideLevelNumber = true;
    protected virtual void Awake()
    {
        Instance = this;
        Fadeout = gameObject.GetComponent<SimpleFade>();
        if(GameManager.instance.Easy== true)
        {
            currentTime += currentTime/2;
        }
        // Save the tick image position
        if (tickImage != null)
            initialTickPosition = tickImage.transform.position;

        StartCoroutine(ShowGoodFeedback());

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
        //moneyText.text = " ";
       // moneyText.text = money.ToString() + " / " + moneyToWin1Star.ToString();
    }
    public virtual void CorrectRobot()
    {
        UpdateMoney(moneyCorrectRobot);
        if (MoneyMultiplier < 3&& GameManager.instance.Hard == false) MoneyMultiplier++;      
        StartCoroutine(ShowGoodFeedback()); 
    }
    public virtual void IncorrectRobot()
    {
        
        loseMoney = true;
        UpdateMoney(moneyWrongRobot);
    }
    void UpdateTimer()
    {
        currentTime -= Time.deltaTime;

        if((currentTime / maxTime) * 0.49f > 0.004f)
            topTimerPart.fillAmount = (currentTime / maxTime) * 0.49f + 0.51f;
        else    
            timerEndAnimation.Play();
            

        bottomTimerPart.fillAmount = 1 - (currentTime / maxTime);

        if (currentTime <= 10) //More violent shake >:D
        {
            timerShakeAnimation.Play("TimerShakeFaster");
        }
        else if (currentTime <= 30) //hope this means 30 last seconds
        {
            SoundManager.Instance.PlaySound(timerShaking, gameObject, SoundManager.SoundType.Loop);
            timerShakeAnimation.Play();
        }
        // Check win/lose condition
        if (currentTime <= 0)
        {
            SoundManager.Instance.StopSound(timerShaking, gameObject);
            currentTime = 0;

            GameManager.instance.finishedMoneyLevel = money;
            GameManager.instance.TotalMoney += money;
            GameManager.instance.minimumMoney = moneyToWin1Star;

            if (WinCondition())
                Win();
            else
                Lose();
        }

        /*
        // Update UI
        int minutes =  Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);

        string secondsString;
        if (seconds < 10) secondsString = "0" + seconds;
        else secondsString = seconds.ToString();

        timerText.text = minutes + " : " + secondsString;*/
    }
    // When the timer ends, each level can have a winCondition that they can override
    protected virtual bool WinCondition()
    {
        return money >= moneyToWin1Star;
    }
    // It is called when the players loses
    private void Lose()
    {
        Debug.Log("LOSE THIS");
        if(incrementOnce == false)
        {
            GameManager.instance.TotalLoss++;
        incrementOnce = true;
        }
        
        StartCoroutine(Fadeout.FadeAndLoadScene(SimpleFade.FadeDirection.In,  false, false));
        
        //GameManager.instance.LoadResultsScene(false, GetLevel(), false);
    }
    public void LoseExhausted()
    {
        Debug.Log("LOSE THIS");
        if (incrementOnce == false)
        {
            GameManager.instance.TotalLoss++;
            incrementOnce = true;
        }
        StartCoroutine(Fadeout.FadeAndLoadScene(SimpleFade.FadeDirection.In,  false, true));
        
        //GameManager.instance.LoadResultsScene(false, GetLevel(), true);
    }
    // It is called when the players wins
    public void Win()
    {
        if (GameManager.instance.ArcadeMode == true) goto NoStars4u;
        if (money >= moneyToWin3Star)
        {
            
            GameManager.instance.amountOfStars = 3;
            if(StarGainOnce== false)
            {
                GameManager.instance.TotalStars += 3;
                StarGainOnce = true;
            }
            
        }
        else if (money >= moneyToWin2Star)
        {
            GameManager.instance.amountOfStars = 2;
            if(StarGainOnce == false)
            {
                GameManager.instance.TotalStars += 2;
                StarGainOnce = true;
            }
            
        }
        else 
        {
            GameManager.instance.amountOfStars = 1;
            if (StarGainOnce == false)
            {
                GameManager.instance.TotalStars += 1;
                StarGainOnce = true;
            }
        }
    NoStars4u:;
        SoundManager.Instance.PlaySound(levelCompleted, gameObject);
        if (decideLevelNumber) {
            decideLevelNumber = false;
            GameManager.instance.decideNextLevelNumber();
        }
        Debug.Log("WIN THIS");
        StartCoroutine(Fadeout.FadeAndLoadScene(SimpleFade.FadeDirection.In,  true, false));

       // GameManager.instance.LoadResultsScene(true, GetLevel(), false);
    }
    public int GetLevel()
    {
        string sceneName = GameManager.instance.GetSceneName();
        int levelNumber = int.Parse(sceneName[sceneName.Length - 2].ToString()) * 10 
            + int.Parse(sceneName[sceneName.Length - 1].ToString());

        return levelNumber;
    }
    public void UpdateMoney(int amount)
    {
        if(amount < 0 || loseMoney== true)
        {
            MoneyMultiplier = 1;
            money += amount;
            if (GameManager.instance.Easy)
            {
                MoneyMultiplier = 2;
                
            }
           
        
            
            loseMoney = false;
        }
        else money += amount * MoneyMultiplier;

        CoinGaugeMoney = money;
        if(GameManager.instance.ArcadeMode == true)
        {
            currentTime += 2 * MoneyMultiplier;
        }
        
        if (money <= 0) money = 0;
        StartCoroutine(AddMoneyVisuals(amount));
    }
    IEnumerator AddMoneyVisuals(int amount)
    {
        visualmoneymultiplier = amount *MoneyMultiplier;
        addMoneyText.text = visualmoneymultiplier.ToString();

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
    IEnumerator ShowGoodFeedback()
    {
        if (tickImage != null)
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
    }
}
