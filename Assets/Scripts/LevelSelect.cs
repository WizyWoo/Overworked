using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{

    //public void LoadLevel(int _lvlNum)
    //{
    //    SceneManager.LoadScene(_lvlNum);
    //}

    //public void LoadLevel(int _lvlNum, bool onlyOnePlayer)
    //{
    //    GameManager.instance.onlyOnePlayer = onlyOnePlayer;
    //    SceneManager.LoadScene(_lvlNum);
    //}

    public void LoadLevel_1()
    {
        GameManager.instance.onlyOnePlayer = false;
        SceneManager.LoadScene(3);
    }

    public void LoadLevel_1_Solo()
    {
        GameManager.instance.onlyOnePlayer = true;
        SceneManager.LoadScene(3);
    }

    public void LoadLevel_2()
    {
        GameManager.instance.onlyOnePlayer = false;
        SceneManager.LoadScene(2);
    }


}
