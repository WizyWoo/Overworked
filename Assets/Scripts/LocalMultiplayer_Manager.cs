using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LocalMultiplayer_Manager : MonoBehaviour
{
    PlayerInputManager playerInputManager;

    private void Awake()
    {
        playerInputManager = GetComponent<PlayerInputManager>();

        //playerInputManager.JoinPlayer(1);
    }

    // This function is called everytime a player joined
    void PlayerJoined()
    {

    }
}
