using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateProfileMenu : MonoBehaviour
{
    TMP_InputField profileInputField;
    [SerializeField] UserProfileMenu userProfileMenu;
    TMP_Text usernameValidityMessage;

    public void SendProfile()
    {
        bool isUnique = true;

        // Get the input field with the profile name
        profileInputField = transform.Find("UserProfileNameInputField").GetComponent<TMP_InputField>();
        
        // Get the text box to display the user profile name valididy message
        usernameValidityMessage = transform.Find("usernameValidityMessage").GetComponent<TMP_Text>(); ;
        
        // Check if the field is empty or if the profile name already exists
        if (profileInputField.text != "")
        {
            foreach (TMP_Dropdown.OptionData userProfile in userProfileMenu.GetComponentInChildren<TMP_Dropdown>().options)
            {
                if (userProfile.text == profileInputField.text)
                {
                    isUnique = false;
                }
            }
            
            if (isUnique)
            {
                // Add the profile name to the dropdown and start training
                usernameValidityMessage.text = "";
                userProfileMenu.AddProfile(profileInputField.text);
                userProfileMenu.RemoveTitle();
                userProfileMenu.StartTraining();
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
