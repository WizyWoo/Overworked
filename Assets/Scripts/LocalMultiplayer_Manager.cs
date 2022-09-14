using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LocalMultiplayer_Manager : MonoBehaviour
{
    PlayerInputManager playerInputManager;

    // A list with all the players
    List<PlayerController> allPlayers;

    [Header("Parameters")]
    // If the player is below this Y position, respawn him
    [SerializeField] float fallDistanceBeforeRespawning;

    // Empty object that contains all the players as childs
    [SerializeField] Transform playerContainer;

    // Half of players are in the first spawnpoint, other half in second
    [SerializeField] Transform[] spawnpoints;

    private void Awake()
    {
        playerInputManager = GetComponent<PlayerInputManager>();

        allPlayers = new List<PlayerController>();
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

        allPlayers.Add(newPlayerController);

        StartCoroutine(TeleportPlayerToSpawnPointInXsec(newPlayerController.transform, playerIndex, 0));
    }

    IEnumerator TeleportPlayerToSpawnPointInXsec(Transform playerTr, int playerIndex, float seconds)
    {
        yield return new WaitForSeconds(seconds);

        playerTr.gameObject.SetActive(true);
        playerTr.position = spawnpoints[playerIndex % 2].position;
    }


    private void Update()
    {
        RespawnSystem();
    }

    void RespawnSystem()
    {
        if (allPlayers.Count != 0)
            foreach (PlayerController player in allPlayers)
            {
                if (player.gameObject.activeSelf && player.transform.position.y < fallDistanceBeforeRespawning) 
                {
                    player.gameObject.SetActive(false);
                    StartCoroutine(TeleportPlayerToSpawnPointInXsec(player.transform, player.playerIndex, 5));
                }
            }
    }


}
