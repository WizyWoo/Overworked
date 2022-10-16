using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MoneySink : MonoBehaviour
{
    public float Money, Tax, Debt, Rent, LeftoverMoney, bonusDebt;
    private Text MoneyText, TaxText, DebtText, RentText, LeftoverMoneyText;
    
    
    // Start is called before the first frame update
    void Start()
    {

        //we find the text, for each number we want
       MoneyText = GameObject.Find("Money.txt").GetComponent<Text>();
        TaxText = GameObject.Find("Tax").GetComponent<Text>(); 
        DebtText = GameObject.Find("Debt").GetComponent<Text>();    
        RentText = GameObject.Find("Rent").GetComponent<Text>();
        LeftoverMoneyText = GameObject.Find("LeftoverMoney").GetComponent<Text>();
        //we then say money is the same as the total money from the game manager
        Money = GameManager.instance.TotalMoney;
        //then if it is exactly zero, we get a new debt, unlikely to happen during normal play but it is possible.
        if (GameManager.instance.TotalDebt == 0 )
        {
            Debt =  Random.Range(10, 100);
            
            GameManager.instance.TotalDebt = ((int)Debt);
            
        }

        if(GameManager.instance.FirstTimeRent == false)
        {
            Rent = Money *1.01f; 
            GameManager.instance.FirstTimeRent = true;
            goto FirstRent;
        }

        Rent += Money * 0.69f; //Nice
    FirstRent:;
        //we then take 1% of the debt and add it to the total, and further make it crazy huge
        Debt = GameManager.instance.TotalDebt;
        bonusDebt = Debt * 0.01f;
        
        GameManager.instance.TotalDebt += ((int)bonusDebt);
        Debt += bonusDebt;
        //we then update the text here and get numbers for tax, adjust them as needed for balance and to rub it in.
        MoneyText.text = "Money: " + Money;
        Tax = 20;
        TaxText.text = "Tax: " + Tax;
        
        DebtText.text = "Debt: " + Debt;
        LeftoverMoneyText.text = " ";
        
        RentText.text = "Rent: " + Rent;
       
        GameManager.instance.TotalMoney -= ((int)Rent + (int)Tax);
    }

    // Update is called once per frame
    void Update()
    {
       
       if(GameManager.instance.TotalMoney < 0)
        {
            GameManager.instance.Overtime = true;
        }
        
    }
    //we pay off rent here using PayPal, jokes aside it just checks if you have paid the landlord "today"
    //and if not it doesnt remove what's left of your money
  
    
    //Currently unused main leftover money bit moved to main function and done automatically
    /*  public void PaidDebt()
    {
        
        PayPal = true;
        LeftoverMoney = 0;
        GameManager.instance.TotalMoney = 0;
        
    }*/
}
