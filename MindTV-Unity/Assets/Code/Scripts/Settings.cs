using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEditor;
using Microsoft.Unity.VisualStudio.Editor;
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
        //We should replace this with a "Mental Command" object
        public int labelNumber = 0;
        public string labelName = "";
        public string animationName = "None";
        public string imagePath = "Assets/icons/cube_primary.png";
        public Color backgroundColor = Settings.ColorForName("Blue (Theme)");

        // can't serialize sprite, need to record where asset is (default or user provided)

        public float imageBaseSize = 100.0f;

        // trial length must be a multiple of windowLength
        public float windowLength = 2.0f;
        public float trialLength = 6.0f;
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
        public List<VideoCell> videoCells = new List<VideoCell>()
        {
            new VideoCell() { tileNumber = 0 },
        };

        // Add new cell to the videoCell list
        public VideoCell AddVideoCell()
        {
            var newCell = new VideoCell() { tileNumber = videoCells.Count };
            videoCells.Add(newCell);
            return newCell;
        }

    }

    [System.Serializable]
    public class VideoCell
    {
        public int tileNumber = 0;
        public string videoTitle = "Video Title";
        public string videoPath = "";
        public Color backgroundColor = Settings.ColorForName("Blue (Theme)");
        public bool includeGraphic = false;
        public string layoutStyle = "Default";
        public string mentalCommandLabel = "None";
    }

    public class MentalCommand
    {
        public string labelName = "";
        public string animationName = "";

        public string imagePath = "Assets/icons/cube_primary.png";

        // public Sprite myImage;

        // void Awake
        // {
        //     // Load the sprite from the asset path
        //     myImage = Resources.Load<Sprite>(imagePath);
        // }
    }

    // Set of user profiles
    public List<User> users = new List<User>();


    // Convert colors to strings for dropdowns
    public static Color ColorForName(string name)
    {
        return colors[name];
    }

    public static string NameForColor(Color color)
    {
        string name = "Unknown";
        if (colors.ContainsValue(color))
        {
            name = colors.FirstOrDefault(item => item.Value == color).Key;
        }
        return name;
    }

    private static readonly Dictionary<string, Color> colors = new Dictionary<string, Color>
    {
        {"Red", Color.red },
        {"Blue", Color.blue },
        {"Green", Color.green },
        {"White", Color.white },
        {"Yellow", Color.yellow },
        {"Black", Color.black },
        {"Magenta", Color.magenta},
        {"Grey", Color.grey},
        {"Cyan", Color.cyan},
        {"Blue (Theme)", new Color(0.34117647f, 0.72156863f, 1.0f, 1.0f)},
        {"Green (Theme)", new Color(0.26277451f, 0.6666667f, 0.54509804f, 1.0f)},
        {"Purple (Theme)", new Color(0.3254902f, 0.21960784f, 0.57254902f, 1.0f)}
    };

    public static float TrialLengthForName(string name)
    {
        return trialLength[name];
    }

    public static string NameForTrialLength(float length)
    {
        string name = "Unknown";
        if (trialLength.ContainsValue(length))
        {
            name = trialLength.FirstOrDefault(item => item.Value == length).Key;
        }
        return name;
    }

    private static readonly Dictionary<string, float> trialLength = new Dictionary<string, float>
    {
        {"6 s (Default)", 6.0f},
        {"2 s", 2.0f},
        {"4 s", 4.0f},
        {"8 s", 8.0f},
        {"10 s", 10.0f},
    };


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
