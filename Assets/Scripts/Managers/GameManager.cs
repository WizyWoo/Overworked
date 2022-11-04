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

    public bool onlyOnePlayer;


    public int finishedMoneyLevel, amountOfStars, minimumMoney, TotalMoney, TotalDebt;
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

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
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
