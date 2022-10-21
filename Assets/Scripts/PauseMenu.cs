using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused;
    bool ShowSettings, ShowTutorial;
    public GameObject PauseMenuPrefab;
    public KeyCode PauseMenuKey;

    //  Gameobject containers for each menu part, 
    //  which is enabled / disabled while "browsing" the menu
    public GameObject SettingsUI, PauseMenuUI, TutorialUI;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(PauseMenuKey))
        {
            isPaused = !isPaused;
        }
        if (isPaused == true)
        {
            Time.timeScale = 0f;

                PauseMenuPrefab.SetActive(true);
            
        }
        if (isPaused == false)
        {
            Time.timeScale = 1;

                PauseMenuPrefab.SetActive(false);
            
        }
    }

    public void ResumeGame() => isPaused = false;

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene("PrototypeHub");
    }

    public void GoToSetting()
    {
        if (!ShowSettings)
        {
            SettingsUI.SetActive(true);
            PauseMenuUI.SetActive(false);
            Debug.Log("Settings menu");
            ShowSettings = true;
        }

        else
        {
            SettingsUI.SetActive(false);
            PauseMenuUI.SetActive(true);
            Debug.Log("Settings menu");
            ShowSettings = false;
        }
    }

    public void RestartLevel() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quited the game");
    }
}
