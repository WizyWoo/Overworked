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


    void Awake()
    {

    }

    IEnumerator StartSpeedingEverything()
    {
        yield return new WaitForSeconds(0);
    }

}
