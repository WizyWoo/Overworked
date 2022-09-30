using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public class SoundSettingsManager
{

    public static SoundSettings SaveVolumeSettings(SoundSettings _settings = null)
    {

        if(_settings == null)
        {

            _settings = new SoundSettings(1, 1, 1, 1, 1, false);

        }

        string _savePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AudioSettings.poggers");

        BinaryFormatter _formatter = new BinaryFormatter();
        FileStream _stream = new FileStream(_savePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

        _formatter.Serialize(_stream, _settings);
        _stream.Close();

        Debug.Log("Successfully saved");

        return _settings;

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
    
    public static SettingsVer SaveSettingsVersion(int _ver)
    {

        string _savePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SettingsVer.poggers");

        SettingsVer _settingsVer = new SettingsVer(_ver);

        BinaryFormatter _formatter = new BinaryFormatter();
        FileStream _stream = new FileStream(_savePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

        _formatter.Serialize(_stream, _settingsVer);
        _stream.Close();

        Debug.Log("Updated version num");

        return _settingsVer;

    }

    public static SettingsVer LoadSettingsVersion(int _ver)
    {

        string _loadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SettingsVer.poggers");

        if(File.Exists(_loadPath))
        {

            BinaryFormatter _formatter = new BinaryFormatter();
            FileStream _stream = new FileStream(_loadPath, FileMode.Open, FileAccess.Read);

            SettingsVer _settingsVer = _formatter.Deserialize(_stream) as SettingsVer;

            _stream.Close();

            return _settingsVer;

        }
        else
        {

            Debug.Log("No version file found at " + _loadPath + " making a new one...");
            SettingsVer _settingsVer = SaveSettingsVersion(_ver);
            SaveVolumeSettings();
            return _settingsVer;

        }

    }


}
