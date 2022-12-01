using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestartbuttonFix : MonoBehaviour
{
    public Button RestartButton;
    // Start is called before the first frame update
    void Start()
    {
        RestartButton = this.GetComponent<Button>();
        RestartButton.onClick.AddListener(TaskOnClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void TaskOnClick()
    {
        GameManager.instance.JustStartTheReset();
    }
}
