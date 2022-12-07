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
        isPaused = false;
    }

    public void RestartScene()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

 

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(PauseMenuKey))
        {
            isPaused = !isPaused;
        
            if (isPaused) {
                Time.timeScale = 0f;
                PauseMenuPrefab.SetActive(true);
            }
            else {
                Resume();
            }
        }
    }

    public void Resume()
    {
        Time.timeScale = 1;
        GameManager.instance.reproduceSound("backFromMenu");
        PauseMenuPrefab.SetActive(false);
        isPaused = false;
    }

    public void GoToSetting()
    {
        if (!ShowSettings){
            SettingsUI.SetActive(true);
            PauseMenuUI.SetActive(false);
            Debug.Log("Settings menu");
            ShowSettings = true;
        }

        else{      
            SettingsUI.SetActive(false);
            PauseMenuUI.SetActive(true);
            Debug.Log("Settings menu");
            ShowSettings = false;
        }
    }

    public void QuitGame() {
        Application.Quit();
        Debug.Log("Quited the game");
    }
}
