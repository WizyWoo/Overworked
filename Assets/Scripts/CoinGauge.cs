using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using UnityEngine;

public class CoinGauge : MonoBehaviour
{
    public bool Full;
    private bool doOnce;
    public int minMoney, RoughPercent, maxMoney;
    public float currentMoney;
    public float CurrentPercent;

    [SerializeField] LevelManager levelManager;
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
        currentMoney = levelManager.GetComponent<LevelManager>().CoinGaugeMoney;

        if (this.gameObject.name == "CoinGauge_Part_2")
        {
            currentMoney -= minMoney;
            if(doOnce == false)
            {
                maxMoney -= minMoney;
                doOnce = true;
            }
            
        }
        if (this.gameObject.name == "CoinGauge_Part_3")
        {
            currentMoney -= minMoney;
            if (doOnce == false)
            {
                maxMoney -= minMoney;
                doOnce = true;
            }

        }

        CurrentPercent = (currentMoney / maxMoney) * 100;

        RoughPercent = (int)CurrentPercent / 10;
        if (Full && RoughPercent <= 10) Full = false; 

        if (!Full && RoughPercent >= 0)
        {
            if (RoughPercent >= 10)
            {
                Full = true;
                foreach (GameObject gameObject in Percentages)
                {
                    gameObject.SetActive(true);
                }
            }
            else
            {
                for (int i = Percentages.Length - 1 - RoughPercent; i >= RoughPercent; i--)
                {
                    Percentages[i].SetActive(false);
                }
                //Puts everything able
                for (int i = 1; i < RoughPercent + 1; i++)
                {
                    Percentages[RoughPercent - i].SetActive(true);
                }
               
            }
        }
    }
}
