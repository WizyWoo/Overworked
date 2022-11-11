using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CoingaugeController : MonoBehaviour
{
    public GameObject levelManager, Coingauge_Part_2, Coingauge_Part_3;
    private int FilledCoinGauge;

    // Start is called before the first frame update
    void Start()
    {
        levelManager = GameObject.Find("LevelManager");

        if(SceneManager.GetActiveScene().name == "Level_01")
        {
            gameObject.GetComponent<CoinGauge>().maxMoney = levelManager.GetComponent<Level01_Manager>().moneyToWin1Star1;
            Coingauge_Part_2.GetComponent<CoinGauge>().minMoney = levelManager.GetComponent<Level01_Manager>().moneyToWin1Star1;
            Coingauge_Part_2.GetComponent<CoinGauge>().maxMoney = levelManager.GetComponent<Level01_Manager>().moneyToWin2Star1;
            Coingauge_Part_3.GetComponent<CoinGauge>().minMoney = levelManager.GetComponent<Level01_Manager>().moneyToWin2Star1;
            Coingauge_Part_3.GetComponent<CoinGauge>().maxMoney = levelManager.GetComponent<Level01_Manager>().moneyToWin3Star1;
        }
        if(SceneManager.GetActiveScene().name == "Level_03")
        {
            gameObject.GetComponent<CoinGauge>().maxMoney = levelManager.GetComponent<Level02_Manager>().moneyToWin1Star1;
            Coingauge_Part_2.GetComponent<CoinGauge>().minMoney = levelManager.GetComponent<Level02_Manager>().moneyToWin1Star1;
            Coingauge_Part_2.GetComponent<CoinGauge>().maxMoney = levelManager.GetComponent<Level02_Manager>().moneyToWin2Star1;
            Coingauge_Part_3.GetComponent<CoinGauge>().minMoney = levelManager.GetComponent<Level02_Manager>().moneyToWin2Star1;
            Coingauge_Part_3.GetComponent<CoinGauge>().maxMoney = levelManager.GetComponent<Level02_Manager>().moneyToWin3Star1; 
        }
       if(SceneManager.GetActiveScene().name == "Level_04")
        {
            gameObject.GetComponent<CoinGauge>().maxMoney = levelManager.GetComponent<Level04_Manager>().moneyToWin1Star1;
            Coingauge_Part_2.GetComponent<CoinGauge>().minMoney = levelManager.GetComponent<Level04_Manager>().moneyToWin1Star1;
            Coingauge_Part_2.GetComponent<CoinGauge>().maxMoney = levelManager.GetComponent<Level04_Manager>().moneyToWin2Star1;
            Coingauge_Part_3.GetComponent<CoinGauge>().minMoney = levelManager.GetComponent<Level04_Manager>().moneyToWin2Star1;
            Coingauge_Part_3.GetComponent<CoinGauge>().maxMoney = levelManager.GetComponent<Level04_Manager>().moneyToWin3Star1;
        }
    }

    // Update is called once per frame
    void Update()
    {   if(Coingauge_Part_3.GetComponent<CoinGauge>().Full == true)
        {
            FilledCoinGauge = 3;
            goto LazySkip;
        }
        if(Coingauge_Part_2.GetComponent<CoinGauge>().Full == true)
        {
            FilledCoinGauge = 2;
            goto LazySkip;
        }
        
        if(gameObject.GetComponent<CoinGauge>().Full == true)
        {
            FilledCoinGauge = 1;
        }
        LazySkip:;
        switch (FilledCoinGauge)
        {
            case 0:
                Coingauge_Part_2.SetActive(false);
                Coingauge_Part_3.SetActive(false);
                break;

                case 1:
                Coingauge_Part_2.SetActive(true);
                    break;
            case 2:
                Coingauge_Part_3.SetActive(true);
                break;

        }
    }
}
