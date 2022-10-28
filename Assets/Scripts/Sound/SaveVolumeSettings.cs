using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveVolumeSettings : MonoBehaviour
{
    public void SaveVol()
    {
       SoundManager.Instance.SaveVolumeSettings();
    }
}
