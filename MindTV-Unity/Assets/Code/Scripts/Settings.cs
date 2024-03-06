using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEditor;
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
        public Color backgroundColor = Settings.ColorForName("Blue (Theme)");

        public string imagePath = "";
        public float imageBaseSize = 100.0f;

        // trial length must be a positive multiple of windowLength
        public float windowLength = 2.0f;
        public float trialLength = 6.0f;

        // TODO - this may be better in an image utilities class
        public Sprite GetImageAsSprite()
        {
            Sprite sprite = null;
            if (File.Exists(imagePath))
            {
                Debug.Log("loading sprite");
                try
                {
                    byte[] imageData = File.ReadAllBytes(imagePath);
                    Texture2D texture = new(1, 1);
                    texture.LoadImage(imageData);

                    // resize to 500 by 500 to get consistent sizes
                    int targetX = 500;
                    int targetY = 500;
                    RenderTexture rt = new RenderTexture(targetX, targetY, 24);
                    RenderTexture.active = rt;
                    Graphics.Blit(texture, rt);
                    Texture2D result = new Texture2D(targetX, targetY);
                    result.ReadPixels(new Rect(0, 0, targetX, targetY), 0, 0);
                    result.Apply();
                    texture = result;

                    // generate image
                    sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
                }
                catch (Exception e)
                {
                    Debug.LogError($"TrainingPrefs: Failed to load {imagePath} with exception {e}");
                }
            }
            return sprite;
        }
    }

    [System.Serializable]
    public class VideoCellPrefs
    {
        public int cellNumber = 0;
        public string videoTitle = "Video Title";
        public string videoPath = "";
        public Color backgroundColor = Settings.ColorForName("Purple (Theme)");
        public bool includeGraphic = true;
        public string mentalCommandLabel = "";
    }

    [System.Serializable]
    public class VideoControlPrefs
    {
        public int controlNumber = 0;
        public Color backgroundColor = Settings.ColorForName("Purple (Theme)");
        public bool includeGraphic = true;
        public string mentalCommandLabel = "";
    }


    [System.Serializable]
    public class User
    {
        public string userProfileName = "";

        public List<TrainingPrefs> trainingPrefs = new List<TrainingPrefs>()
        {
            // Hard code the number of labels we can have to 4, maybe later allow user to add
            new TrainingPrefs() { labelNumber = 0 },
            new TrainingPrefs() { labelNumber = 1 },
            new TrainingPrefs() { labelNumber = 2 },
            new TrainingPrefs() { labelNumber = 3 },
        };

        public List<VideoCellPrefs> videoCellPrefs = new List<VideoCellPrefs>()
        {
            // Start the default user out with a single video cell
            new VideoCellPrefs() { cellNumber = 0 },
        };

        public List<VideoControlPrefs> videoControlPrefs = new List<VideoControlPrefs>()
        {
            // Hard code the number of labels we can have to 4, maybe later allow user to add
            new VideoControlPrefs() { controlNumber = 0 },
            new VideoControlPrefs() { controlNumber = 1 },
            new VideoControlPrefs() { controlNumber = 2 },
            new VideoControlPrefs() { controlNumber = 3 },
        };


        public List<string> AvailableLabels()
        {
            // discard labels that are blank
            List<string> allLabels = trainingPrefs.Select(prefs => prefs.labelName).ToList();
            List<string> assignedLabels = allLabels.FindAll(label => label.Count() > 0);
            return assignedLabels;
        }

        public Sprite GetImageForLabel(string label)
        {
            TrainingPrefs prefs = trainingPrefs.Find(prefs => prefs.labelName == label);
            return prefs?.GetImageAsSprite() ?? null;
        }
        public int GetIDForLabel(string label)
        {
            TrainingPrefs prefs = trainingPrefs.Find(prefs => prefs.labelName == label);
            int labelId = prefs?.labelNumber ?? -100;
            return labelId;
        }


        public VideoCellPrefs AddVideoCell()
        {
            var newCell = new VideoCellPrefs() { cellNumber = videoCellPrefs.Count };
            videoCellPrefs.Add(newCell);
            return newCell;
        }

        public void RemoveLastVideoCell()
        {
            if (videoCellPrefs.Any())
            {
                videoCellPrefs.RemoveAt(videoCellPrefs.Count - 1);
            }
        }
    }


    // The set of user profiles, aka "user settings"
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
