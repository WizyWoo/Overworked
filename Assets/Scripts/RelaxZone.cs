using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelaxZone : MonoBehaviour
{
    List<PlayerController> playersInRelaxZone = new List<PlayerController>();
    [SerializeField] float relaxSpeed;

    private void FixedUpdate()
    {
        Debug.Log("playersInRelaxZone = " + playersInRelaxZone);

        foreach (PlayerController player in playersInRelaxZone)
            player.Relaxing(relaxSpeed);
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
