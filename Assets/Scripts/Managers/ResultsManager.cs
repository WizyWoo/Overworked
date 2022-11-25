using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class ResultsManager : MonoBehaviour
{

    private List<string> cutscenes = new List<string>() 
    { "outside", "test", "BossCoNversation", "Boss_alone", "family_photo" };

    [SerializeField] Transform winScreen;
    [SerializeField] Transform loseScreen;
    [SerializeField] TMP_Text loseText;

    [SerializeField] Button bossConversation_Bt;
    [SerializeField] Button retry_Bt;
    [SerializeField] Button mainMenu_Bt;

    [SerializeField] GameObject stars;

    [SerializeField] TextMeshProUGUI moneyText, minimumMoneyText, neededMoney;

    // Returns true at the end of a game if the players won the level
    public bool playersWon;
    // Returns the level just completed or just failed
    public int levelFinished;

    public bool exhausted;

    private void Start()
    {
        stars.SetActive(false);
    }

    public void Setup()
    {
        Debug.Log("playersWon = " + playersWon);
        Debug.Log("levelFinished = " + levelFinished);

        moneyText.text = GameManager.instance.finishedMoneyLevel.ToString();
        minimumMoneyText.text = GameManager.instance.minimumMoney.ToString();

        GameManager.instance.overworked = false;

        if (playersWon)
            SetupWinState();
        else SetupLoseState(GameManager.instance.overworked);
    }


    // It prepares the scene with all the win elements, hiding the lose elements
    void SetupWinState()
    {
        winScreen.gameObject.SetActive(true);
        loseScreen.gameObject.SetActive(false);

        bossConversation_Bt.gameObject.SetActive(true);
        retry_Bt.gameObject.SetActive(true);
        mainMenu_Bt.gameObject.SetActive(true);

        //set up stars
        stars.SetActive(true);
        for (int i = 0; i < GameManager.instance.amountOfStars; i++)
            stars.transform.GetChild(i).GetComponent<Image>().enabled = true;
    }

    // It prepares the scene with all the lose elements, hiding the win elements
    // There are two ways of losing, being overworked or not making enough money at the end of the level
    void SetupLoseState(bool overworked)
    {
        if (overworked)
            loseText.text = "YOU HAVE BEEN OVERWORKED";

        if(exhausted)
        {
            loseText.text = "YOU HAVE BEEN OVERWORKED";
        }
        else
        {
            loseText.text = "NOT PROFITABLE ENOUGH FOR THE COMPANY";
            neededMoney.gameObject.SetActive(true);
            minimumMoneyText.gameObject.SetActive(true);
        }

        winScreen.gameObject.SetActive(false);
        loseScreen.gameObject.SetActive(true);

        bossConversation_Bt.gameObject.SetActive(false);
        retry_Bt.gameObject.SetActive(true);
        mainMenu_Bt.gameObject.SetActive(true);    

        GameManager.instance.overworked = false;
    }


    // Buttons

    public void Boss_Speech_Bt()
    {
        Debug.Log("GO TALK WITH THE BOSS");

        GameManager.instance.LoadScene(cutscenes[Random.Range(0,cutscenes.Count)]);


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
