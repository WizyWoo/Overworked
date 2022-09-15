using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    // Craftable Item
    [SerializeField] GameObject spawnThisPrefab;

    [SerializeField] public float repeatRate;
    [SerializeField] int initialOffset;

    private void Awake()
    {
        Invoke("SpawnItem", initialOffset);
    }

    void SpawnItem()
    {
        Instantiate(spawnThisPrefab, transform);

        Invoke("SpawnItem", repeatRate);
    }
}
