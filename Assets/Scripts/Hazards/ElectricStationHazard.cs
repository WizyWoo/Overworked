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
    public GameObject ElectricityFX, PlayerElectrocutionFX, warningFX;
    public Collider HazardArea;
    [SerializeField, Tooltip("The minimum and maximum time before wires electrify")]
    private float minTime, maxTime;
    [SerializeField, Tooltip("How long wires electrify for")]
    private float electrifyTime;
    public float RolledTime, warningTime;
    private void Start()
    {

        StartCoroutine(nameof(Electrify));

    }

    private IEnumerator Electrify()
    {
        RolledTime = Random.Range(minTime, maxTime);
        warningTime = RolledTime - 1;
        yield return new WaitForSeconds(warningTime);
        warningFX.SetActive(true);
        yield return new WaitForSeconds(1);
        
        SoundManager.Instance.PlaySound(ElectrifySound, gameObject, SoundManager.SoundType.Loop);

        yield return new WaitForSeconds(1);

        ElectricityFX.SetActive(true);
        HazardArea.enabled = true;

        Invoke(nameof(ShortCircuit), electrifyTime);

    }

    private void ShortCircuit()
    {

        ElectricityFX.SetActive(false);
        warningFX.SetActive(false);
        HazardArea.enabled = false;

        SoundManager.Instance.StopSound(ElectrifySound, gameObject);

        StartCoroutine(nameof(Electrify));

    }

    private void OnTriggerEnter(Collider _col)
    {

        if(_col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {

            CancelInvoke();
            StopAllCoroutines();

            ElectrocutedPlayer(_col.gameObject);
            ShortCircuit();

        }

    }

    private void ElectrocutedPlayer(GameObject _player)
    {

        _player.GetComponent<PlayerController>().Electrocuted(StaminaHit);
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
