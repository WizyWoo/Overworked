[System.Serializable]
public class SoundSettings
{

    public float AmbianceVolume, MusicVolume, SFXVolume, UIVolume, MasterVolume;
    public bool Mute;

    public SoundSettings(float _ambiance, float _music, float _sFX , float _uI, float _master, bool _mute)
    {

        AmbianceVolume = _ambiance;
        MusicVolume = _music;
        SFXVolume = _sFX;
        UIVolume = _uI;
        MasterVolume = _master;

    }

}