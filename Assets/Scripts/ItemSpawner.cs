using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    // Craftable Item
    [SerializeField] GameObject spawnThisPrefab;

    [SerializeField] int repeatRate;

    private void Awake()
    {
        InvokeRepeating("SpawnItem", 0, repeatRate);
    }

    void SpawnItem()
    {
        Instantiate(spawnThisPrefab, null);
    }
}
