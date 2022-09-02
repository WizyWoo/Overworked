using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetLevel : MonoBehaviour
{
    public string CurrentSceneName;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Gets the current Scene name, saves it briefly, then reloads that scene. 
    //attach the void to a button to make it do it's magic.
   public void Restart_Current_Level()
    {
        CurrentSceneName = SceneManager.GetActiveScene().name;
        
        SceneManager.LoadScene(CurrentSceneName);
    }
}
