using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SelectLabelMenu : MonoBehaviour, ISaveable
{
    [SerializeField] TMP_Dropdown.OptionData title;

    private bool firstSelection = true;

    private TMP_Dropdown dropdown;

    public GameObject createnew;
    private List<SaveData.Label> labelData = new List<SaveData.Label>();


    void Start()
    {
        dropdown = GetComponentInChildren<TMP_Dropdown>();
        LoadJsonData(this);
        
        createnew = GameObject.Find("CreateLabelMenu");
        createnew.SetActive(false);

        //Add the title placeholder to the dropdown
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

    public void RemoveLabel()
    {
        // Get the index of the current option
        int currentOptionIndex = dropdown.value;

        // Remove the label from the save data
        foreach (SaveData.Label label in labelData)
        {
            if (label.labelData == dropdown.options[currentOptionIndex].text)
            {
                Debug.Log("Label found. Deleting user");
                labelData.Remove(label);
                SaveJsonData(this);
                break;
            }
        }
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

    
    private static void SaveJsonData(SelectLabelMenu a_SelectLabelMenu)
    {
        SaveData sd = new SaveData();
        a_SelectLabelMenu.PopulateSaveData(sd);

        if (FileManager.WriteToFile("LabelData.dat", sd.ToJson()))
        {
            Debug.Log("Save successful");
        }
    }

    private static void LoadJsonData(SelectLabelMenu a_SelectLabelMenu)
    {
        if (FileManager.LoadFromFile("LabelData.dat", out var json))
        {
            SaveData sd = new SaveData();
            sd.LoadFromJson(json);

            a_SelectLabelMenu.LoadFromSaveData(sd);
            Debug.Log("Load complete");
        }
    }

    public void PopulateSaveData(SaveData a_SaveData)
    {
        a_SaveData.labelDataList = labelData;
    }

    public void LoadFromSaveData(SaveData a_SaveData)
    {
        labelData = a_SaveData.labelDataList;

        dropdown.ClearOptions();
        foreach (SaveData.Label label in a_SaveData.labelDataList)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = label.labelData;
            dropdown.options.Add(option);
        }
    }
}
