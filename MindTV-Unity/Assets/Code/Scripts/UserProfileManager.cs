using System.Collections.Generic;
using UnityEngine;
using System.Linq; // For using LINQ queries

public class UserProfileManager : MonoBehaviour
{
    public static UserProfileManager Instance { get; private set; }

    public List<SaveData.User> userProfiles;
    public SaveData.User currentUser; // Current active user

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
        SaveData.User newUser = new SaveData.User() { userProfileName = profileName };
        userProfiles.Add(newUser);
        // Set the new user as the current user
        currentUser = newUser;
        // Save profiles to file
        SaveProfiles();
    }

    // public void SaveCurrentUserData()
    // {
    //     // Update the current user data in the list
    //     var existingUser = userProfiles.FirstOrDefault(u => u.userProfileName == currentUser.userProfileName);
    //     if (existingUser != null)
    //     {
    //         // Update existing user data
    //         userProfiles[userProfiles.IndexOf(existingUser)] = currentUser;
    //     }
    //     else
    //     {
    //         // Add new user data if it doesn't exist
    //         userProfiles.Add(currentUser);
    //     }
    //     // Save the updated list of profiles to file
    //     SaveProfiles();
    // }

    private void SaveProfiles()
    {
        SaveData saveData = new SaveData() { userProfiles = userProfiles };
        FileManager.WriteToFile("UserData.dat", saveData.ToJson());
    }

    private void LoadProfiles()
    {
        if (FileManager.LoadFromFile("UserData.dat", out var json))
        {
            SaveData saveData = new SaveData();
            saveData.LoadFromJson(json);
            userProfiles = saveData.userProfiles;
        }
        else
        {
            // Initialize an empty list if no data is found
            userProfiles = new List<SaveData.User>();
        }
    }

    // Additional methods to manipulate user profiles can be added here
    // Example: RemoveUserProfile, UpdateUserProfile, etc.
}
