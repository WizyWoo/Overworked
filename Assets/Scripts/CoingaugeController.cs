using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CoingaugeController : MonoBehaviour
{
    public GameObject levelManager, Coingauge_Part_2, Coingauge_Part_3;
    

    // Start is called before the first frame update
    void Start()
    {if(SceneManager.GetActiveScene().name == "Level_03")
        {
            levelManager.GetComponent<Level02_Manager>().moneyToWin1Star1 = gameObject.GetComponent<CoinGauge>().maxMoney ;
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
