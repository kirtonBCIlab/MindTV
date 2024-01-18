using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SelectLabelMenu : MonoBehaviour
{
    [SerializeField] TMP_Dropdown.OptionData title;

    private bool firstSelection = true;

    private TMP_Dropdown dropdown;

    public GameObject createnew;
    void Start()
    {
        dropdown = GetComponentInChildren<TMP_Dropdown>();
        
        createnew = GameObject.Find("CreateLabelMenu");
        createnew.SetActive(false);

        // Add the title placeholder to the dropdown
       // dropdown.options.Insert(dropdown.value, title);
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

    public void RemoveLabel()
    {
        // Get the index of the current option
        int currentOptionIndex = dropdown.value;

        // Remove the label from the dropdown
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

    public void AddLabel(string label)
    {
        TMP_Dropdown.OptionData newLabel = new TMP_Dropdown.OptionData();
        newLabel.text = label;
        dropdown.options.Add(newLabel);

        dropdown.value = dropdown.options.IndexOf(newLabel);
    }

    public void OnCreateNewLabel()
    {
      createnew.SetActive(true);
    }

    public void OnCancel()
    {
        createnew.SetActive(false);
    }

    public void ToTrainingMenu()
    {
        SceneManager.LoadScene("TrainingScene");
    }
}
