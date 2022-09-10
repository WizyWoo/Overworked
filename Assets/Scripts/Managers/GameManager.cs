using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    static public GameManager instance;

    // Returns true at the end of a game if the players won the level
    public bool playersWon;

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
    public void LoadResultsScene(bool win)
    {
        playersWon = win;
        LoadScene("ResultsScene");
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    //IEnumerator ChangeScene_IEnum()
    //{
    //    yield return new WaitForSeconds(1);
    //}
}
