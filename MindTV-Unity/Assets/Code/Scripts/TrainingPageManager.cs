using BCIEssentials.ControllerBehaviors;
using BCIEssentials.StimulusObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEditor;

public class TrainingPageManager : MonoBehaviour
{
    // UI elements within the TrainingPage prefab
    [SerializeField] private GameObject _SPO;
    [SerializeField] private Button inventoryButton;
    [SerializeField] private TMP_InputField trainingLabelEntry;
    [SerializeField] private TMP_Dropdown colorDropdown;
    [SerializeField] private TMP_Dropdown animDropdown;
    [SerializeField] private Slider imageSizeSlider;
    [SerializeField] private Button imageSizeResetButton;
    [SerializeField] private GameObject activeTrainingFrame;
    [SerializeField] private GameObject trainingOptionsFrame;
    [SerializeField] private GameObject displayStartTrainingButton;
    [SerializeField] private GameObject displayNumberOfTimesTrained;
    [SerializeField] private TMP_Dropdown trialLengthDropdown;

    // Event to signal when preferences have been changed
    public static event Action TrainingPrefsChanged;

    // Reference to training settings
    private Settings.TrainingPrefs trainingPrefs;

    //Exposing this so that we can change the base size of the training object
    private float originalBaseSize = 100.0f;
    private float targetImageResolution = 512f;

    private Vector3 originalPosition;
    private UITweener tweener;


    private void Start()
    {
        InitializeSettings();
        InitializeViews();
        InitializeListeners();

        // TODO - move this to a helper
        originalPosition = _SPO.transform.position;

        // TODO - move to a helper
        float currentBaseSize = trainingPrefs.imageBaseSize;
        _SPO.transform.localScale = new Vector3(currentBaseSize, currentBaseSize, currentBaseSize);
        imageSizeSlider.value = currentBaseSize;
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
        // Update the UI to match current settings
        UpdateTrainingLabel();
        UpdateTrainingPageColor();
        UpdateTrialLength();
        UpdateAnimation();
        UpdateImageSize();
        UpdateImage();
    }

    private void InitializeListeners()
    {
        inventoryButton.onClick.AddListener(ToggleInventoryVisibility);
        trainingLabelEntry.onEndEdit.AddListener(LabelChanged);
        colorDropdown.onValueChanged.AddListener(ColorChanged);
        trialLengthDropdown.onValueChanged.AddListener(TrialLengthChanged);
        animDropdown.onValueChanged.AddListener(AnimationChanged);
        imageSizeSlider.onValueChanged.AddListener(ImageSizeChanged);
        imageSizeResetButton.onClick.AddListener(ResetImageSize);
    }


    public void ToggleInventoryVisibility()
    {
        // Hide/show UI elements for configuring the training page
        trainingOptionsFrame.SetActive(!trainingOptionsFrame.activeSelf);
        displayStartTrainingButton.SetActive(!displayStartTrainingButton.activeSelf);
        displayNumberOfTimesTrained.SetActive(!displayNumberOfTimesTrained.activeSelf);
    }


    public void UpdateTrainingLabel()
    {
        trainingLabelEntry.text = trainingPrefs.labelName;
    }

    public void LabelChanged(string labelText)
    {
        trainingPrefs.labelName = labelText;
        TrainingPrefsChanged?.Invoke();
    }


    public void UpdateTrainingPageColor()
    {
        // set background color
        Image imageComponent = activeTrainingFrame.GetComponent<Image>();
        imageComponent.color = trainingPrefs.backgroundColor;

        // set the dropdown to color from settings
        string colorName = Settings.NameForColor(trainingPrefs.backgroundColor);
        int colorIndex = colorDropdown.options.FindIndex(option => option.text == colorName);
        colorDropdown.value = colorIndex;
    }

    public void ColorChanged(int colorIndex)
    {
        // translate drop down text into a color and persist
        string colorName = colorDropdown.options[colorIndex].text;
        Color color = Settings.ColorForName(colorName);
        trainingPrefs.backgroundColor = color;

        UpdateTrainingPageColor();
        TrainingPrefsChanged?.Invoke();
    }


