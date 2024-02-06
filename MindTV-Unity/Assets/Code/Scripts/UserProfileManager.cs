using System.Collections.Generic;
using UnityEngine;
using System.Linq; // For using LINQ queries

public class UserProfileManager : MonoBehaviour
{
    public static UserProfileManager Instance { get; private set; }

    public List<UserProfiles.User> userProfiles;
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
        UserProfiles.User newUser = new UserProfiles.User() { userProfileName = profileName };
        userProfiles.Add(newUser);
        // Set the new user as the current user
        currentUser = newUser;
        // Save profiles to file
        SaveProfiles();
    }

    public void SaveCurrentUserData()
    {
        Debug.Log("SaveCurrentUserData method not implemented yet");
        // // Update the current user data in the list
        // var existingUser = userProfiles.FirstOrDefault(u => u.userProfileName == currentUser.userProfileName);
        // if (existingUser != null)
        // {
        //     // Update existing user data
        //     userProfiles[userProfiles.IndexOf(existingUser)] = currentUser;
        // }
        // else
        // {
        //     // Add new user data if it doesn't exist
        //     userProfiles.Add(currentUser);
        // }
        // // Save the updated list of profiles to file
        // SaveProfiles();
    }

    private void SaveProfiles()
    {
        UserProfiles profile = new UserProfiles() { userProfiles = userProfiles };
        FileManager.WriteToFile("UserData.dat", profile.ToJson());
    }

    private void LoadProfiles()
    {
        if (FileManager.LoadFromFile("UserData.dat", out var json))
        {
            UserProfiles profile = new UserProfiles();
            profile.LoadFromJson(json);
            userProfiles = profile.userProfiles;
        }
        else
        {
            // Initialize an empty list if no data is found
            userProfiles = new List<UserProfiles.User>();
        }
    }

    public void StartTraining(string profileName)
    {
        Debug.Log("Starting training for user: " + profileName);
        SetCurrentUser(profileName);
        SaveCurrentUserData();
        StartTrainingActivity();
    }

    private void SetCurrentUser(string profileName)
    {
        Debug.Log("SetCurrentUser method not implemented yet");
        // var existingUser = userProfiles.FirstOrDefault(user => user.userProfileName == profileName);
        // if (existingUser != default(UserProfiles.User))
        // {
        //     // Existing user
        //     currentUser = existingUser;
        // }
        // else
        // {
        //     // New user
        //     currentUser = new UserProfiles.User { userProfileName = profileName };
        //     userProfiles.Add(currentUser);
        // }
    }

    private void StartTrainingActivity()
    {
        Debug.Log("StartingTrainingActivity not implemented yet");
        // Logic to start training, like loading a new scene or starting a new activity
        // Debug.Log("Starting training activity for user: " + currentUser.userProfileName);
        // SceneManager.LoadScene("TrainingScene"); // Example scene name
    }
}
