using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaWaterfall : MonoBehaviour
{
    [SerializeField] GameObject lavaBlurp;

    [SerializeField] Vector2 timeForLavaBlurp;
    // Start is called before the first frame update
    void Start()
    {
        float time = Random.Range(timeForLavaBlurp.x, timeForLavaBlurp.y);
        Invoke("LavaBlurp", time);
    }

    void LavaBlurp()
    {
        GameObject clon = Instantiate(lavaBlurp, this.transform);

        float time = Random.Range(timeForLavaBlurp.x, timeForLavaBlurp.y);
        Invoke("LavaBlurp", time);
    }
}
