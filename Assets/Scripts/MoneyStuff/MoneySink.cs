using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MoneySink : MonoBehaviour
{
    public float Money, Tax, Debt, Rent, LeftoverMoney, bonusDebt;
    private Text MoneyText, TaxText, DebtText, RentText, LeftoverMoneyText;
    public bool PayPal;
    
    // Start is called before the first frame update
    void Start()
    {
       MoneyText = GameObject.Find("Money.txt").GetComponent<Text>();
        TaxText = GameObject.Find("Tax").GetComponent<Text>(); 
        DebtText = GameObject.Find("Debt").GetComponent<Text>();    
        RentText = GameObject.Find("Rent").GetComponent<Text>();
        LeftoverMoneyText = GameObject.Find("LeftoverMoney").GetComponent<Text>();
        
        Money = GameManager.instance.TotalMoney;
        if (GameManager.instance.TotalDebt == 0 )
        {
            Debt =  Random.Range(5000, 50000);
            
            GameManager.instance.TotalDebt = ((int)Debt);
            
        }
        
        
        bonusDebt = Debt * 0.01f;
        GameManager.instance.TotalDebt += ((int)bonusDebt);
        Debt = GameManager.instance.TotalDebt;

    }

    // Update is called once per frame
    void Update()
    {
        MoneyText.text = "Money: " + Money;
        Tax = Money * 0.1f;
        TaxText.text = "Tax: " + Tax;
        DebtText.text = "Debt: " + Debt;
        RentText.text = "Rent: " + Rent;
        Rent = Money * 0.69f; //Nice
        if(PayPal == false)
        {
            LeftoverMoney = Money - (Tax + Rent);
        }
       
        LeftoverMoneyText.text = "After Fees: " + LeftoverMoney;
    }
    public void PaidDebt()
    {
        Debt -= LeftoverMoney;
        PayPal = true;
        LeftoverMoney = 0;
    }
}
