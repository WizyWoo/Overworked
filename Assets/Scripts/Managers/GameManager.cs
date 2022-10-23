using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    static public GameManager instance;

    // Simulates if there is only one player, for tutorial reasons
    [HideInInspector] public bool onlyOnePlayer;

    // Turns true if the players were overworked, and they lost because of this
    public bool overworked;

    InputDevice[] currentPlayerDevices;


    public int finishedMoneyLevel, amountOfStars, minimumMoney, TotalMoney, TotalDebt;
    public bool FirstTimeRent, Overtime;
    private void Awake()
    {
        // Singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else Destroy(this.gameObject);
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
        // Save player count
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            FindObjectOfType<JoingameManager>().SelectPlayers();
        }


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


    // Se llama cuando se han elegido todos los jugadores y se cambia de escena
    // It is called when all the players have joined and they decide to start playing
    public void AllPlayersSelected(InputDevice[] currentPlayerDevices_)
    {
        if (currentPlayerDevices_ != null)
        {
            currentPlayerDevices = currentPlayerDevices_;

            for (int i = 0; i < currentPlayerDevices.Length; i++)
            {
                if (currentPlayerDevices[i] != null)
                    Debug.Log("currentPlayerDevices_" + i + " = " + currentPlayerDevices[i].name);
            }
        }

        LoadScene("Gameplay_Scene");
    }
}
