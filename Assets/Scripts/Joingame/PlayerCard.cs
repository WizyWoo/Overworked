using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCard : MonoBehaviour
{
    [HideInInspector] public int playerIndex;

    [HideInInspector] public JoingameManager joingameManager;

    [SerializeField] TMP_Text playerNumber;

    [SerializeField] Image panelLightColor;
    [SerializeField] Image panelDarkColor;

    private void Start()
    {
        // Update number
        playerNumber.text = "P " + (playerIndex + 1);

        // Update color
        panelLightColor.color = joingameManager.playerColors[playerIndex].dark;
        panelDarkColor.color = joingameManager.playerColors[playerIndex].light;
    }



}