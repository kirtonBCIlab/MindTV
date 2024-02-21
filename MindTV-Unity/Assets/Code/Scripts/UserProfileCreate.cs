using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class UserProfileCreate : MonoBehaviour
{
    [SerializeField] private TMP_InputField profileInputField;
    [SerializeField] private TMP_Text usernameValidityMessage;
    [SerializeField] private Button createButton;

    void Start()
    {
        createButton.onClick.AddListener(CreateProfile);
    }

    public void CreateProfile()
    {
        // Check if the field is empty
        if (!string.IsNullOrEmpty(profileInputField.text))
        {
            // Check if the username matches the regex pattern
            string validUsernamePattern = @"^[a-zA-Z0-9_ –—]+$"; // Regex pattern
            if (!Regex.IsMatch(profileInputField.text, validUsernamePattern))
            {
                Debug.Log("Invalid characters used");
                usernameValidityMessage.text = "Invalid characters used.\n\nOnly upper and lower case letters, numbers, spaces, underscores (_), en dash hyphen (-), and em dash hyphen (–) are allowed.";
                return; // Stop further processing
            }

            // Check if the username is unique
            string newProfileName = profileInputField.text;
            if (SettingsManager.Instance.UserProfileExists(newProfileName) != true)
            {
                // Add the profile name to the UserProfileManager
                Debug.Log("Creating new profile: " + profileInputField.text);
                SettingsManager.Instance.AddUserProfile(profileInputField.text);
                usernameValidityMessage.text = "";
            }
            else
            {
                Debug.Log("Profile name is already taken");
                usernameValidityMessage.text = "Profile name is already taken";
            }
        }
        else
        {
            Debug.Log("Profile name cannot be empty");
            usernameValidityMessage.text = "Profile name cannot be empty";
        }
    }
}