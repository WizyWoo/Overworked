using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    static public GameManager instance;
    [SerializeField] Button[] buttons;
    [SerializeField] Animator[] animators;
    // Turns true if the players were overworked, and they lost because of this
    public bool overworked;

    [HideInInspector] public InputDevice[] currentPlayerDevices;
    public string CurrentLevel;
    public bool onlyOnePlayer;
    public bool FakeReset, UnlockEverything;
    public bool KonamiCode, ArcadeMode, ArcadeModeApp, AMAHardMode, AMAEasyMode, Easy,Hard; 
    public int finishedMoneyLevel, amountOfStars, minimumMoney, TotalMoney, TotalDebt, TrainPercent, TotalStars, TotalLoss;
    public bool FirstTimeRent, Overtime;
    static private int levelNumberPlaying = 0;
    static private bool activateNextButton = false;
    public FMODUnity.EventReference unlockLevelSound, levelClick, genericClick, backFromMenu;
    private void Awake()
    {
        // Singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        //Disable every button and activate just the first one
        if (buttons.Length > 0) {
            foreach (Button button in buttons)
            {
                button.enabled = false;
            }

            foreach (Animator animator in animators)
            {
                animator.enabled = false;
            }
            activateNextLevelButton(levelNumberPlaying);
        }
 
    }

    private void Update()
    {
        if(SceneManager.GetActiveScene().name == "MainMenu")
        {
            Time.timeScale = 1;
        }
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            CurrentLevel = SceneManager.GetActiveScene().name;
        }

        if(TotalLoss>= 3|| KonamiCode==true)
        {
            ArcadeModeApp = true;
            AMAEasyMode = true;
        }
        if(TotalStars >= 24 || KonamiCode == true)
        {
            
            AMAHardMode = true;
        }
        if (TotalStars >= 16 || KonamiCode == true)
        {
            ArcadeModeApp = true;
        }
    }

    // At the end of a level this is called by the level manager for loading the results scene
    public void LoadResultsScene(bool win, int level, bool exhausted)
    {
        StartCoroutine(LoadResultsScene_IEnum(win, level, exhausted));
    }

    IEnumerator LoadResultsScene_IEnum(bool win, int level, bool exhausted)
    {
        LoadScene("ResultsScreen");

        // Wait 1 frame
        yield return new WaitForSeconds(0);

        // Assign the necessary variables to the ResultManager of the scene
        ResultsManager resultManager = FindObjectOfType<ResultsManager>();
        resultManager.levelFinished = level;
        resultManager.playersWon = win;
        resultManager.exhausted = exhausted;
        resultManager.Setup();
    }

    public void LoadLevel(int levelNumber)
    {
        // Save the devices that are going to be used in the game.
        if (SceneManager.GetActiveScene().buildIndex == 0)
            JoingameManager.GetInstance().SelectPlayers();

        string levelNumberString = levelNumber.ToString();
        if (levelNumber <= 9) levelNumberString = "0" + levelNumberString;

        SceneManager.LoadScene("Level_" + levelNumberString);
    }

    public string GetSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }
    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneName);
    }
    public void Easymode()
    {
        Easy = !Easy;
        Hard = false;
    }
    public void Hardmode()
    {
        Easy = false;
        Hard = !Hard;
    }
    public void Arcademode()
    {
        ArcadeMode = !ArcadeMode;
    }
    public void JustStartTheReset()
    {
        
        StartCoroutine(FakeResetVoid());
    }
    private IEnumerator FakeResetVoid()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
        StartCoroutine(ResetPart2());
        yield return new WaitForSeconds(0);
        
    }
    private IEnumerator ResetPart2()
    {
        SceneManager.LoadScene(CurrentLevel);
        
        yield return new WaitForSeconds(0);
    }
    //IEnumerator LoadScene_IEnum()
    //{
    //    yield return new WaitForSeconds(1);
    //}


    // It is called when all the players have joined and they decide to start playing
    // This method just store the current devices information.
    public void AllPlayersSelected(InputDevice[] currentPlayerDevices_)
    {
        int numDevices = 0;

        if (currentPlayerDevices_ != null)
        {
            currentPlayerDevices = currentPlayerDevices_;

            for (int i = 0; i < currentPlayerDevices.Length; i++)
            {
                if (currentPlayerDevices[i] != null)
                {
                    Debug.Log("currentPlayerDevices_" + i + " = " + currentPlayerDevices[i].name);
                    numDevices++;
                }
            }
        }

        onlyOnePlayer = numDevices == 1;
    }
    public void decideNextLevelNumber()
    {
        string levelName = GetSceneName();
        char levelNumberName = levelName[levelName.Length - 1];
        //I have to substract 48 because in ASCII 1 in char is kept as 49, 2 as 50....
        if (levelNumberPlaying < levelNumberName - 48)
        {
            activateNextButton = true;
            levelNumberPlaying = levelNumberName - 48;
        }
        else activateNextButton = false;
    }
    private void activateNextLevelButton(int levelNumber)
    {
        //Enable next level button
        for (int i = 0; i < levelNumber + 1; i++)
        {
            if(!buttons[i].isActiveAndEnabled) buttons[i].enabled = true;
        }
        //Disable the locker and black screen of levels unlocked
        for (int i = 1; i < levelNumber; i++)
        {
            if (animators[i - 1].gameObject.activeSelf)
            {
                animators[i - 1].gameObject.SetActive(false);
            }
        }
        if(levelNumber - 1 >= 0)
        {
            if (activateNextButton) {
                animators[levelNumber - 1].enabled = true;
            }
               
            else
                animators[levelNumber - 1].gameObject.SetActive(false);
        }
    }

    public void reproduceSound(string sound)
    {
        switch (sound)
        {
            case "unlockLevelSound":
                if(activateNextButton)
                    SoundManager.Instance.PlaySound(unlockLevelSound, gameObject);
                break;
            case "levelClickDefault":
                SoundManager.Instance.PlaySound(levelClick, gameObject);
                break;
            case "genericClick":
                SoundManager.Instance.PlaySound(genericClick, gameObject);
                break;
            case "backFromMenu":
                SoundManager.Instance.PlaySound(backFromMenu, gameObject);
                break;
            default:
                break;
        }
    }
}
