using BCIEssentials.ControllerBehaviors;
using BCIEssentials.StimulusObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class StimulusManager : MonoBehaviour
{
    [SerializeField] private TrainingMenuController trainingController;
    [SerializeField] private TMP_Text countDownText;
    [SerializeField] private GameObject _SPO;

    // This identifies which training tab
    public int labelNumber;

    public TMP_InputField trainingLabelEntry;
    public TMP_Dropdown colorDropdown;
    public TMP_Dropdown animDropdown;
    private UITweener tweener;

    private GameObject activeTraining;

    private MIControllerBehavior controllerBehaviour;

    //Exposing this so that we can change the base size of the training object
    public float originalBaseSize = 100.0f;
    private float currentBaseSize;
    public float targetImageResolution = 512f;
    private Vector3 originalPosition;
    public Slider baseSizeSlider;

    void Start()
    {
        trainingController = GameObject.FindGameObjectWithTag("TrainingPanel").GetComponent<TrainingMenuController>();
        originalPosition = _SPO.transform.position;

        currentBaseSize = originalBaseSize;
        _SPO.transform.localScale = new Vector3(currentBaseSize, currentBaseSize, currentBaseSize);
        baseSizeSlider.value = currentBaseSize;

        InitializeListeners();
        InitializeViews();
    }

    void InitializeListeners()
    {
        trainingLabelEntry.onEndEdit.AddListener(SetTrainingLabel);
    }

    void InitializeViews()
    {
        // Look up our training preferences and apply to the view
        if (SettingsManager.Instance != null)
        {
            Settings.User user = SettingsManager.Instance.currentUser;
            Settings.TrainingPrefs prefs = user.trainingPrefs[labelNumber];

            SetTrainingLabel(prefs.labelText);
        }
        else
        {
            Debug.LogWarning("SettingsManager not found");
        }
    }


    //resets the position and scale of the traning object
    void ResetSPO()
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

    public void SetTrainingLabel(string labelText)
    {
        // Persist the setting
        if (SettingsManager.Instance != null)
        {
            Settings.User user = SettingsManager.Instance.currentUser;
            user.trainingPrefs[labelNumber].labelText = labelText;
            Debug.Log("These are my training labels: " + user.trainingPrefs[labelNumber].labelText);
        }

        // Initialize the input field
        trainingLabelEntry.text = labelText;

        // Set current tab's label to training label
        TabGroup tabGroup = GameObject.Find("TabArea").GetComponent<TabGroup>();
        
        //This is repeated code, and we should put it at the top of the method probably.
        if (SettingsManager.Instance != null && tabGroup != null)
        {
            Settings.User user = SettingsManager.Instance.currentUser;
            // Set the label for each tab under tab group based on their index
            foreach (Tab tab in tabGroup.tabs)
            {
                tab.GetComponentInChildren<TextMeshProUGUI>().text = user.trainingPrefs[tab.transform.GetSiblingIndex()].labelText;
            }
        }
        else
        {
            Debug.LogWarning("SettingsManager and tabGroup not found");
        }

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

    public void SetAnimationOnSelection()
    {
        string animText = animDropdown.options[animDropdown.value].text;
        tweener = _SPO.GetComponent<UITweener>();
        tweener.SetTweenFromString(animText);
        Debug.Log("end of method in stim manager");
    }
}
