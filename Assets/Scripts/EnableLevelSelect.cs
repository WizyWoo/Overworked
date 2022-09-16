using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableLevelSelect : MonoBehaviour , IInteractable
{

    public GameObject LevelSelectPanel;

    public void Activate(Transform _player = null, bool _buttonDown = true)
    {

        LevelSelectPanel.SetActive(true);

    }
    
}
