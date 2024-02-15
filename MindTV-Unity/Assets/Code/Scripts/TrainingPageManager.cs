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
    [SerializeField] private TrainingMenuController trainingController;
    [SerializeField] private TMP_Text countDownText;
    [SerializeField] private GameObject _SPO;

    // This identifies which training tab
    public int labelNumber;

    // UI elements within the TrainingPage prefab
    public TMP_InputField trainingLabelEntry;
    public TMP_Dropdown colorDropdown;
    public TMP_Dropdown animDropdown;

    private UITweener tweener;
    private GameObject activeTraining;
    private MIControllerBehavior controllerBehaviour;

    // Reference to training settings
    private Settings.TrainingPrefs trainingPrefs;

    //Exposing this so that we can change the base size of the training object
    public float originalBaseSize = 100.0f;
    private float currentBaseSize;
    public float targetImageResolution = 512f;
    private Vector3 originalPosition;
    public Slider baseSizeSlider;

    private void Start()
    {
        InitializeSettings();
        InitializeListeners();
        InitializeViews();

        // TODO - move this to a helper
        trainingController = GameObject.FindGameObjectWithTag("TrainingPanel").GetComponent<TrainingMenuController>();
        originalPosition = _SPO.transform.position;

        currentBaseSize = originalBaseSize;
        _SPO.transform.localScale = new Vector3(currentBaseSize, currentBaseSize, currentBaseSize);
        baseSizeSlider.value = currentBaseSize;
    }

    private void InitializeSettings()
    {
        // Use a dummy training preferences if the SettingsManager isn't available
        trainingPrefs = SettingsManager.Instance.currentUser.trainingPrefs[labelNumber] ?? new Settings.TrainingPrefs();
    }

    private void InitializeListeners()
    {
        trainingLabelEntry.onEndEdit.AddListener(TrainingLabelChanged);

        colorDropdown.onValueChanged.AddListener(SetSaveTrainingPageColor);
    }

    private void InitializeViews()
    {
        PresentTrainingLabel();

        // Set the color dropdown to the color of the training page
        // SetTrainingPageColor(prefs.backgroundColor);

        PresentTrainingPageColor();
    }


    public void PresentTrainingLabel()
    {
        Debug.Log("updating training text");
        trainingLabelEntry.text = trainingPrefs.labelText;
    }

    public void TrainingLabelChanged(string labelText)
    {
        trainingPrefs.labelText = labelText;

        // update the tabs also
    }


    public void SetSaveTrainingPageColor(int colorIndex)
    {
        // Persist the setting for color
        if (SettingsManager.Instance != null)
        {
            Settings.User user = SettingsManager.Instance.currentUser;
            string colorText = colorDropdown.options[colorIndex].text;
            user.trainingPrefs[labelNumber].backgroundColor = ColorByName.colors[colorText];
        }
        else
        {
            Debug.LogWarning("SettingsManager not found");
        }

    }

    public void PresentTrainingPageColor()
    {

        // //Set Color of Tabs
        // foreach (Tab tab in GameObject.Find("TabArea").GetComponent<TabGroup>().tabs)
        // {
        //     // Look up our training preferences and apply to the view
        //     Settings.TrainingPrefs prefs = SettingsManager.Instance.currentUser.trainingPrefs[tab.transform.GetSiblingIndex()];
        //     tab.GetComponent<Image>().color = prefs.backgroundColor;
        // }
        // //Set color of Training Page
        // Debug.Log("This is my sibling index: " + transform.GetSiblingIndex());
        //transform.GetChild(0).gameObject.GetComponent<Image>().color = prefs.backgroundColor;

    }

    public void ChangeBackgroundColor()
    {
        // //Emily's way
        // get the first transform game object in child
        activeTraining = transform.GetChild(0).gameObject;
        Image imageComponent = activeTraining.GetComponent<Image>();
        string colorText = colorDropdown.options[colorDropdown.value].text;
        Color color = ColorByName.colors[colorText];
        //Alternative way to get the color name without needing a static ref, but using a scriptable object. Could be good for persisting changes.
        //Color color = trainingPageSO.colors[colorText];
        imageComponent.color = color;
        //This is the lazy bad way to do it
        // Set current tab's label to training label
        TabGroup tabGroup = GameObject.Find("TabArea").GetComponent<TabGroup>();
        tabGroup.MatchPageColor();
    }




    //resets the position and scale of the traning object
    public void ResetSPO()
    {
        _SPO.transform.position = originalPosition;
    }

    //changes the training object image property
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

    //changes the training object base size
    public void ModifyBaseSizeWithSlider()
    {
        currentBaseSize = baseSizeSlider.value;
        //Debug.Log("Base size changed to " + currentBaseSize);
        SetBaseSize(currentBaseSize);
    }

    //resets the base size
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
}
