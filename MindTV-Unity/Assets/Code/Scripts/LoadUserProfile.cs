using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadUserProfile : MonoBehaviour
{
    [SerializeField] TMP_Dropdown.OptionData title;

    private bool firstSelection = true;
    private TMP_Dropdown dropdown;

    void Start()
    {
        dropdown = GetComponentInChildren<TMP_Dropdown>();
        PopulateDropdownWithProfiles();

        // Add the title placeholder to the dropdown
        if (title != null)
        {
            dropdown.options.Insert(0, title);
            dropdown.RefreshShownValue();
        }
        // dropdown.options.Insert(dropdown.value, title);
        // dropdown.RefreshShownValue();
    }

    private void PopulateDropdownWithProfiles()
    {
        dropdown.ClearOptions();

        // Fetch the user profiles from the UserProfileManager
        var profiles = UserProfileManager.Instance.userProfiles;
        foreach (var profile in profiles)
        {
            TMP_Dropdown.OptionData newOption = new TMP_Dropdown.OptionData(profile.userProfileName);
            dropdown.options.Add(newOption);
        }

        dropdown.RefreshShownValue();
    }

    public void RemoveTitle()
    {
        if (firstSelection && title != null)
        {
            // Get the selected option
            string selectedOption = dropdown.options[dropdown.value].text;

            // Remove the title placeholder after a profile has been selected
            dropdown.options.Remove(title);
            dropdown.RefreshShownValue();

            // Display the selected option
            dropdown.value = dropdown.options.FindIndex(option => option.text == selectedOption);
            dropdown.RefreshShownValue();
        }

        firstSelection = false;
    }

    public void StartTraining()
    {
        // var currentUserProfileName = dropdown.options[dropdown.value].text;

        // // Check if the current user is a new user or an existing one
        // var existingUser = UserProfileManager.Instance.userProfiles.Find(user => user.userProfileName == currentUserProfileName);
        
        // if (existingUser != null)
        // {
        //     Debug.Log("User profile found");
        //     // Set the current user as the existing user
        //     CurrentUser.currentUser = existingUser;
        //     CurrentUser.newUser = false;
        // }
        // else
        // {
        //     Debug.Log("New user");
        //     // Create a new user profile and set it as the current user
        //     var newUser = new SaveData.User { userProfileName = currentUserProfileName };
        //     CurrentUser.currentUser = newUser;
        //     UserProfileManager.Instance.AddUserProfile(currentUserProfileName);
        //     CurrentUser.newUser = true;
        // }

        // // Assuming there is a method to save the current user's data
        // UserProfileManager.Instance.SaveCurrentUserData();

        // Logic to start training, like loading a new scene or starting a new activity
        StartTrainingActivity();
    }

    private void StartTrainingActivity()
    {
        Debug.Log("Starting training activity – not implemented yet");
        // Load the next scene or start the training activity
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
