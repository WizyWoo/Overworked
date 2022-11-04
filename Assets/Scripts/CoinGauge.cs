using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using UnityEngine;

public class CoinGauge : MonoBehaviour
{
    public bool Full;
    public int minMoney,currentMoney,RoughPercent, maxMoney;
    private float CurrentPercent;

    public GameObject[] Percentages;
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject percent in Percentages)
        {
            percent.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        CurrentPercent = (currentMoney/maxMoney) * 100 ;
        
        if (CurrentPercent < 10) RoughPercent = 0;
        if (CurrentPercent > 10) RoughPercent = 1;
        if(CurrentPercent > 20) RoughPercent = 2;
        if(CurrentPercent > 30) RoughPercent = 3;
        if (CurrentPercent > 40) RoughPercent = 4;
        if (CurrentPercent > 50) RoughPercent = 5;
        if (CurrentPercent > 60) RoughPercent = 6;
        if (CurrentPercent > 70) RoughPercent = 7;
        if (CurrentPercent > 80) RoughPercent = 8;
        if (CurrentPercent > 90) RoughPercent = 9;
        if (CurrentPercent > 100) RoughPercent = 10;

        switch (RoughPercent)
        {
            case 0:
                foreach (GameObject percent in Percentages)
                {
                    percent.SetActive(false);
                }
                break;
                case 1:
                Percentages[RoughPercent - 1].SetActive(true);
                break;
                case 2:
                Percentages[RoughPercent - 1].SetActive(true);
                break;
                case 3:
                Percentages[RoughPercent - 1].SetActive(true);
                break;
                case 4:
                Percentages[RoughPercent - 1].SetActive(true);
                break;
                case 5:
                Percentages[RoughPercent - 1].SetActive(true);
                break;
                case 6:
                Percentages[RoughPercent - 1].SetActive(true);
                break;
                case 7:
                Percentages[RoughPercent - 1].SetActive(true);
                break;
                case 8:
                Percentages[RoughPercent - 1].SetActive(true);
                break;
                case 9:
                Percentages[RoughPercent - 1].SetActive(true);
                break;
                case 10:
                Percentages[RoughPercent - 1].SetActive(true);
                break;
        }


    }
}
