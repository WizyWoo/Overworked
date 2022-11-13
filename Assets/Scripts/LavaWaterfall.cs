using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaWaterfall : MonoBehaviour
{
    [SerializeField] Transform initalPoint, finalPoint, lavaWaterfall;
    [SerializeField] Vector2 lavaSpawnTimes;

    Rigidbody lavaWaterFallRb;
    bool falling;
    float timeToFall, timeToAppear, timerToFall, timerToAppear;
    // Start is called before the first frame update
    void Start()
    {
        lavaWaterFallRb = lavaWaterfall.GetComponent<Rigidbody>();
        lavaWaterFallRb.useGravity = false;
        falling = false;

        timeToFall = Random.Range(lavaSpawnTimes.x, lavaSpawnTimes.y);
        timeToAppear = Random.Range(lavaSpawnTimes.x, lavaSpawnTimes.y);
    }

    private void Update()
    {
        if (!falling)
        {
            if (lavaWaterfall.gameObject.activeInHierarchy)
            {
                timerToFall += Time.deltaTime;

                if (timerToFall >= timeToFall)
                {
                    Fall();
                }
            }
            else
            {
                timerToAppear += Time.deltaTime;

                if (timerToAppear >= timeToAppear)
                {
                    Appear();
                }
            }
        }
        else
        {
            if (lavaWaterfall.position.y <= finalPoint.position.y)
            {
                falling = false;

                lavaWaterfall.position = initalPoint.position;
                lavaWaterFallRb.useGravity = false;

                lavaWaterfall.gameObject.SetActive(false);
            }
        }
    }

    void Appear()
    {
        Debug.Log("Appear");
        lavaWaterfall.gameObject.SetActive(true);

        timerToAppear = 0;
        timeToAppear = Random.Range(lavaSpawnTimes.x, lavaSpawnTimes.y);
    }

    void Fall()
    {
        Debug.Log("Fall");
        lavaWaterFallRb.useGravity = true;
        falling = true;

        timerToFall = 0;
        timeToFall = Random.Range(lavaSpawnTimes.x, lavaSpawnTimes.y);
    }
}
