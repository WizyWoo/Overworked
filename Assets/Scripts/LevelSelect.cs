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
        GameManager.instance.LoadLevel(1);
    }

    public void LoadLevel_1_Solo()
    {
        GameManager.instance.onlyOnePlayer = true;
        GameManager.instance.LoadLevel(4);
    }

    public void LoadLevel_2()
    {
        GameManager.instance.onlyOnePlayer = false;
        GameManager.instance.LoadLevel(2);
    }

    public void LoadLevel_3()
    {
        GameManager.instance.onlyOnePlayer = false;
        GameManager.instance.LoadLevel(3);
    }

}
