using System.Collections.Generic;
using UnityEngine;
using System.Linq; // For using LINQ queries

public class UserProfileManager : MonoBehaviour
{
    public static UserProfileManager Instance { get; private set; }

    public UserProfiles.User currentUser; // Current active user

    private void Awake()
    {
        // Check if an instance already exists
        if (Instance == null)
        {
            // Assign this instance to the static instance and make it persistent
            Instance = this;
            DontDestroyOnLoad(gameObject);
            // Load existing profiles
            LoadProfiles();
        }
        else if (Instance != this)
        {
            // If a different instance exists, destroy this one
            Destroy(gameObject);
        }
    }

    public void AddUserProfile(string profileName)
    {
        // Add new user profile
        UserProfiles.User user = new UserProfiles.User() { userProfileName = profileName };
        profiles.users.Add(user);
        // Set the new user as the current user
        currentUser = user;
        SaveProfiles();
    }

    public List<string> GetUserProfileNames()
    {
        return profiles.users.Select(user => user.userProfileName).ToList();
    }

    public bool UserProfileExists(string name)
    {
        return profiles.users.Any(user => user.userProfileName == name);
    }

    private void SaveProfiles()
    {
        FileManager.WriteToFile("UserData.dat", profiles.ToJson());
    }

    private void LoadProfiles()
    {
        if (FileManager.LoadFromFile("UserData.dat", out var json))
        {
            profiles.LoadFromJson(json);
        }
        else
        {
            // Initialize an empty list if no data is found
            profiles = new UserProfiles();
        }
    }

    // set of user profiles
    private UserProfiles profiles;
}
