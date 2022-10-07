using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class ElectricStationHazard : MonoBehaviour
{

    public EventReference ElectrifySound, PlayerElectrocutedSound;
    [Tooltip("How much stamina the player loses")]
    public float StaminaHit;
    public float ElectrocutionTime;
    public GameObject ElectricityFX, PlayerElectrocutionFX;
    public Collider HazardArea;
    [SerializeField, Tooltip("The minimum and maximum time before wires electrify")]
    private float minTime, maxTime;
    [SerializeField, Tooltip("How long wires electrify for")]
    private float electrifyTime;

    private void Start()
    {

        StartCoroutine(nameof(Electrify));

    }

    private IEnumerator Electrify()
    {

        yield return new WaitForSeconds(Random.Range(minTime, maxTime));

        SoundManager.Instance.PlaySound(ElectrifySound, gameObject, SoundManager.SoundType.Loop);

        yield return new WaitForSeconds(1);

        ElectricityFX.SetActive(true);
        HazardArea.enabled = true;

        Invoke(nameof(ShortCircuit), electrifyTime);

    }

    private void ShortCircuit()
    {

        ElectricityFX.SetActive(false);
        HazardArea.enabled = false;

        SoundManager.Instance.StopSound(ElectrifySound, gameObject);

        StartCoroutine(nameof(Electrify));

    }

    private void OnTriggerEnter(Collider _col)
    {

        if(_col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {

            Debug.Log("Electrified");
            CancelInvoke();
            StopAllCoroutines();

            ElectrocutedPlayer(_col.gameObject);
            ShortCircuit();

        }

    }

    private void ElectrocutedPlayer(GameObject _player)
    {

        _player.GetComponent<PlayerController>().HitOnStamina(StaminaHit);
        _player.GetComponent<Rigidbody>().isKinematic = true;
        SoundManager.Instance.PlaySound(PlayerElectrocutedSound, gameObject);
        Instantiate(PlayerElectrocutionFX, _player.transform).GetComponent<DestroyAfter>().DestroyTimer = ElectrocutionTime;

        StartCoroutine(nameof(DebuffOff), _player);

    }

    private IEnumerator DebuffOff(GameObject _player)
    {

        yield return new WaitForSeconds(ElectrocutionTime);

        _player.GetComponent<Rigidbody>().isKinematic = false;

    }

}
