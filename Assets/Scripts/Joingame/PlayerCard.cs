using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerCard : MonoBehaviour
{
    [HideInInspector] public int playerIndex;

    [HideInInspector] public JoingameManager joingameManager;

    [SerializeField] TMP_Text playerNumber;

    [SerializeField] Image panelLightColor_Img;
    [SerializeField] Image panelDarkColor_Img;

    [SerializeField] Image device_Img;

    [SerializeField] Image doggo_Img;

    [SerializeField] Sprite gamepadSprite;
    [SerializeField] Sprite keyboardSprite;

    Vector2 gamepadScale = new Vector2(1, 1);
    Vector2 keyboardScale = new Vector2(1.2f, 0.7f);

    private void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        // Update number
        playerNumber.text = "P " + (playerIndex + 1);

        // Update color
        panelLightColor_Img.color = joingameManager.playerColors[playerIndex].dark;
        panelDarkColor_Img.color = joingameManager.playerColors[playerIndex].light;

        // Update Character Image
        doggo_Img.sprite = joingameManager.allPlayerSprites[playerIndex];

        // Update Device Image
        if (IsController())
        {
            device_Img.sprite = gamepadSprite;
            device_Img.transform.localScale = gamepadScale;
        }
        else
        {
            device_Img.sprite = keyboardSprite;
            device_Img.transform.localScale = keyboardScale;
        }
    }

    bool IsController()
    {
        bool isController = false;

        InputDevice inputDevice = joingameManager.allPlayers[playerIndex].inputDevice;
        for (int i = 0; i < Gamepad.all.Count; i++)
        {
            if (inputDevice == Gamepad.all[i])
                isController = true;
        }

        return isController;
    }


    public bool pressingStart;
    public void StartGame(InputAction.CallbackContext context)
    {
        if (context.started)
            pressingStart = true;
        else if (context.canceled)
            pressingStart = false;
    }

    public void Exit(InputAction.CallbackContext context)
    {
        joingameManager.PlayerExit(transform);
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }
}