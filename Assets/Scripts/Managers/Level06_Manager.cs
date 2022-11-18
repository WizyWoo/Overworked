using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Level06_Manager : LevelManager
{

    [SerializeField]
    private GameObject bridges;

    protected override void Awake()
    {

        base.Awake();

        // Setup level for 1 or 2 players
        if (GameManager.instance.onlyOnePlayer)
            bridges.SetActive(true);
        else
            bridges.SetActive(false);

    }

}