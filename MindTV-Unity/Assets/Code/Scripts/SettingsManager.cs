using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using Unity.VisualScripting;
using System; // For using LINQ queries


/// <summary>
/// SettingsManager is responsible for loading settings from disk and pushing out to
/// the application.  It is also responsible for gathering changed settings from the application
/// and writing to disk.
/// </summary>
public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }

    // Current active user settings
    public Settings.User currentUser;


    public void Awake()
    {
        // Set up a singleton (static instance) of SettingsManager
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Debug.Log("SettingsManager: initializing");

        settings = new Settings();
        currentUser = new Settings.User();  // make a dummy user (not saved)
        Instance = this;

        DontDestroyOnLoad(gameObject);
        LoadSettings();
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

    private Settings settings;
}
