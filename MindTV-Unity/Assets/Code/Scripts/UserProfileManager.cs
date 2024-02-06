using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting; // For using LINQ queries

public class UserProfileManager : MonoBehaviour
{
    public static UserProfileManager Instance { get; private set; }

    public UserProfiles.User currentUser; // Current active user settings

    private void Awake()
    {
        // Set up a singleton (static instance) of UserProfileManager
        if (Instance == null)
        {
            profiles = new UserProfiles();
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadProfiles();
        }
        else if (Instance != this)
        {
            // If a different instance exists, destroy this one
            Destroy(gameObject);
        }
    }

    public List<string> GetUserProfileNames()
    {
        return profiles.users.Select(user => user.userProfileName).ToList();
    }

    public bool UserProfileExists(string name)
    {
        return profiles.users.Any(user => user.userProfileName == name);
    }

    public void AddUserProfile(string name)
    {
        // Add new user profile and make it the current user
        UserProfiles.User user = new UserProfiles.User() { userProfileName = name };
        profiles.users.Add(user);
        currentUser = user;
        SaveProfiles();
    }

    public void ActivateUserProfile(string name)
    {
        // UserProfiles.User is a struct which means List.Find() will return a default User if name
        // doesn't exist (instead of null).  To avoid this, check existence first.  This is dumb.
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


    public void SaveProfiles()
    {
        FileManager.WriteToFile("UserData.dat", profiles.ToJson());
    }

    public void LoadProfiles()
    {
        if (FileManager.LoadFromFile("UserData.dat", out var json))
        {
            profiles.LoadFromJson(json);
        }
    }

    // set of user profiles
    private UserProfiles profiles;
}
