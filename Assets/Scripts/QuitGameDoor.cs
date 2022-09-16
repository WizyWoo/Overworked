using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGameDoor : MonoBehaviour , IInteractable
{

    public GameObject QuitGamePanel;

    public void Activate(Transform _player = null, bool _buttonDown = true)
    {

        QuitGamePanel.SetActive(true);

    }

    public void QuitGame()
    {

        Application.Quit();

    }

}
