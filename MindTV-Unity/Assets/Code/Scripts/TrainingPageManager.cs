using BCIEssentials.ControllerBehaviors;
using BCIEssentials.StimulusObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class TrainingPageManager : MonoBehaviour
{
    // UI elements within the TrainingPage prefab
    public TMP_InputField trainingLabelEntry;
    public TMP_Dropdown colorDropdown;
    public TMP_Dropdown animDropdown;
    public GameObject activeTraining;
    public GameObject _SPO;

    // Event to signal when preferences have been changed
    // TODO - this could be replaced with a scriptable object event where
    // the settings then tell others when they have been changed.
    public static event Action TrainingPrefsChanged;

    // Reference to training settings
    private Settings.TrainingPrefs trainingPrefs;

    //Exposing this so that we can change the base size of the training object
    public float originalBaseSize = 100.0f;
    public float targetImageResolution = 512f;
    public Slider baseSizeSlider;

    [SerializeField] private GameObject trainingOptionsFrame;
    [SerializeField] private GameObject displayStartTrainingButton;
    [SerializeField] private GameObject displayNumberOfTimesTrained;
    private float currentBaseSize;
    private Vector3 originalPosition;

    private UITweener tweener;


    private void Start()
    {
        InitializeSettings();
        InitializeViews();
        InitializeListeners();

        // TODO - move this to a helper
        originalPosition = _SPO.transform.position;

        currentBaseSize = originalBaseSize;
        _SPO.transform.localScale = new Vector3(currentBaseSize, currentBaseSize, currentBaseSize);
        baseSizeSlider.value = currentBaseSize;
    }

    //SPO Toybox stuff
    private void OnEnable()
    {
        SPOToyBox spoToyBox = FindObjectOfType<SPOToyBox>(); //We should set this up better, as there should only ever be one toybox.
         // Check if there is an SPO in the SPOToyBox with the same ID as TabNumber
        if (spoToyBox != null)
        {
            // Get the label number from the TrainingPage sibling index, same as what we use to save data
            int labelNumber = transform.GetSiblingIndex();
            if (spoToyBox.GetSPO(labelNumber) == null)
            {
                Debug.Log("SPO with ID " + labelNumber + " not found in SPOToyBox!");
                // trainingObjectSPO = spoToyBox.GetSPO(tabNumber);
            }
            else
            {
                Destroy(_SPO); // Destroy the current SPO (if any)
                _SPO = spoToyBox.GetSPO(labelNumber);
            }
            // {
            //     // Destroy(trainingObjectSPO); // Destroy the current SPO (if any)
            //     trainingObjectSPO = spoToyBox.GetSPO(tabNumber);
            // }
            // catch
            // {
            //     Debug.LogError("SPO with ID " + tabNumber + " not found in SPOToyBox!");
            // }
        }
    }


    private void InitializeSettings()
    {
        // Use the TrainingPage sibling index as the "label number".  This is needed to choose the correct
        // TrainingPrefs object from the data model.  Use a dummy TrainingPrefs if one is not found.
        int labelNumber = transform.GetSiblingIndex();
        trainingPrefs = SettingsManager.Instance?.currentUser.trainingPrefs[labelNumber] ?? new Settings.TrainingPrefs();
    }

    private void InitializeViews()
    {
        UpdateTrainingLabel();
        UpdateTrainingPageColor();
    }

    private void InitializeListeners()
    {
        trainingLabelEntry.onEndEdit.AddListener(LabelChanged);
        colorDropdown.onValueChanged.AddListener(ColorChanged);
    }



    public void UpdateTrainingLabel()
    {
        trainingLabelEntry.text = trainingPrefs.labelText;
    }

    public void LabelChanged(string labelText)
    {
        trainingPrefs.labelText = labelText;
        TrainingPrefsChanged();
    }


    public void UpdateTrainingPageColor()
    {
        Image imageComponent = activeTraining.GetComponent<Image>();
        imageComponent.color = trainingPrefs.backgroundColor;

        // TODO - this should be a function offered by the dropdown
        string colorName = ColorByName.NameForColor(trainingPrefs.backgroundColor);
        int colorIndex = colorDropdown.options.FindIndex(name => name.text == colorName);
        colorDropdown.value = colorIndex;
    }

    public void ColorChanged(int colorIndex)
    {
        // translate drop down text into a color and persist
        string colorText = colorDropdown.options[colorIndex].text;
        Color color = ColorByName.colors[colorText];
        trainingPrefs.backgroundColor = color;

        TrainingPrefsChanged();
        UpdateTrainingPageColor();
    }


    // resets the position and scale of the traning object
    public void ResetSPO()
    {
        _SPO.transform.position = originalPosition;
    }

    // changes the training object image property
    // TODO - this is coupled to InventorySlot, consider replacing an image changed event
    // Then TrainingPageManager can decide what to do when the event happens.
    public void SetTrainingObject(Sprite image_sprite)
    {
        ResetSPO();
        _SPO.GetComponent<SpriteRenderer>().sprite = image_sprite;

        // If we want the newly set image to be reset to the original size, use this: (uncomment the line below)
        ResetBaseSize();
    }


    // Calculate the scale factor needed to resize the longest dimension to targetImageResolution (512x512)
    private float UniformImageSizeScaleFactor(SpriteRenderer spriteRenderer)
    {
        float width = spriteRenderer.sprite.texture.width;
        float height = spriteRenderer.sprite.texture.height;
        float maxDimension = Mathf.Max(width, height);
        float scaleFactor = targetImageResolution / maxDimension;
        return scaleFactor;
    }

    // Sets the base size of the training object
    private void SetBaseSize(float size)
    {
        SpriteRenderer spriteRenderer = _SPO.GetComponent<SpriteRenderer>();
        float uniformScaleFactor = UniformImageSizeScaleFactor(spriteRenderer);
        float scaledSize = size * uniformScaleFactor;
        _SPO.transform.localScale = new Vector3(scaledSize, scaledSize, scaledSize);
    }

    // changes the training object base size
    public void ModifyBaseSizeWithSlider()
    {
        currentBaseSize = baseSizeSlider.value;
        //Debug.Log("Base size changed to " + currentBaseSize);
        SetBaseSize(currentBaseSize);
    }

    // resets the base size
    public void ResetBaseSize()
    {
        currentBaseSize = originalBaseSize;
        baseSizeSlider.value = currentBaseSize;
        SetBaseSize(currentBaseSize);
        //Also going to reset the position of the training object
        ResetSPO();
    }


    public void SetAnimationOnSelection()
    {
        string animText = animDropdown.options[animDropdown.value].text;
        tweener = _SPO.GetComponent<UITweener>();
        tweener.SetTweenFromString(animText);
        Debug.Log("end of method in stim manager");
    }

    //This is brought over from TrainingMenuController as one of 2 things I think I can see that is being used
     public void HighlightSelectedSprite(GameObject inventorySlot)
    {
        GameObject[] inventory = GameObject.FindGameObjectsWithTag("InventorySlot");

        foreach (GameObject slot in inventory)
        {
            GameObject frame = slot.transform.Find("Frame").gameObject;
            frame.SetActive(false);
        }

        GameObject selectedFrame = inventorySlot.transform.Find("Frame").gameObject;
        selectedFrame.SetActive(true);
    }

    //This is brought over from TrainingMenuController as one of 2 things I think I can see that is being used
        public void ToggleInventoryVisibility()
    {
        trainingOptionsFrame.SetActive(!trainingOptionsFrame.activeSelf);
        displayStartTrainingButton.SetActive(!displayStartTrainingButton.activeSelf);
        displayNumberOfTimesTrained.SetActive(!displayNumberOfTimesTrained.activeSelf);
    }
}
