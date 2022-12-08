using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class player_can_press : MonoBehaviour
{
    public GameObject Exit_panel;

    public Button B;
    public Color default_color;

    public bool test_interaction;
    private bool null_value = false;


    private void Start()
    {
        B = GetComponent<Button>();
        default_color = B.image.color;

        if (Exit_panel == null) null_value = true;      
    }

    private void Update()
    {
        Debug.Log(Exit_panel.activeSelf);
        Debug.Log(null_value);

        if (null_value)
        { clickable(); return; }

        if (Exit_panel.activeSelf == false)
        { gray_out(); return; }


        clickable();


        void gray_out()
        {
            B.interactable = false;
            B.image.color = Color.gray;
        }
        void clickable()
        {
            B.interactable = true;
            B.image.color = default_color;
        }
    }




}
