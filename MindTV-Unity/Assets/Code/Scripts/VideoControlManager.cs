using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BCIEssentials.StimulusObjects;

//Require that the object has a SPO component
[RequireComponent(typeof(SPO))]
public class VideoControlManager : MonoBehaviour
{
    [SerializeField] private Image backgroundCell;
    [SerializeField] private TMP_Dropdown backgroundColorDropdown;
    [SerializeField] private Toggle includeImageToggle;
    [SerializeField] private Image imageGraphic;
    [SerializeField] private TMP_Dropdown mentalCommandDropdown;
    [SerializeField] private TMP_Text mentalCommandName;

    private SPO _spo;

    public Settings.VideoControlPrefs controlPrefs;

    void Start()
    {
        InitializeSettings();
        InitializeListeners();
        InitializeViews();
    }

    private void InitializeSettings()
    {
        // Use the sibling index as the "label number".  This is needed to choose the correct
        // VideoControlPrefs object from the data model.  Use a dummy object if one is not found.
        int controlNumber = transform.GetSiblingIndex();
        controlPrefs = SettingsManager.Instance?.currentUser.videoControlPrefs[controlNumber] ?? new Settings.VideoControlPrefs();
        // Get the SPO component on this game object
        _spo = GetComponent<SPO>(); 
    }

    public void InitializeListeners()
    {
        backgroundColorDropdown.onValueChanged.AddListener(CellColorChanged);
        includeImageToggle.onValueChanged.AddListener(ImageVisibilityChanged);
        mentalCommandDropdown.onValueChanged.AddListener(MentalCommandChanged);
    }

    public void InitializeViews()
    {
        UpdateMentalCommandOptions();
        UpdateMentalCommand();
        UpdateCellColor();
        UpdateImageVisibility();
        UpdateImage();
    }

    public void UpdateMentalCommandOptions()
    {
        // populate the dropdown with mental commands (labels)
        // Add the title placeholder to the dropdown so nothing appears selected
        List<string> labels = SettingsManager.Instance?.currentUser.AvailableLabels();
        mentalCommandDropdown.ClearOptions();
        mentalCommandDropdown.options.Insert(0, new TMP_Dropdown.OptionData());
        foreach (var label in labels)
        {
            TMP_Dropdown.OptionData newOption = new TMP_Dropdown.OptionData(label);
            mentalCommandDropdown.options.Add(newOption);
        }
    }


    public void UpdateMentalCommand()
    {
        mentalCommandName.text = controlPrefs.mentalCommandLabel;
        mentalCommandDropdown.value = mentalCommandDropdown.options.FindIndex(option => option.text == controlPrefs.mentalCommandLabel);
        //Update the SPO component with the new mental command
        _spo.ObjectID = (int)(SettingsManager.Instance?.currentUser.GetIDForLabel(controlPrefs.mentalCommandLabel));
    }

    public void MentalCommandChanged(int labelIndex)
    {
        string mentalCommand = mentalCommandDropdown.options[labelIndex].text;
        controlPrefs.mentalCommandLabel = mentalCommand;
        UpdateMentalCommand();
        UpdateImage();
    }


    public void UpdateCellColor()
    {
        backgroundCell.color = controlPrefs.backgroundColor;

        // make drop down match color
        string colorName = Settings.NameForColor(controlPrefs.backgroundColor);
        int colorIndex = backgroundColorDropdown.options.FindIndex(option => option.text == colorName);
        backgroundColorDropdown.value = colorIndex;
    }

    public void CellColorChanged(int cellIndex)
    {
        string colorName = backgroundColorDropdown.options[cellIndex].text;
        Color color = Settings.ColorForName(colorName);
        controlPrefs.backgroundColor = color;
        UpdateCellColor();
    }


    public void UpdateImageVisibility()
    {
        imageGraphic.enabled = controlPrefs.includeGraphic;
        includeImageToggle.isOn = controlPrefs.includeGraphic;
    }

    public void ImageVisibilityChanged(bool isOn)
    {
        controlPrefs.includeGraphic = isOn;
        UpdateImageVisibility();
    }


    public void UpdateImage()
    {
        imageGraphic.sprite = SettingsManager.Instance?.currentUser.GetImageForLabel(controlPrefs.mentalCommandLabel);
    }

}

