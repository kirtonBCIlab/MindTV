using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
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
        public int labelNumber = 0;
        public string labelName = "";
        public string animationName = "None";
        public Color backgroundColor = Settings.ColorForName("Blue (Theme)");

        public float windowLength = 2;  // trial length must be a multiple of windowLength
        public float trialLength = 6;
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
        public List<VideoPrefs> videoPrefs = new List<VideoPrefs>()
        {
        };

        public int numberVideoCells = 4;
    }

    [System.Serializable]
    public class VideoPrefs
    {
        public int numberVideoCells = 1;

    }

    [System.Serializable]
    public class VideoCell
    {
        public int tileNumber = 0;
        public string videoTitle = "Video Title";
        public string videoPath = "";
        public Color backgroundColor = Settings.ColorForName("Blue (Theme)");
        public Image videoThumbnail;
        public bool includeGraphic = false;
        public string layoutStyle = "Default";
        public string mentalCommandLabel = "None";
    }
    // Set list of available videos
    // public List<VideoPrefs> videos = new List<VideoPrefs>()
    // {
    //     new VideoPrefs() { videoTitle = "Zelda", videoPath = "Assets/Videos/DevVideo0-Zelda.mp4" },
    //     new VideoPrefs() { videoTitle = "PBJ Time", videoPath = "Assets/Videos/DevVideo1-PeanutButterJelly.mp4" },
    //     new VideoPrefs() { videoTitle = "Watermelon Meow Meow", videoPath = "Assets/Videos/DevVideo2-WatermelonMeowMeow.mp4" },
    //     new VideoPrefs() { videoTitle = "Kaguya Nursery Rhyme", videoPath = "Assets/Videos/DevVideo3-KaguyaOSTNurseryRhyme.mp4" },
    // };

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
        {"6 s (Default)", 6},
        {"2 s", 2},
        {"4 s", 4},
        {"8 s", 8},
        {"10 s", 10},
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
