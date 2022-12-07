using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class at_work : MonoBehaviour
{
    void Update()
    {
        transform.GetChild(0).gameObject.SetActive(is_at_home());
        transform.GetChild(1).gameObject.SetActive(!is_at_home());
    }

    public bool is_at_home() => (SceneManager.GetSceneByName("MainMenu") == SceneManager.GetActiveScene());
}
