using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    private void Update()
    {
        UpdateTimer();
    }

    void UpdateTimer()
    {
        currentTime -= Time.deltaTime;

        // Update UI
        int minutes =  Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);

        string secondsString;
        if (seconds < 10) secondsString = "0" + seconds;
        else secondsString = seconds.ToString();

        timerText.text = minutes + " : " + secondsString;

        // Check win/lose condition
        if (currentTime <= 0)
        {
            currentTime = 0;

            if (WinCondition())
                Win();
            else
                Lose();
        }
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
    }

    // It is called when the players wins
    protected void Win()
    {
        Debug.Log("WIN THIS");
    }
}
