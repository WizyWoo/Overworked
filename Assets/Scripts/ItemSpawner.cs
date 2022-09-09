using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    // Craftable Item
    [SerializeField] GameObject spawnThisPrefab;

    [SerializeField] int repeatRate;
    [SerializeField] int initialOffset;

    private void Awake()
    {
        InvokeRepeating("SpawnItem", initialOffset, repeatRate);
    }

    void SpawnItem()
    {
        Instantiate(spawnThisPrefab, transform);
    }
}
