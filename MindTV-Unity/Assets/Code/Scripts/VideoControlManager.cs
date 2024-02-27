using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class VideoControlManager : MonoBehaviour
{
    [SerializeField] private Image backgroundCell;
    [SerializeField] private Image imageGraphic;

    [SerializeField] private TMP_Dropdown mentalCommandDropdown;
    [SerializeField] private TMP_Text mentalCommandName;

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
    }

    public void InitializeListeners()
    {
        mentalCommandDropdown.onValueChanged.AddListener(MentalCommandChanged);
    }

    public void InitializeViews()
    {
        UpdateMentalCommandOptions();
        UpdateMentalCommand();
        UpdateCellColor();
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
    }

    public void MentalCommandChanged(int labelIndex)
    {
        string mentalCommand = mentalCommandDropdown.options[mentalCommandDropdown.value].text;
        controlPrefs.mentalCommandLabel = mentalCommand;
        UpdateMentalCommand();
        UpdateImage();
    }


    public void UpdateCellColor()
    {
        // TODO... why do this?  make color part of mental command like image

        // backgroundCell.color = videoCellPrefs.backgroundColor;

        // // make drop down match color
        // string colorName = Settings.NameForColor(videoCellPrefs.backgroundColor);
        // int colorIndex = backgroundColorDropdown.options.FindIndex(option => option.text == colorName);
        // backgroundColorDropdown.value = colorIndex;
    }

    public void UpdateImage()
    {
        // TODO - apply visibility too

        imageGraphic.sprite = SettingsManager.Instance?.currentUser.GetImageForLabel(controlPrefs.mentalCommandLabel);
    }

}

