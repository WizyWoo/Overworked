[System.Serializable]
public class SoundSettings
{

    float AmbianceVolume, MusicVolume, SFXVolume, UIVolume;
    bool Mute;

    public SoundSettings(float _ambiance, float _music, float _sFX , float _uI, bool _mute)
    {

        AmbianceVolume = _ambiance;
        MusicVolume = _music;
        SFXVolume = _sFX;
        UIVolume = _uI;

    }

}