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

    [SerializeField] Image panelLightColor;
    [SerializeField] Image panelDarkColor;

    [SerializeField] Image device;

    Vector2 controllerScale = new Vector2(1, 1);
    Vector2 keyboardScale = new Vector2(1.2f, 0.7f);

    InputDevice currentInputDevice;

    private void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        // Update number
        playerNumber.text = "P " + (playerIndex + 1);

        // Update color
        panelLightColor.color = joingameManager.playerColors[playerIndex].dark;
        panelDarkColor.color = joingameManager.playerColors[playerIndex].light;

        // Update the current device

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