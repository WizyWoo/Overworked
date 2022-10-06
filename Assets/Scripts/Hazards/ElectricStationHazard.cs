using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricStationHazard : MonoBehaviour
{
    public GameObject ElectricityFX;
    public Collider HazardArea;
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
        HazardArea.enabled = true;

        Invoke(nameof(ShortCircuit), electrifyTime);

    }

    private void ShortCircuit()
    {

        ElectricityFX.SetActive(false);
        HazardArea.enabled = false;

        Invoke(nameof(Electrify), Random.Range(minTime, maxTime));

    }

    private void OnTriggerEnter(Collider _col)
    {

        if(_col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {

            Debug.Log("Electrified");
            CancelInvoke();

            ShortCircuit();

        }

    }

}
