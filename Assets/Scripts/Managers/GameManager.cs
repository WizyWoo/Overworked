using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static public GameManager instance;

    // Turns true if the players were overworked, and they lost because of this
    public bool overworked;

    [HideInInspector] public InputDevice[] currentPlayerDevices;
    public string CurrentLevel;
    public bool onlyOnePlayer;
    public bool FakeReset;
    public bool KonamiCode, ArcadeMode, ArcadeModeApp, AMAHardMode, AMAEasyMode; 
    public int finishedMoneyLevel, amountOfStars, minimumMoney, TotalMoney, TotalDebt, TrainPercent, TotalStars, TotalFailures;
    public bool FirstTimeRent, Overtime;
    private void Awake()
    {
        // Singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            CurrentLevel = SceneManager.GetActiveScene().name;
        }

        if(TotalFailures>= 3)
        {
            ArcadeModeApp = true;
            AMAEasyMode = true;
        }
        if(TotalStars >= 24)
        {
            ArcadeModeApp = true;
            AMAHardMode = true;
        }
        if (TotalStars >= 16)
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
        SceneManager.LoadScene(sceneName);
    }

    public void JustStartTheReset()
    {
        StartCoroutine(FakeResetVoid());
    }
    private IEnumerator FakeResetVoid()
    {
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
}
