using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level04_Manager : LevelManager
{
    [Header("\n" + "\n" + "PRIVATE VARIABLES OF THIS LEVEL")]

    [Header(stringLine + "RobotRails" + stringLine + "\n")]

    [SerializeField] float slow_conveyorSpeed;
    [SerializeField] float fast_conveyorSpeed;
    [Header("References")]
    [SerializeField] FailSafeConveyor[] slow_conveyor;
    [SerializeField] FailSafeConveyor[] fast_conveyor;

    private void Awake()
    {
        base.Awake();

        // ADJUST LEVEL DIFFICULTY VARIABLES

        for (int i = 0; i < slow_conveyor.Length; i++)
            slow_conveyor[i].TransportSpeed = slow_conveyorSpeed;

        for (int i = 0; i < fast_conveyor.Length; i++)
            fast_conveyor[i].TransportSpeed = fast_conveyorSpeed;
    }
}
