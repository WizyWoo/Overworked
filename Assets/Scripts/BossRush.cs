using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRush : MonoBehaviour
{
    // If is 2, everything will be twice as fast
    [SerializeField] float speedUpMultiplier;

    // Everything will speed up in X seconds
    [SerializeField] int startSpeedingThingsIn;
    [SerializeField] int duration;

    [SerializeField] ConveyorBelt[] fasterConveyorBelts;
    [SerializeField] ItemSpawner[] FasterSpawners;

    [SerializeField] RobotRail[] fasterRobotRail;


    void Awake()
    {
        //StartCoroutine(StartSpeedingEverything());

        Invoke("StartSpeedingThingsUp", startSpeedingThingsIn);
    }

    void StartSpeedingThingsUp()
    {

    }

    IEnumerator StartSpeedingEverythingIEnumerator()
    {
        SpeedEverything();

        yield return new WaitForSeconds(duration);

        SlowEverything();
    }


    void SpeedEverything()
    {
        foreach (ConveyorBelt conveyorBelt in fasterConveyorBelts)
            conveyorBelt.speed *= speedUpMultiplier;

        //foreach (ConveyorBelt conveyorBelt in fasterConveyorBelts)
        //    conveyorBelt.speed *= speedUpMultiplier;
    }

    void SlowEverything()
    {
        foreach (ConveyorBelt conveyorBelt in fasterConveyorBelts)
            conveyorBelt.speed *= speedUpMultiplier;
    }
}
