using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public class SoundSettingsManager
{

    public static void SaveVolumeSettings(SoundSettings _settings)
    {

        if(_settings == null)
        {

            _settings = new SoundSettings(100, 100, 100, 100, false);

        }

        string _savePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AudioSettings.poggers");

        BinaryFormatter _formatter = new BinaryFormatter();
        FileStream _stream = new FileStream(_savePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

        _formatter.Serialize(_stream, _settings);
        _stream.Close();

    }

    public static SoundSettings LoadVolumeSettings()
    {

        string _loadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AudioSettings.poggers");

        if(File.Exists(_loadPath))
        {

            BinaryFormatter _formatter = new BinaryFormatter();
            FileStream _stream = new FileStream(_loadPath, FileMode.Open, FileAccess.Read);

            SoundSettings _settings = _formatter.Deserialize(_stream) as SoundSettings;

            _stream.Close();

            return _settings;

        }
        else
        {

            Debug.Log("No settings file found at " + _loadPath);
            return null;

        }

    }

}
