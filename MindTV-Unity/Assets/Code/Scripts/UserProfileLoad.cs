using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserProfileLoad : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private Button loadButton;

    void Start()
    {
        dropdown.onValueChanged.AddListener(SelectionChanged);
        loadButton.onClick.AddListener(LoadSelectedUserProfile);
    }

    void OnEnable()
    {
        PopulateDropdownWithProfiles();
    }

    private void PopulateDropdownWithProfiles()
    {
        loadButton.gameObject.SetActive(false);
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

    public void SelectionChanged(int index)
    {
        loadButton.gameObject.SetActive(true);
    }

    public void LoadSelectedUserProfile()
    {
        string name = dropdown.options[dropdown.value].text;
        SettingsManager.Instance.ActivateUserProfile(name);
    }

}
