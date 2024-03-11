using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using BCIEssentials.StimulusObjects;
using BCIEssentials.Controllers;

//Require that the object has a SPO component
[RequireComponent(typeof(SPO))]
[RequireComponent(typeof(VideoPanelButtonEffect))]
public class VideoControlManager : MonoBehaviour
{
    [SerializeField] private Image backgroundCell;
    [SerializeField] private TMP_Dropdown backgroundColorDropdown;
    [SerializeField] private Toggle includeImageToggle;
    [SerializeField] private Image imageGraphic;
    [SerializeField] private TMP_Dropdown mentalCommandDropdown;
    [SerializeField] private TMP_Text mentalCommandName;
    [SerializeField] private Slider slider;

    private SPO _spo;

    public Settings.VideoControlPrefs controlPrefs;

    [Tooltip("Invoked when the SPO Controller selects this SPO")]
    public UnityEvent OnSelectedEvent = new();

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

        if (BCIController.Instance != null && BCIController.Instance.ActiveBehavior.BehaviorType == BCIBehaviorType.P300)
        {
            Debug.Log("P300 is the active behavior, enabling P300 effect");
            gameObject.GetComponent<SPO>().StartStimulusEvent.AddListener(gameObject.GetComponent<VideoPanelButtonEffect>().SetOn);
            gameObject.GetComponent<SPO>().StopStimulusEvent.AddListener(gameObject.GetComponent<VideoPanelButtonEffect>().SetOff);
            UpdateP300Effect();
        }
        else
        {
            Debug.Log("P300 is not the active behavior, disabling P300 effect");
            //If not, disable the P300 effect
            gameObject.GetComponent<VideoPanelButtonEffect>().enabled = false;
        }
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
        _spo.ObjectID = (int)(SettingsManager.Instance?.currentUser.GetLabelNumberForLabelName(controlPrefs.mentalCommandLabel));
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

        //Initialize the P300 effect listener if the BCI instance is set to P300
        if (BCIController.Instance.ActiveBehavior.BehaviorType == BCIBehaviorType.P300)
        {
            UpdateP300Effect();
        }
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
        imageGraphic.sprite = SettingsManager.Instance?.currentUser.GetImageForLabelName(controlPrefs.mentalCommandLabel);
    }

    public void UpdateP300Effect()
    {
        // TODO - get rid of the duplication of code here.
        VideoPanelButtonEffect p300Effect = gameObject.GetComponent<VideoPanelButtonEffect>();
        p300Effect._flashOffColor = controlPrefs.backgroundColor;
    }

    public void VoteWithBCI()
    {
        slider.value += 1;

        if (slider.value >= slider.maxValue)
        {
            MakeSelectionWithBCI();
            slider.value = slider.minValue;
        }
    }


    public void MakeSelectionWithBCI()
    {
        OnSelectedEvent?.Invoke();
    }

}

