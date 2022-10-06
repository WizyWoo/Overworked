using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricStationHazard : MonoBehaviour
{
    public GameObject ElectricityFX;
    [SerializeField, Tooltip("The minimum and maximum time before wires electrify")]
    private float minTime, maxTime;
    [SerializeField, Tooltip("How long wires electrify for")]
    private float electrifyTime;

    private void Start()
    {

        Invoke(nameof(Electrify), Random.Range(minTime, maxTime));

    }

    private void Electrify()
    {

        ElectricityFX.SetActive(true);

        Invoke(nameof(ShortCircuit), electrifyTime);

    }

    private void ShortCircuit()
    {

        ElectricityFX.SetActive(false);

        Invoke(nameof(Electrify), Random.Range(minTime, maxTime));

    }

}
