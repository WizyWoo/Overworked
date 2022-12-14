using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class JoingameManager : MonoBehaviour
{
    static JoingameManager instance;

    public static JoingameManager GetInstance()
    { return instance; }

    [SerializeField] Transform playerContainer;

    [Serializable]
    public struct PlayerColor
    { public Color light, dark; }
    [SerializeField] public PlayerColor[] playerColors = new PlayerColor[4];


    // Local multiplayer variables
    PlayerInputManager playerInputManager;

    // Start game UI
    [SerializeField] Image holdStartGame_Img;
    [SerializeField] Image holdStartGameBack_Img;
    float holdStartGame_Value = 0;


    [Serializable]
    public struct playerJoined
    {
        public playerJoined(PlayerInput playerInput_, PlayerCard playerCard_, InputDevice inputDevice_)
        {
            playerInput = playerInput_;
            playerCard = playerCard_;
            inputDevice = inputDevice_;
        }

        public PlayerInput playerInput;
        public PlayerCard playerCard;
        public InputDevice inputDevice;
    }

    [SerializeField]
    [HideInInspector] public List<playerJoined> allPlayers;

    [SerializeField] public Sprite[] allPlayerSprites;


    [SerializeField] GameObject exitPanel;

    [SerializeField] TMP_Text joinText;

    [SerializeField] GameObject playerCard;

    // Player sprites on the screen
    [SerializeField] Transform[] players;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        playerInputManager = GetComponent<PlayerInputManager>();
        allPlayers = new List<playerJoined>();

        exitPanel.SetActive(false);

        allPlayers.Clear();

        InputDevice[] devices = GameManager.instance.currentPlayerDevices;

        if (devices != null)
        {
            for (int i = 0; i < devices.Length; i++)
                if (devices[i] != null)
                {
                    Transform newTransform = PlayerInput.Instantiate(playerCard, pairWithDevice: devices[i]).transform;
                    newTransform.localScale = Vector3.one;
                }
        }

        //// No players at first
        //foreach (Transform playerSprite in players)
        //    playerSprite.gameObject.SetActive(false);

        UpdatePlayerSprites();
    }

    #region Join Exit Events

    public void PlayerJoined(PlayerInput newplayer)
    {
        // Asignaciones al script del jugador
        PlayerCard newPlayerCard = newplayer.GetComponent<PlayerCard>();
        newPlayerCard.joingameManager = this;
        newPlayerCard.playerIndex = playerInputManager.playerCount - 1;

        newPlayerCard.transform.SetParent(playerContainer);

        // SpawnAnimation
        newplayer.transform.localScale = Vector3.one;
        newplayer.transform.DOKill();
        //newplayer.transform.DOScale(1, 2).SetEase(Ease.OutElastic);


        allPlayers.Add(new playerJoined(newplayer, newPlayerCard, newplayer.GetDevice<InputDevice>()));

        exitPanel.SetActive(true);

        UpdatePlayerSprites();
        //players[newPlayerCard.playerIndex].gameObject.SetActive(true);
    }

    public void PlayerExit(Transform playerTransform)
    {
        PlayerInput playerInput = playerTransform.GetComponent<PlayerInput>();
        PlayerCard playerCard = playerTransform.GetComponent<PlayerCard>();
        InputDevice inputDevice = playerInput.GetDevice<InputDevice>();

        allPlayers.Remove(new playerJoined(playerInput, playerCard, inputDevice));

        Destroy(playerTransform.gameObject);

        // Change the rest of player indexes and UI gfx
        for (int i = 0; i < allPlayers.Count; i++)
        {
            PlayerCard thisPlayerCard = allPlayers[i].playerCard;

            thisPlayerCard.playerIndex = i;
            thisPlayerCard.UpdateUI();
        }

        if (allPlayers.Count == 0)
            exitPanel.SetActive(false);

        UpdatePlayerSprites();
        //players[playerInput.playerIndex].gameObject.SetActive(false);
    }

    void UpdatePlayerSprites()
    {
        int nPlayers = allPlayers.Count;

        for (int i = 0; i < 4; i++)
        {
            if (i < nPlayers)
                players[i].gameObject.SetActive(true);
            else 
                players[i].gameObject.SetActive(false);
        }

    }

    #endregion

    private void Update()
    {
        //if (SomeonePressingStart())
        //    holdStartGame_Value += Time.deltaTime * .5f;
        //else 
        //    holdStartGame_Value -= Time.deltaTime * .5f;

        //holdStartGame_Value = Math.Clamp(holdStartGame_Value, 0, 1);

        //float currentSize = Mathf.Lerp(1, 1.4f, holdStartGame_Value);
        //holdStartGameBack_Img.transform.localScale = new Vector3(currentSize, currentSize);
        //holdStartGame_Img.transform.localScale = new Vector3(currentSize, currentSize);

        //holdStartGame_Img.fillAmount = holdStartGame_Value;


        //currentPlayerDevices = new InputDevice[4];
        //for (int i = 0; i < allPlayers.Count; i++)
        //    currentPlayerDevices[i] = allPlayers[i].inputDevice;

        //if (holdStartGame_Value >= 1)
        //    GameManager.instance.AllPlayersSelected(currentPlayerDevices);

        if (!playerInputManager.joiningEnabled)
            playerInputManager.EnableJoining();
    }

    public void SelectPlayers()
    {
        // Store all the current being used devices and passing the variable to the gameManager
        InputDevice[] devices = new InputDevice[4];
        for (int i = 0; i < allPlayers.Count; i++)
            devices[i] = allPlayers[i].inputDevice;

        GameManager.instance.AllPlayersSelected(devices);
    }
}