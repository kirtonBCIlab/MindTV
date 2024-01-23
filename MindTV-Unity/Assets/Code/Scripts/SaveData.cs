using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    [System.Serializable]
    public struct GeneralSettingsPrefs
    {
        public float volume;
        public int resolution; // The index of the dropdown
        public bool isFullscreen;
    }

    [System.Serializable]
    public struct BCIStimulusPrefs
    {
        public string itemID;
        public bool isVisible;
        public string parent;
        public Vector3 position;
        public bool baseSizeOn;
        public float baseSize;
        public bool greyscaleOn;
        public bool outlineOn;
        public int outlineIndex;
        public Color outlineColor;
        public bool flashColorOn;
        public int flashColorIndex;
        public Color flashColor;
        public bool textureOn;
        public bool zoomOn;
        public float zoomValue;
    }

    [System.Serializable]
    public struct User
    {
        public string userProfileName;
        public GeneralSettingsPrefs generalSettingsPrefs;
        public List<BCIStimulusPrefs> bciStimulusPrefs;
    }

    [System.Serializable]
    public struct Label
    {
        public string labelData;
    }

    public List<Label> labelDataList;
    public List<User> userProfiles;


    // Convert class to Json string
    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

    // Fill in class from Json string
    public void LoadFromJson(string a_Json)
    {
        JsonUtility.FromJsonOverwrite(a_Json, this);
    }
}

public interface ISaveable
{
    void PopulateSaveData(SaveData a_SaveData);
    void LoadFromSaveData(SaveData a_SaveData);
}