using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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

    [SerializeField] Image[] spawnRadialImage;
    [SerializeField] TMP_Text[] spawnText;

    InputDevice[] devices;

    [SerializeField] GameObject playerPrefab;

    private void Start()
    {
        playerInputManager = GetComponent<PlayerInputManager>();

        allPlayers = new List<PlayerController>();

        devices = GameManager.instance.currentPlayerDevices;

        int j = 0;
        for (int i = 0; i < devices.Length; i++)
            if (devices[i] != null) j++;

        Debug.Log("devices = " + j);


        int numberOfDevices = 0;
        for (int i = 0; i < devices.Length; i++)
            if (devices[i] != null)
                numberOfDevices++;

        // For debugging purposes, if no players have been selected from the menu, just let players join manually
        if (numberOfDevices == 0)
            playerInputManager.joinBehavior = PlayerJoinBehavior.JoinPlayersWhenButtonIsPressed;
        else
            playerInputManager.joinBehavior = PlayerJoinBehavior.JoinPlayersManually;

        if (numberOfDevices != 0)
            for (int i = 0; i < devices.Length; i++)
            {
                if (devices[i] == null) continue;

                PlayerController newPlayer = PlayerInput.Instantiate(playerPrefab, pairWithDevice: devices[i]).GetComponent<PlayerController>();

                newPlayer.playerIndex = i;
            }
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
        int spawnPoint = playerIndex % 2;

        float currentTime = seconds;

        spawnpoints[spawnPoint].gameObject.SetActive(true);


        playerTr.GetComponentInChildren<PlayerController>().enabled = false;


        while (currentTime > 0)
        {
            currentTime -= Time.deltaTime;

            spawnText[spawnPoint].text = Mathf.CeilToInt(currentTime).ToString();
            spawnRadialImage[spawnPoint].fillAmount = 1 - (currentTime / 5);

            yield return 0;
        }

        spawnpoints[spawnPoint].gameObject.SetActive(false);

        playerTr.GetComponentInChildren<PlayerController>().enabled = true;

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
                if (player.enabled && player.transform.position.y < fallDistanceBeforeRespawning)
                {
                    Debug.Log("RespawnSystem");

                    LevelManager level01_Manager = FindObjectOfType<LevelManager>();
                    if (level01_Manager != null) level01_Manager.UpdateMoney(level01_Manager.moneyWhenFall);

                    player.enabled = false;
                    StartCoroutine(TeleportPlayerToSpawnPointInXsec(player.transform, player.playerIndex, 5));
                    if (player.itemGrabbed)
                        player.itemGrabbed.UngrabItem();
                    if (player.ItemsInRangeForGrabbing.Count > 0)
                        player.ItemsInRangeForGrabbing.Clear();
                }
            }
        // Update spawnerItem
    }
}
