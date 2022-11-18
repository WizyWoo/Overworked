using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TrainPercentAdjust : MonoBehaviour
{
    public InputField TrainPercent;
    public string TrainText;
    public int TrainValue;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameManager.instance.TrainPercent = TrainValue;
        TrainText = TrainPercent.text;
        TrainValue = int.Parse(TrainText);
    }
    
}
