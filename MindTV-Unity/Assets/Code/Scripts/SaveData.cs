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
        public Color backgroundColor;
    }

    [System.Serializable]
    public struct MIPrefs
    {
        public float windowLength;
        public float interWindowInterval;
        public bool setupRequired;
        public int numberTrainingSelections;
        public int numberTrainingWindows;
        public bool persistantTrainTarget;
        public float pauseTimeBeforeTraining;
        public float trainBreakTime;
        public int numberSelectionsBeforeTraining;
        public int numberSelectionsBetweenTrianing;
        public bool shamFeedback;

    }

    [System.Serializable]
    public struct BciCellPrefs
    {
        public string itemID;
        public bool isVisible;
        public string parent;
        public Vector3 position;
        public bool baseSizeOn;
        public float baseSize;
        public bool outlineOn;
        public int outlineIndex;
        public Color outlineColor;
        public Color backgroundColor;
    }

    [System.Serializable]
    public struct PageObjectPrefs
    {

    }

    [System.Serializable]
    public struct User
    {
        public string userProfileName;
        public GeneralSettingsPrefs generalSettingsPrefs;
        public List<BciCellPrefs> bciCellPrefs;
    }

    [System.Serializable]
    public struct Label
    {
        public string labelData;
    }

    public List<Label> labelDataList;
    public List<User> userProfiles;

    //Saving data from the activity
    [System.Serializable]
    public struct ActivitySettings
    {
        //This is where Activity Setting data will go.

    }


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
