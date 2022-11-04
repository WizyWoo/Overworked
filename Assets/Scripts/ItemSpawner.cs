using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    // Craftable Item
    [SerializeField] GameObject spawnThisPrefab;

    [SerializeField] public float repeatRate;
    [SerializeField] public int initialOffset;

    [SerializeField] public bool functional = true;

    public Generator generator;

    private void Start()
    {
        Invoke("SpawnItem", initialOffset);
    }

    void SpawnItem()
    {
        if (functional)
            Instantiate(spawnThisPrefab, transform.position, Quaternion.identity);

        if (generator != null)
        {
            if (generator.working) Invoke("SpawnItem", repeatRate);
        }
        else Invoke("SpawnItem", repeatRate);
    }
}
