using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PPFXURP : MonoBehaviour
{

    public int ColorBlindnessType;
    [SerializeField]
    private Material overlayMat;

    private void FixedUpdate()
    {

        UpdateAccesabilitySetting();

    }

    private void UpdateAccesabilitySetting()
    {

        overlayMat.SetInt("type", ColorBlindnessType);

    }

}
