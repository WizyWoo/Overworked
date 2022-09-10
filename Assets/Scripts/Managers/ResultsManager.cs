using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultsManager : MonoBehaviour
{
    [SerializeField] Transform winScreen;
    [SerializeField] Transform loseScreen;

    [SerializeField] Button nextLevel_Bt;
    [SerializeField] Button retry_Bt;
    [SerializeField] Button mainMenu_Bt;

    private void Awake()
    {
        Debug.Log("RESULTS = " + GameManager.instance.playersWon);

        // Hide everything
        winScreen.gameObject.SetActive(false);
        loseScreen.gameObject.SetActive(false);
        Button[] buttons = GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
            button.gameObject.SetActive(false);


        if (GameManager.instance.playersWon)
            SetupWinState();
        else SetupLoseState();
    }

    // It prepares the scene with all the win elements, hiding the lose elements
    void SetupWinState()
    {
        winScreen.gameObject.SetActive(true);

        nextLevel_Bt.gameObject.SetActive(true);
        retry_Bt.gameObject.SetActive(true);
        mainMenu_Bt.gameObject.SetActive(true);
    }

    // It prepares the scene with all the lose elements, hiding the win elements
    void SetupLoseState()
    {
        winScreen.gameObject.SetActive(true);

        retry_Bt.gameObject.SetActive(true);
        mainMenu_Bt.gameObject.SetActive(true);
    }
}
