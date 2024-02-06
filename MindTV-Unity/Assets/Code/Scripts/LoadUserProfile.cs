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
    }

    private void PopulateDropdownWithProfiles()
    {
        dropdown.ClearOptions();

        // Fetch the user profiles from the UserProfileManager
        var profileNames = UserProfileManager.Instance.GetUserProfileNames();
        foreach (var name in profileNames)
        {
            TMP_Dropdown.OptionData newOption = new TMP_Dropdown.OptionData(name);
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

    public void LoadSelectedUserForTraining()
    {
        string name = dropdown.options[dropdown.value].text;
        UserProfileManager.Instance.ActivateUserProfile(name);
    }

}
