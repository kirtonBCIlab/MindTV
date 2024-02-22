using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using Unity.VisualScripting;
using System; // For using LINQ queries


/// <summary>
/// SettingsManager is responsible for loading Settings from disk and making them available
/// to the rest of the applicaiton via currentUser.  When the applicaiton shuts down the
/// Settings object is saved to disk.
/// </summary>
public class SettingsManager : MonoBehaviour
{
    // Singleton property to access settings: SettingsManager.Instance.currentUser...
    // This will initialize SettingsManager if not already present, such as when
    // starting from a scene other than MainMenu.
    public static SettingsManager Instance
    {
        get
        {
            if (privateInstance == null)
            {
                InitializeInstance();
            }
            return privateInstance;
        }
    }

    // Current active user settings
    public Settings.User currentUser;


    public void Awake()
    {
        // If singleton doesn't exist, then I'm the singleton
        if (privateInstance == null)
        {
            Debug.Log("SettingsManager: initializing");
            privateInstance = this;
            DontDestroyOnLoad(this.gameObject);
            InitializeSettings();
            LoadSettings();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnApplicationQuit()
    {
        SaveSettings();
    }


    public List<string> GetUserProfileNames()
    {
        return settings.users.Select(user => user.userProfileName).ToList();
    }

    public bool UserProfileExists(string name)
    {
        return settings.users.Exists(user => user.userProfileName == name);
    }

    public void AddUserProfile(string name)
    {
        // Add new User and make it current
        Settings.User user = new Settings.User() { userProfileName = name };
        settings.users.Add(user);
        currentUser = user;
        SaveSettings();
    }

    public void ActivateUserProfile(string name)
    {
        if (UserProfileExists(name))
        {
            currentUser = settings.users.Find(user => user.userProfileName == name);
            Debug.Log("SettingsManager: activated user " + currentUser.userProfileName);
        }
        else
        {
            Debug.Log("SettingsManager: user " + name + " not found");
        }
    }

    public void DeleteUserProfile(string name)
    {
        if (UserProfileExists(name))
        {
            Settings.User user = settings.users.Find(user => user.userProfileName == name);
            settings.users.Remove(user);
            Debug.Log("SettingsManager: removed user " + user.userProfileName);
        }
        else
        {
            Debug.Log("SettingsManager: user " + name + " not found");
        }
    }


    public void SaveSettings()
    {
        Debug.Log("SettingsManager: saving settings");
        FileManager.WriteToFile("UserData.dat", settings.ToJson());
    }

    public void LoadSettings()
    {
        if (FileManager.LoadFromFile("UserData.dat", out var json))
        {
            Debug.Log("SettingsManager: loading settings");
            settings.LoadFromJson(json);
        }
    }


    private void InitializeSettings()
    {
        // Settings will contain any User objects loaded from disk
        settings = new Settings();

        // Start with a default User (default settings)
        currentUser = new Settings.User();
    }

    private static void InitializeInstance()
    {
        // If there isn't already an instance of the SettingsManager, then create a
        // game object with a SettingsManager component.  The game object will call 
        // SettingsManager.Awake() which completes the initialization.
        SettingsManager instance = FindObjectOfType<SettingsManager>();
        if (instance == null)
        {
            GameObject go = new GameObject();
            go.name = "SettingsManager Singleton";
            go.AddComponent<SettingsManager>();
            DontDestroyOnLoad(go);
        }
    }


    private Settings settings;
    private static SettingsManager privateInstance;
}
