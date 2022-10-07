using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class ElectricStationHazard : MonoBehaviour
{

    public EventReference ElectrifySound, PlayerElectrocutedSound;
    [Tooltip("How much stamina the player loses")]
    public float StaminaHit;
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

        SoundManager.Instance.PlaySound(ElectrifySound, gameObject, SoundManager.SoundType.Loop);

        Invoke(nameof(ShortCircuit), electrifyTime);

    }

    private void ShortCircuit()
    {

        ElectricityFX.SetActive(false);
        HazardArea.enabled = false;

        SoundManager.Instance.StopSound(ElectrifySound, gameObject);

        Invoke(nameof(Electrify), Random.Range(minTime, maxTime));

    }

    private void OnTriggerEnter(Collider _col)
    {

        if(_col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {

            Debug.Log("Electrified");
            CancelInvoke();

            ElectrocutedPlayer(_col.gameObject);
            ShortCircuit();

        }

    }

    private void ElectrocutedPlayer(GameObject _player)
    {

        _player.GetComponent<PlayerController>().HitOnStamina(StaminaHit);
        SoundManager.Instance.PlaySound(PlayerElectrocutedSound, gameObject);

    }

}
