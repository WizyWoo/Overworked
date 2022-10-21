using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JoingameManager : MonoBehaviour
{

    [SerializeField] Transform playerContainer;

    [Serializable]
    public struct PlayerColor
    { public Color light, dark; }
    [SerializeField] public PlayerColor[] playerColors = new PlayerColor[4];


    // Local multiplayer variables
    PlayerInputManager playerInputManager;
    List<PlayerInput> allPlayers;

    private void Awake()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
        allPlayers = new List<PlayerInput>();
    }

    public void PlayerJoined(PlayerInput newplayer)
    {
        // Asignaciones al script del jugador
        PlayerCard newPlayerCard = newplayer.GetComponent<PlayerCard>();
        newPlayerCard.joingameManager = this;
        newPlayerCard.playerIndex = playerInputManager.playerCount - 1;

        newPlayerCard.transform.SetParent(playerContainer);

        // SpawnAnimation
        newplayer.transform.localScale = Vector3.zero;
        newplayer.transform.DOScale(1, 2).SetEase(Ease.OutElastic);

        allPlayers.Add(newplayer);
    }
}