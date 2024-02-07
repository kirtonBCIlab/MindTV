using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UserProfiles is a collection of user settings
/// The settings can be converted to/from Json using ToJson() and LoadFromJson()
/// </summary>
[System.Serializable]
public class UserProfiles
{
    // These are just placeholders, update as needed
    [System.Serializable]
    public struct General
    {
        public float volume;
        public int resolution; // The index of the dropdown
        public bool isFullscreen;
        public Color backgroundColor;
    }

    // These are just placeholders, update as needed
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

    // These are just placeholders, update as needed
    [System.Serializable]
    public struct BciCell
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
    public struct Label
    {
        public string name;
    }

    [System.Serializable]
    public struct User
    {
        public string userProfileName;
        public List<Label> labels;

        // These are just placeholders, update as needed
        public General generalSettingsPrefs;
        public List<BciCell> bciCellPrefs;
    }


    // Set of user profiles
    public List<User> users = new List<User>();


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
