using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class OvertimeText : MonoBehaviour
{
    public TMP_Text Text;
    // Start is called before the first frame update
    void Start()
    {
        Text = gameObject.GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (GameManager.instance.Overtime == true)
        {
            Text.text = "Do Overtime";
        }
    }
}
