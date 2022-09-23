using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public bool working;

    [SerializeField] ConveyorBelt[] conveyorBelts;
    [SerializeField] RobotRail[] robotRails;

    float[] beltsInitialSpeeds;
    float[] railsInitialSpeeds;
    // Start is called before the first frame update
    void Start()
    {
        working = true;

        beltsInitialSpeeds = new float[conveyorBelts.Length];
        for (int i = 0; i < conveyorBelts.Length; i++)
        {
            beltsInitialSpeeds[i] = conveyorBelts[i].speed;
        }

        railsInitialSpeeds = new float[robotRails.Length];
        for (int i = 0; i < robotRails.Length; i++)
        {
            railsInitialSpeeds[i] = robotRails[i].speed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q)) SwitchOff();
    }

    public void SwitchOff()
    {
        working = false;

        foreach (ConveyorBelt belt in conveyorBelts)
        {
            belt.speed = 0;
        }

        foreach(RobotRail rail in robotRails)
        {
            rail.speed = 0;
        }
    }

    public void SwitchOn()
    {
        working = true;

        for (int i = 0; i < conveyorBelts.Length; i++)
        {
            conveyorBelts[i].speed = beltsInitialSpeeds[i];
        }

        for (int i = 0; i < robotRails.Length; i++)
        {
            robotRails[i].speed = railsInitialSpeeds[i];
        }
    }
}
