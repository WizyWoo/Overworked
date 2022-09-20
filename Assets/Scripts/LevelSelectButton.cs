using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelSelectButton : MonoBehaviour
{
    public int LeveltoLoad;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LoadLevel()
    {
        SceneManager.LoadScene(LeveltoLoad);
    }
}
