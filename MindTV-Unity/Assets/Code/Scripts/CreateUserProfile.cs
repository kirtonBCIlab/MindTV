using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class CreateuserProfile : MonoBehaviour
{
    TMP_InputField profileInputField;
    [SerializeField] LoadManageUserProfiles userProfileMenu;
    TMP_Text usernameValidityMessage;

    public void SendProfile()
    {
        bool isUnique = true;
        string validUsernamePattern = @"^[a-zA-Z0-9_ –—]+$"; // Regex pattern

        // Get the input field with the profile name
        profileInputField = transform.Find("UserProfileNameInputField").GetComponent<TMP_InputField>();
        
        // Get the text box to display the user profile name validity message
        usernameValidityMessage = transform.Find("usernameValidityMessage").GetComponent<TMP_Text>();
        
        // Check if the field is empty
        if (!string.IsNullOrEmpty(profileInputField.text))
        {
            // Check if the username matches the regex pattern
            if (!Regex.IsMatch(profileInputField.text, validUsernamePattern))
            {
                Debug.Log("Invalid characters used");
                usernameValidityMessage.text = "Invalid characters used. Only upper and lower case letters, numbers, spaces, underscores (_), en dash hyphen (-), and em dash hyphen (–) are allowed.";
                return; // Stop further processing
            }

            // foreach (TMP_Dropdown.OptionData userProfile in userProfileMenu.GetComponentInChildren<TMP_Dropdown>().options)
            // {
            //     // if (userProfile.text == profileInputField.text)
            //     // {
            //     //     isUnique = false;
            //     //     break;
            //     // }
            // }

            if (isUnique)
            {
                // Add the profile name to the dropdown and start training
                usernameValidityMessage.text = "";
                userProfileMenu.AddProfile(profileInputField.text);
                // userProfileMenu.RemoveTitle();
                // userProfileMenu.StartTraining();
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