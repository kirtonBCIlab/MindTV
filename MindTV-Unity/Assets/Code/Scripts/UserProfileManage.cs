using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserProfileManage : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private Button deleteButton;

    void Start()
    {
        dropdown.onValueChanged.AddListener(SelectionChanged);
        deleteButton.onClick.AddListener(LoadSelectedUserProfile);
    }

    void OnEnable()
    {
        PopulateDropdownWithProfiles();
    }

    public void SelectionChanged(int index)
    {
        deleteButton.gameObject.SetActive(true);
    }

    public void LoadSelectedUserProfile()
    {
        string name = dropdown.options[dropdown.value].text;
        SettingsManager.Instance.DeleteUserProfile(name);
        PopulateDropdownWithProfiles();
    }

    // TODO - the dropdown could be made into a profile selection prefab and reused in load / manage prefabs
    private void PopulateDropdownWithProfiles()
    {
        deleteButton.gameObject.SetActive(false);
        dropdown.ClearOptions();

        // Add the title placeholder to the dropdown so nothing appears selected
        dropdown.options.Insert(0, new TMP_Dropdown.OptionData());

        // Fetch the user profiles from the SettingsManager
        var profileNames = SettingsManager.Instance.GetUserProfileNames();
        foreach (var name in profileNames)
        {
            TMP_Dropdown.OptionData newOption = new TMP_Dropdown.OptionData(name);
            dropdown.options.Add(newOption);
        }

        dropdown.RefreshShownValue();
    }

}
