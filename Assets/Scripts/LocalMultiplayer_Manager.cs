using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LocalMultiplayer_Manager : MonoBehaviour
{
    PlayerInputManager playerInputManager;

    List<PlayerController> allPlayers;

    private void Awake()
    {
        playerInputManager = GetComponent<PlayerInputManager>();

        //playerInputManager.JoinPlayer(1);
    }

    // This function is called everytime a player joined
    public void PlayerJoined(PlayerInput newplayer)
    {
        PlayerController newPlayerController = newplayer.GetComponent<PlayerController>();

        newPlayerController.playerIndex = playerInputManager.playerCount - 1;

        //newPlayerController
    }
}
