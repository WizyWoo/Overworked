using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class RelaxZone : MonoBehaviour , IInteractable
{
    List<PlayerController> playersInRelaxZone = new List<PlayerController>();
    [SerializeField] float relaxSpeed;

    [SerializeField] ParticleSystem particleSys;
    [SerializeField] EventReference HealingSound, Music;
    public Transform CoffeeCupPoint;
    public GameObject CoffeeCupPrefab;
    public Vector3 Offset;

    private void Awake()
    {

    }

    private void Update()
    {
        if (playersInRelaxZone.Count == 0)
        {
            particleSys.Stop();
            return;
        }
        foreach (PlayerController player in playersInRelaxZone)
        {
            player.Relaxing(relaxSpeed);

            if (!particleSys.isPlaying)
            {
                particleSys.Play();
                SoundManager.Instance.PlaySound(HealingSound, gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player;
        if (other.TryGetComponent<PlayerController>(out player))
        {

            GameObject _newCup = Instantiate(CoffeeCupPrefab, CoffeeCupPoint);
            _newCup.transform.position += Offset;
            CoffeeCupPoint = _newCup.transform;
            playersInRelaxZone.Add(player);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerController player;
        if (other.TryGetComponent<PlayerController>(out player))
            playersInRelaxZone.Remove(player);
    }

    public void Activate(Transform _player = null, bool _buttonDown = true)
    {

        SoundManager.Instance.PlaySound(Music, gameObject);

    }

}
