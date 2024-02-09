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

    private void Awake()
    {
        // Set up a singleton (static instance) of SettingsManager
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Debug.Log("SettingsManager: initializing");

        profiles = new Settings();
        currentUser = new Settings.User();  // make a default user        
        Instance = this;

        SceneManager.sceneLoaded += OnSceneLoaded;
        DontDestroyOnLoad(gameObject);
        LoadSettings();
    }

    public List<string> GetUserProfileNames()
    {
        return profiles.users.Select(user => user.userProfileName).ToList();
    }

    public bool UserProfileExists(string name)
    {
        return profiles.users.Exists(user => user.userProfileName == name);
    }

    public void AddUserProfile(string name)
    {
        // Add new user profile and make it the current user
        Settings.User user = new Settings.User() { userProfileName = name };
        profiles.users.Add(user);
        currentUser = user;
        SaveSettings();
    }

    public void ActivateUserProfile(string name)
    {
        // UserProfiles.User is a struct, which is meant to be a value type.  This means it can't be null. 
        // That means List.Find() will return a default User if name doesn't exist (instead of null).
        // To avoid this, check existence first.  This is kinda dumb, but how C# works.
        if (UserProfileExists(name))
        {
            currentUser = profiles.users.Find(user => user.userProfileName == name);
            Debug.Log("UserProfileManager: activated user profile " + currentUser.userProfileName);
        }
        else
        {
            Debug.Log("UserProfileManager: user profile " + name + " not found");
        }
    }


    public void SaveSettings()
    {
        FileManager.WriteToFile("UserData.dat", profiles.ToJson());
    }

    public void LoadSettings()
    {
        if (FileManager.LoadFromFile("UserData.dat", out var json))
        {
            profiles.LoadFromJson(json);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Debug.Log("SettingsManager: training prefs are" + currentUser.trainingPrefs);
    }


    // set of user profiles
    private Settings profiles;
}