    public void UpdateTrialLength()
    {
        float trialLength = trainingPrefs.trialLength;

        // set the dropdown to the length from settings
        string trialLengthName = Settings.NameForTrialLength(trialLength);
        int index = trialLengthDropdown.options.FindIndex(option => option.text == trialLengthName);
        trialLengthDropdown.value = index;
    }

    public void TrialLengthChanged(int trialLengthIndex)
    {
        // Get the string label of the TMP dropdown and convert it to a float
        string trialLengthName = trialLengthDropdown.options[trialLengthIndex].text;
        float trialLength = Settings.TrialLengthForName(trialLengthName);
        trainingPrefs.trialLength = trialLength;

        UpdateTrialLength();
        UpdateAnimation();
    }


    public void UpdateAnimation()
    {
        float trialLength = trainingPrefs.trialLength;
        string animationName = trainingPrefs.animationName;

        // Set the dropdown to animation from settings
        int index = animDropdown.options.FindIndex(option => option.text == animationName);
        animDropdown.value = index;

        // Set the animation type and length of the UITWeener
        UITweener uiTweener = _SPO.GetComponent<UITweener>();
        uiTweener.SetTweenFromString(animationName);
        uiTweener.duration = trialLength;

        // Additional configuration based on type and length
        if (uiTweener != null)
        {
            string tweenAnimation = uiTweener.GetTweenAnimation();
            Debug.Log("TrainingObjectSPO.UITweener: Tween animation is " + tweenAnimation);
            switch (tweenAnimation)
            {
                case "Bounce":
                    break;
                case "Rotate":
                    break;
                case "RotatePunch":
                    break;
                case "Grow":
                    break;
                case "Left":
                    break;
                case "Right":
                    break;
                case "Up":
                    break;
                case "Down":
                    break;
                case "Shake":
                    uiTweener.shakeSpeed = 0.25f; // This shake speed will do 8 shakes per 2 second window
                    uiTweener.numShakes = (int)Math.Round(uiTweener.duration / uiTweener.shakeSpeed); // Scale the number of shakes to the target trial length
                    break;
                case "Wiggle":
                    uiTweener.wiggleSpeed = 0.5f; // This wiggle speed will do 4 shakes per 2 second window
                    uiTweener.numWiggles = (int)Math.Round(uiTweener.duration / uiTweener.wiggleSpeed); // Scale the number of wiggles to the target trial length
                    break;
                default:
                    Debug.Log("No tween animation selected.");
                    break;
            }
        }
    }

    public void AnimationChanged(int animationIndex)
    {
        string animationName = animDropdown.options[animationIndex].text;
        trainingPrefs.animationName = animationName;

        UpdateAnimation();
    }


    public void UpdateImageSize()
    {
        float currentBaseSize = trainingPrefs.imageBaseSize;
        imageSizeSlider.value = currentBaseSize;

        SpriteRenderer spriteRenderer = _SPO.GetComponent<SpriteRenderer>();
        float uniformScaleFactor = UniformImageSizeScaleFactor(spriteRenderer);
        float scaledSize = currentBaseSize * uniformScaleFactor;
        _SPO.transform.localScale = new Vector3(scaledSize, scaledSize, scaledSize);
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

    public void ResetImageSize()
    {
        trainingPrefs.imageBaseSize = originalBaseSize;
        _SPO.transform.position = originalPosition;
        UpdateImageSize();
    }

    public void ImageSizeChanged(float value)
    {
        trainingPrefs.imageBaseSize = value;
        UpdateImageSize();
    }


    public void UpdateImage()
    {
        string path = trainingPrefs.imagePath;
        Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
        ImageChanged(sprite);
    }

    // TODO - this is called by the individual InventorySlot objects.  Refactor to work like
    // the other UI elements managed by TrainingPageController, ie: use an image changed event, 
    // or have TrainingPageController attach listeners to individual InventorySlot objects.

    // TODO - replace this to operate with just the name of the sprite we want
    // The inventory list becomes just a list of names and displayed sprites
    public void ImageChanged(Sprite sprite)
    {
        // This may not work with user provided assets
        trainingPrefs.imagePath = AssetDatabase.GetAssetPath(sprite);
        _SPO.GetComponent<SpriteRenderer>().sprite = sprite;
    }
}
