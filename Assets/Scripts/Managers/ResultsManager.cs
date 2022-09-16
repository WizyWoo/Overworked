using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class ResultsManager : MonoBehaviour
{
    [SerializeField] Transform winScreen;
    [SerializeField] Transform loseScreen;

    [SerializeField] Button nextLevel_Bt;
    [SerializeField] Button retry_Bt;
    [SerializeField] Button mainMenu_Bt;

    [SerializeField] GameObject stars;

    [SerializeField] TextMeshProUGUI moneyText;

    // Returns true at the end of a game if the players won the level
    public bool playersWon;
    // Returns the level just completed or just failed
    public int levelFinished;

    private void Awake()
    {
    }

    private void Start()
    {
        stars.SetActive(false);
    }

    public void Setup()
    {
        Debug.Log("playersWon = " + playersWon);
        Debug.Log("levelFinished = " + levelFinished);

        moneyText.text = GameManager.instance.finishedMoneyLevel.ToString();

        if (playersWon)
            SetupWinState();
        else SetupLoseState();
    }


    // It prepares the scene with all the win elements, hiding the lose elements
    void SetupWinState()
    {
        winScreen.gameObject.SetActive(true);
        loseScreen.gameObject.SetActive(false);

        nextLevel_Bt.gameObject.SetActive(true);
        retry_Bt.gameObject.SetActive(true);
        mainMenu_Bt.gameObject.SetActive(true);

        //set up stars
        stars.SetActive(true);
        for (int i = 0; i < GameManager.instance.amountOfStars; i++)
        {
            stars.transform.GetChild(i).GetComponent<Image>().enabled = true;
        }
    }

    // It prepares the scene with all the lose elements, hiding the win elements
    void SetupLoseState()
    {
        winScreen.gameObject.SetActive(false);
        loseScreen.gameObject.SetActive(true);

        nextLevel_Bt.gameObject.SetActive(false);
        retry_Bt.gameObject.SetActive(true);
        mainMenu_Bt.gameObject.SetActive(true);
    }


    // Buttons

    public void Overworld_Btn()
    {
        Debug.Log("GO TO OVERWORLD");
    }

    public void Retry_Btn()
    {
        GameManager.instance.LoadLevel(levelFinished);
    }

    public void MainMenu_Btn()
    {
        SceneManager.LoadScene(0);
        Debug.Log("GO TO MAIN MENU");
    }
}
