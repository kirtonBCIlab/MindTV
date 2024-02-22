using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;
using System.Collections.Generic;

public class VideoCellManager : MonoBehaviour
{
    //Settings for the video cell
    public Settings.VideoCell videoCell;

    [SerializeField]    private Image backgroundCell;
    [SerializeField]    private TMP_Dropdown backgroundColorDropdown;
    [SerializeField]    private TMP_InputField videoTitleInputField;
    [SerializeField]    private TMP_Text videoTitleText;
    [SerializeField]    private Toggle includeImageToggle;
    [SerializeField]    private Image imageGraphic;
   // [SerializeField]    private TMP_Dropdown videoClipDropdown;
    [SerializeField]    private TMP_Dropdown mentalCommandDropdown;
    [SerializeField]    private TMP_Text mentalCommandName;
     
    public RawImage previewImage; // Assign in Inspector
    private VideoPlayer videoPlayer; // Used for loading video frames
    private int videoIndex; // Index of the video clip this cell represents
    private static int instanceCount = 0; // Keep track of instantiated VideoSelectorCells

    void Start()
    {
        // Limit instance creation to prevent memory leak
        if (instanceCount >= 4)
        {
            Debug.LogWarning("Exceeding VideoSelectorCell limit. Consider reusing existing instances.");
            return;
        }
        instanceCount++;

        // Initialize VideoPlayer
        videoPlayer = gameObject.AddComponent<VideoPlayer>();
        videoPlayer.playOnAwake = false;
        videoPlayer.renderMode = VideoRenderMode.APIOnly; // We only need the video frame data
        if (SettingsManager.Instance.currentUser.videoCells.Count > 0)
        {
            videoCell = SettingsManager.Instance.currentUser.videoCells[0];
        }
        else 
        {
            videoCell = SettingsManager.Instance.currentUser.AddVideoCell();
        }

        // Initialize Listeners
        InitializeListeners();
        InitializeVideoCell();
    }

    public void InitializeListeners()
    {
        backgroundColorDropdown.onValueChanged.AddListener(delegate { UpdateCellColor(); });
        videoTitleInputField.onEndEdit.AddListener(delegate { UpdateVideoTitle(); });
        includeImageToggle.onValueChanged.AddListener(delegate { UpdateImage(); });
        //videoClipDropdown.onValueChanged.AddListener(delegate { ChangeVideoClip(); });
        mentalCommandDropdown.onValueChanged.AddListener(delegate { UpdateMentalCommand(); });
    }

    public void SetupCell(VideoClip videoClip, int index)
    {
        videoIndex = index;
        videoPlayer.clip = videoClip;
        StartCoroutine(LoadPreview());
    }

    public void SetVideoCell(Settings.VideoCell cell)
    {
        videoCell = cell;
        InitializeVideoCell();
    }

    public void InitializeVideoCell()
    {
        // Set the background color
        backgroundCell.color = videoCell.backgroundColor;
        string colorName = Settings.NameForColor(videoCell.backgroundColor);
        int colorIndex = backgroundColorDropdown.options.FindIndex(option => option.text == colorName);
        backgroundColorDropdown.value = colorIndex;

        // Set the video title
        videoTitleText.text = videoCell.videoTitle;
        videoTitleInputField.text = videoCell.videoTitle;

        // Set the image
        imageGraphic.gameObject.GetComponent<Image>().enabled = videoCell.includeGraphic;
        includeImageToggle.isOn = videoCell.includeGraphic;

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

        // Set the mental command label
        mentalCommandName.text = videoCell.mentalCommandLabel;
        mentalCommandDropdown.value = mentalCommandDropdown.options.FindIndex(option => option.text == videoCell.mentalCommandLabel);
    }

    IEnumerator LoadPreview()
    {
        videoPlayer.Prepare();
        while (!videoPlayer.isPrepared)
        {
            yield return null;
        }

        // Play and immediately pause to get the first frame
        videoPlayer.Play();
        videoPlayer.Pause();

        // Wait until the frame is ready
        yield return new WaitForEndOfFrame();

        // Set the preview image
        previewImage.texture = videoPlayer.texture;

        // Optionally, reset and cleanup
        videoPlayer.Stop();
    }

    void OnDestroy()
    {
        if (videoPlayer != null)
        {
            Destroy(videoPlayer.gameObject); // Cleanup VideoPlayer component
        }
        instanceCount--; // Update the count of existing instances
    }

    // Method to be called when this cell is selected
    public void OnSelect()
    {
        // Logic to handle video selection, possibly involving communication with VideoManager or another component
        Debug.Log($"Video {videoIndex} selected.");
    }

    // Update the Background Color of the cell
    public void UpdateCellColor()
    {
        string colorText = backgroundColorDropdown.options[backgroundColorDropdown.value].text;
        Color color = Settings.ColorForName(colorText);
        if (color == null)
        {
            Debug.LogError("ColorForName returned null for colorText: " + colorText);
            return;
        }

        backgroundCell.color = color;
        videoCell.backgroundColor = color;
    }

    // Update the Video Title of the cell
    public void UpdateVideoTitle()
    {
        videoTitleText.text = videoTitleInputField.text;
        videoCell.videoTitle = videoTitleInputField.text;
    }

    // Include Image in the cell
    public void UpdateImage()
    {
        imageGraphic.gameObject.GetComponent<Image>().enabled = includeImageToggle.isOn;
        videoCell.includeGraphic = includeImageToggle.isOn;
    }

    // Update the Mental Command Label of the cell
    public void UpdateMentalCommand()
    {
        string mentalCommand = mentalCommandDropdown.options[mentalCommandDropdown.value].text;
        mentalCommandName.text = mentalCommand;
        videoCell.mentalCommandLabel = mentalCommand;
    }
}
