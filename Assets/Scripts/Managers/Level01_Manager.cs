using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class Level01_Manager : LevelManager
{
    [Header("\n" + "\n" + "PRIVATE VARIABLES OF THIS LEVEL")]

    [Header(stringLine + "RobotRails" + stringLine + "\n")]

    [SerializeField] float slow_robotRailSpeed;
    [SerializeField] float fast_robotRailSpeed;
    [Header("References")]
    [SerializeField] RobotRail[] slow_robotRail;
    [SerializeField] RobotRail[] fast_robotRail;

    [Header(stringLine + "Spawners" + stringLine + "\n")]
    [SerializeField] float robotSpawner_rate;
    [Header("References")]
    [SerializeField] ItemSpawner left_RobotSpawner;
    [SerializeField] ItemSpawner right_RobotSpawner;

    [Header("\n" + stringLine + stringLine + stringLine + "\n")]

    [Header("This Level References")]


    [SerializeField] GameObject middleFloor;

    [SerializeField] GameObject relaxingZone_1Players;
    [SerializeField] GameObject[] relaxingZones_2Players;


    protected override void Awake()
    {
        base.Awake();

        // Setup level for 1 or 2 players
        if (GameManager.instance.onlyOnePlayer)
        {
            middleFloor.SetActive(true);

            relaxingZone_1Players.SetActive(true);
            foreach (GameObject relaxZone in relaxingZones_2Players)
                relaxZone.SetActive(false);
        }
        else
        {
            middleFloor.SetActive(false);

            relaxingZone_1Players.SetActive(false);
            foreach (GameObject relaxZone in relaxingZones_2Players)
                relaxZone.SetActive(true);
        }



        // ADJUST LEVEL DIFFICULTY VARIABLES

        for (int i = 0; i < slow_robotRail.Length; i++)
            slow_robotRail[i].speed = slow_robotRailSpeed;

        for (int i = 0; i < fast_robotRail.Length; i++)
            fast_robotRail[i].speed = fast_robotRailSpeed;


        // SPAWNERS
        // Robot Spawners
        ItemSpawner[] leftRobotSpawners = left_RobotSpawner.GetComponentsInChildren<ItemSpawner>();
        foreach (ItemSpawner spawner in leftRobotSpawners)
        {
            spawner.repeatRate = robotSpawner_rate;
            spawner.initialOffset = 0;
        }

        ItemSpawner[] rightRobotSpawners = right_RobotSpawner.GetComponentsInChildren<ItemSpawner>();
        foreach (ItemSpawner spawner in rightRobotSpawners)
        {
            spawner.repeatRate = robotSpawner_rate;
            spawner.initialOffset = (int)robotSpawner_rate / 2;
        }
    } 
}
