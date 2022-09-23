using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    // Craftable Item
    [SerializeField] GameObject spawnThisPrefab;

    [SerializeField] public float repeatRate;
    [SerializeField] int initialOffset;

    public Generator generator;

    private void Awake()
    {
        Invoke("SpawnItem", initialOffset);
    }

    void SpawnItem()
    {
        Instantiate(spawnThisPrefab, transform.position, transform.rotation);


        if(generator != null)
        {
            if(generator.working) Invoke("SpawnItem", repeatRate);
        }
        else Invoke("SpawnItem", repeatRate);
    }
}
