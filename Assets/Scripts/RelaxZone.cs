using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelaxZone : MonoBehaviour
{
    List<PlayerController> playersInRelaxZone = new List<PlayerController>();
    [SerializeField] float relaxSpeed;

    [SerializeField] ParticleSystem particleSys;

    private void Awake()
    {

    }

    private void Update()
    {
        Debug.Log("playersInRelaxZone = " + playersInRelaxZone.Count);

        if (playersInRelaxZone.Count == 0)
        {
            particleSys.Stop();
            return;
        }
        foreach (PlayerController player in playersInRelaxZone)
        {
            player.Relaxing(relaxSpeed);

            if (!particleSys.isPlaying)
                particleSys.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player;
        if (other.TryGetComponent<PlayerController>(out player))
            playersInRelaxZone.Add(player);
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerController player;
        if (other.TryGetComponent<PlayerController>(out player))
            playersInRelaxZone.Remove(player);
    }
}
