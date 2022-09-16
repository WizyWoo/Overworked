using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : MonoBehaviour
{

    [Header("Variables")]
    [SerializeField] float maxTime;
    float currentTime;

    [Header("Referencias")]
    [SerializeField] TMP_Text timerText;

    private void Awake()
    {
        currentTime = maxTime;
    }

    protected virtual void Update()
    {
        UpdateTimer();
    }

    void UpdateTimer()
    {
        currentTime -= Time.deltaTime;

        // Check win/lose condition
        if (currentTime <= 0)
        {
            currentTime = 0;

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
    protected void Lose()
    {
        Debug.Log("LOSE THIS");

        GameManager.instance.LoadResultsScene(false, GetLevel());
    }

    // It is called when the players wins
    protected void Win()
    {
        Debug.Log("WIN THIS");
        GameManager.instance.LoadResultsScene(true, GetLevel());
    }

    int GetLevel()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        int levelNumber = int.Parse(sceneName[sceneName.Length - 2].ToString()) * 10 
            + int.Parse(sceneName[sceneName.Length - 1].ToString());

        return levelNumber;
    }
}
