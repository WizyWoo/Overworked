using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaSpawner : MonoBehaviour
{
    [SerializeField] GameObject lavaPrefab;
    [SerializeField] Vector2 lavaSpawnTimes;

    // Start is called before the first frame update
    void Start()
    {
        float time = Random.Range(lavaSpawnTimes.x, lavaSpawnTimes.y);
        Invoke("LavaSpawn", time);
    }

    void LavaSpawn()
    {
        Instantiate(lavaPrefab, transform.position, Quaternion.identity);
        float time = Random.Range(lavaSpawnTimes.x, lavaSpawnTimes.y);
        Invoke("LavaSpawn", time);
    }
}
