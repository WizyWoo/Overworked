using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level01_Manager : LevelManager
{
    [Header("This Level Variables")]

    [SerializeField] Image[] strikes;

    int correctRobots, incorrectRobots;

    private void Start()
    {
        foreach (Image strike in strikes)
            strike.enabled = false;
    }

    public void CorrectRobot()
    {
        Debug.Log("Correct Robot");
        correctRobots++;
    }

    public void IncorrectRobot()
    {
        Debug.Log("Incorrect Robot");
        incorrectRobots++;

        // Update UI
        strikes[incorrectRobots - 1].enabled = true;

        if (incorrectRobots >= 3)
            Lose();
    }
}
