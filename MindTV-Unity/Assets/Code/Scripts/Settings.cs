using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;


/// <summary>
/// UserProfiles is a collection of user settings
/// The settings can be converted to/from Json using ToJson() and LoadFromJson()
/// </summary>
[System.Serializable]
public class Settings
{
    [System.Serializable]
    public class TrainingPrefs
    {
        public int labelNumber;
        public string labelText;
        public string animationText;
        public Color backgroundColor;
    }

    [System.Serializable]
    public class User
    {
        public string userProfileName = "";

        // This could be handled with a getter that allocates new TrainingPrefs, want to 
        // avoid cluttering each view with add/remove logic.
        public List<TrainingPrefs> trainingPrefs = new List<TrainingPrefs>()
        {
            new TrainingPrefs() { labelNumber = 0 },
            new TrainingPrefs() { labelNumber = 1 },
            new TrainingPrefs() { labelNumber = 2 },
            new TrainingPrefs() { labelNumber = 3 },
        };

        // Add other things we need to persist for user here
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
