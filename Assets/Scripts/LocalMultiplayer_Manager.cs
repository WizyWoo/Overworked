using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LocalMultiplayer_Manager : MonoBehaviour
{
    PlayerInputManager playerInputManager;

    // A list with all the players
    List<PlayerController> allPlayers;

    // Empty object that contains all the players as childs
    [SerializeField] Transform playerContainer;

    // Half of players are in the first spawnpoint, other half in second
    [SerializeField] Transform[] spawnpoints;

    private void Awake()
    {
        playerInputManager = GetComponent<PlayerInputManager>();

        //playerInputManager.JoinPlayer(1);
    }

    // This function is called everytime a player joined
    public void PlayerJoined(PlayerInput newplayer)
    {
        // Get a reference
        PlayerController newPlayerController = newplayer.GetComponent<PlayerController>();

        // Set player index
        int playerIndex = playerInputManager.playerCount - 1;
        newPlayerController.playerIndex = playerIndex;
        newPlayerController.transform.SetParent(playerContainer);

       // allPlayers.Add(newPlayerController);

        newPlayerController.transform.position = spawnpoints[playerIndex%2].position;
    }
}
