using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class UserProfileMenu : MonoBehaviour, ISaveable
{
    [SerializeField] TMP_Dropdown.OptionData title;

    private bool firstSelection = true;

    private TMP_Dropdown dropdown;

    private List<SaveData.User> userProfiles = new List<SaveData.User>();

    void Start()
    {
        dropdown = GetComponentInChildren<TMP_Dropdown>();
        LoadJsonData(this);

        // Add the title placeholder to the dropdown
        dropdown.options.Insert(dropdown.value, title);
        dropdown.RefreshShownValue();
    }

    public void RemoveTitle()
    {
        if (firstSelection)
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

    public void RemoveProfile()
    {
        // Get the index of the current option
        int currentOptionIndex = dropdown.value;

        // Remove the profile from the save data
        foreach (SaveData.User user in userProfiles)
        {
            if (user.userProfileName == dropdown.options[currentOptionIndex].text)
            {
                Debug.Log("User profile found. Deleting user");
                userProfiles.Remove(user);
                SaveJsonData(this);
                break;
            }
        }

        // Remove the user profile from the dropdown
        dropdown.options.Remove(dropdown.options[currentOptionIndex]);
        dropdown.RefreshShownValue();

        int numOptions = dropdown.options.Count;

        if (currentOptionIndex > numOptions - 1)
        {
            currentOptionIndex = 0;
        }

        // Display the next option
        dropdown.value = currentOptionIndex;
        dropdown.RefreshShownValue();
    }

    public void AddProfile(string profileName)
    {
        TMP_Dropdown.OptionData newProfile = new TMP_Dropdown.OptionData();
        newProfile.text = profileName;
        dropdown.options.Add(newProfile);

        dropdown.value = dropdown.options.IndexOf(newProfile);
    }

    public void StartTraining()
    {
        CurrentUser.newUser = true;

        // Assign current user to selected user profile
        foreach (SaveData.User user in userProfiles)
        {
            Debug.Log(user.userProfileName);
            if (user.userProfileName == dropdown.options[dropdown.value].text)
            {
                Debug.Log("User profile found");
                CurrentUser.currentUser = user;
                CurrentUser.newUser = false;
            }
        }

        if (CurrentUser.newUser == true)
        {
            Debug.Log("New user");
            SaveData.User user = new SaveData.User();
            user.userProfileName = dropdown.options[dropdown.value].text;
            CurrentUser.currentUser = user;
            userProfiles.Add(user);
        }

        SaveJsonData(this);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    private static void SaveJsonData(UserProfileMenu a_userProfileMenu)
    {
        SaveData sd = new SaveData();
        a_userProfileMenu.PopulateSaveData(sd);

        if (FileManager.WriteToFile("UserData.dat", sd.ToJson()))
        {
            Debug.Log("Save successful");
        }
    }

    private static void LoadJsonData(UserProfileMenu a_userProfileMenu)
    {
        if (FileManager.LoadFromFile("UserData.dat", out var json))
        {
            SaveData sd = new SaveData();
            sd.LoadFromJson(json);

            a_userProfileMenu.LoadFromSaveData(sd);
            Debug.Log("Load complete");
        }
    }

    public void PopulateSaveData(SaveData a_SaveData)
    {
        a_SaveData.userProfiles = userProfiles;
    }

    public void LoadFromSaveData(SaveData a_SaveData)
    {
        userProfiles = a_SaveData.userProfiles;

        dropdown.ClearOptions();
        foreach (SaveData.User user in a_SaveData.userProfiles)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = user.userProfileName;
            dropdown.options.Add(option);
        }
    }
}
