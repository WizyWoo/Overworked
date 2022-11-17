using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CoingaugeController : MonoBehaviour
{
    public LevelManager levelManager;
    private int filledCoinGauge;
    [SerializeField] CoinGauge coinGauge1, coinGauge2, coinGauge3;

    // Start is called before the first frame update
    void Start()
    {
        coinGauge1.maxMoney = levelManager.moneyToWin1Star;
        coinGauge2.minMoney = levelManager.moneyToWin1Star;
        coinGauge2.maxMoney = levelManager.moneyToWin2Star;
        coinGauge3.minMoney = levelManager.moneyToWin2Star;
        coinGauge3.maxMoney = levelManager.moneyToWin3Star;
    }

    // Update is called once per frame
    void Update()
    {   
        if(coinGauge2.Full == true)
        {
            filledCoinGauge = 2;
        }

        else if (coinGauge1.Full == true)
        {
            filledCoinGauge = 1;
        }
        else filledCoinGauge = 0;

        switch (filledCoinGauge)
        {
            case 0:
                coinGauge2.gameObject.SetActive(false);
                coinGauge3.gameObject.SetActive(false);
                break;

                case 1:
                coinGauge2.gameObject.SetActive(true);
                coinGauge3.gameObject.SetActive(false);
                break;
            case 2:
                coinGauge3.gameObject.SetActive(true);
                break;

        }
    }
}
