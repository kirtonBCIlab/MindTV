using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;
using System.Collections.Generic;

public class VideoCellManager : MonoBehaviour
{
    [SerializeField] private Image backgroundCell;
    [SerializeField] private TMP_Dropdown backgroundColorDropdown;
    [SerializeField] private TMP_InputField videoTitleInputField;
    [SerializeField] private TMP_Text videoTitleText;
    [SerializeField] private Toggle includeImageToggle;
    [SerializeField] private Image imageGraphic;
    // [SerializeField]    private TMP_Dropdown videoClipDropdown;
    [SerializeField] private TMP_Dropdown mentalCommandDropdown;
    [SerializeField] private TMP_Text mentalCommandName;

    public RawImage previewImage; // Assign in Inspector
    private VideoPlayer videoPlayer; // Used for loading video frames
    private int videoIndex; // Index of the video clip this cell represents
    private static int instanceCount = 0; // Keep track of instantiated VideoSelectorCells

    // Cache settings for the video cell, assigned by SetVideoCellPrefs()
    public Settings.VideoCellPrefs videoCellPrefs = new Settings.VideoCellPrefs();

    void Start()
    {
        // Limit instance creation to prevent memory leak
        if (instanceCount >= 4)
        {
            Debug.LogWarning("Exceeding VideoSelectorCell limit. Consider reusing existing instances.");
            return;
        }
        instanceCount++;

        // TODO - do we need to persist this just to get a thumbnail?
        // Initialize VideoPlayer
        videoPlayer = gameObject.AddComponent<VideoPlayer>();
        videoPlayer.playOnAwake = false;
        videoPlayer.renderMode = VideoRenderMode.APIOnly;

        // Initialize Listeners
        InitializeListeners();
        InitializeVideoCell();
    }

    // Called by VideoPageManager when VideoCell prefabs are created based on Settings
    public void SetVideoCellPrefs(Settings.VideoCellPrefs prefs)
    {
        videoCellPrefs = prefs;
        InitializeVideoCell();
    }

    public void InitializeListeners()
    {
        backgroundColorDropdown.onValueChanged.AddListener(CellColorChanged);
        videoTitleInputField.onEndEdit.AddListener(VideoTitleChanged);
        includeImageToggle.onValueChanged.AddListener(ImageVisibilityChanged);
        mentalCommandDropdown.onValueChanged.AddListener(MentalCommandChanged);

        // TODO - add changing video clip
        //videoClipDropdown.onValueChanged.AddListener(VideoClipChanged);
    }

    public void InitializeVideoCell()
    {
        UpdateMentalCommandOptions();
        UpdateMentalCommand();
        UpdateCellColor();
        UpdateVideoTitle();
        UpdateImage();
        UpdateImageVisibility();
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
        mentalCommandName.text = videoCellPrefs.mentalCommandLabel;
        mentalCommandDropdown.value = mentalCommandDropdown.options.FindIndex(option => option.text == videoCellPrefs.mentalCommandLabel);
    }

    public void MentalCommandChanged(int labelIndex)
    {
        string mentalCommand = mentalCommandDropdown.options[mentalCommandDropdown.value].text;
        videoCellPrefs.mentalCommandLabel = mentalCommand;
        UpdateMentalCommand();
        UpdateImage();
    }


    public void UpdateCellColor()
    {
        backgroundCell.color = videoCellPrefs.backgroundColor;

        // make drop down match color
        string colorName = Settings.NameForColor(videoCellPrefs.backgroundColor);
        int colorIndex = backgroundColorDropdown.options.FindIndex(option => option.text == colorName);
        backgroundColorDropdown.value = colorIndex;
    }

    public void CellColorChanged(int cellIndex)
    {
        string colorName = backgroundColorDropdown.options[cellIndex].text;
        Color color = Settings.ColorForName(colorName);
        videoCellPrefs.backgroundColor = color;
        UpdateCellColor();
    }


    public void UpdateVideoTitle()
    {
        videoTitleText.text = videoCellPrefs.videoTitle;
        videoTitleInputField.text = videoCellPrefs.videoTitle;
    }

    public void VideoTitleChanged(string title)
    {
        videoCellPrefs.videoTitle = title;
        UpdateVideoTitle();
    }


    public void UpdateImageVisibility()
    {
        imageGraphic.enabled = videoCellPrefs.includeGraphic;
        includeImageToggle.isOn = videoCellPrefs.includeGraphic;
    }

    public void ImageVisibilityChanged(bool isOn)
    {
        videoCellPrefs.includeGraphic = isOn;
        UpdateImageVisibility();
    }


    public void UpdateImage()
    {
        imageGraphic.sprite = SettingsManager.Instance?.currentUser.GetImageForLabel(videoCellPrefs.mentalCommandLabel);
    }


    public void VideoClipChanged(string path)
    {
        // TODO - implement changing video, not sure if this menthod gets a path 
    }


    // TODO - reorganize how video thumbnail is found
    public void SetupCell(VideoClip videoClip, int index)
    {
        videoIndex = index;
        videoPlayer.clip = videoClip;
        StartCoroutine(LoadPreview());
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


    public void OnSelect()
    {
        // Logic to handle video selection, possibly involving communication with VideoManager or another component
        Debug.Log($"Video {videoIndex} selected.");
    }
}
