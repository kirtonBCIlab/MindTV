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

    [SerializeField] private GameObject videoThumbnailButton;

    // Signal to parent that this cell was selected
    public static event Action<int> VideoCellSelected;

    // TODO - remove this when video selection working
    [SerializeField] private VideoClip temporaryVideoClip;

    // TODO - used to create a thumbnail, this is a bit hacky as the player hangs around
    private VideoPlayer videoPlayer;

    // Cache settings for the video cell, assigned by SetVideoCellPrefs()
    public Settings.VideoCellPrefs cellPrefs = new Settings.VideoCellPrefs();

    void Awake()
    {
        // TODO - used to create a thumbnail, this is a bit hacky as the player hangs around
        // A nice approach would be a VideoThumbnailFactory that produces a still image from
        // a video.  No need to persist the player in memory to show a thumbnail.
        videoPlayer = gameObject.AddComponent<VideoPlayer>();
        videoPlayer.playOnAwake = false;
        videoPlayer.renderMode = VideoRenderMode.APIOnly;
    }

    void Start()
    {
        InitializeListeners();
        InitializeViews();
    }

    // Called by VideoPageManager when VideoCell prefabs are created based on Settings
    public void SetVideoCellPrefs(Settings.VideoCellPrefs prefs)
    {
        cellPrefs = prefs;
        InitializeViews();
    }

    public void InitializeListeners()
    {
        backgroundColorDropdown.onValueChanged.AddListener(CellColorChanged);
        videoTitleInputField.onEndEdit.AddListener(VideoTitleChanged);
        includeImageToggle.onValueChanged.AddListener(ImageVisibilityChanged);
        mentalCommandDropdown.onValueChanged.AddListener(MentalCommandChanged);

        // TODO - video selection can tie in here
        //videoClipDropdown.onValueChanged.AddListener(VideoClipChanged);

        videoThumbnailButton.GetComponent<Button>().onClick.AddListener(ThumbNailButtonPressed);
    }

    public void InitializeViews()
    {
        UpdateMentalCommandOptions();
        UpdateMentalCommand();
        UpdateCellColor();
        UpdateVideoTitle();
        UpdateImage();
        UpdateImageVisibility();
        UpdateVideoThumbnail();
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
        mentalCommandName.text = cellPrefs.mentalCommandLabel;
        mentalCommandDropdown.value = mentalCommandDropdown.options.FindIndex(option => option.text == cellPrefs.mentalCommandLabel);
    }

    public void MentalCommandChanged(int labelIndex)
    {
        string mentalCommand = mentalCommandDropdown.options[labelIndex].text;
        cellPrefs.mentalCommandLabel = mentalCommand;
        UpdateMentalCommand();
        UpdateImage();
    }


    public void UpdateCellColor()
    {
        backgroundCell.color = cellPrefs.backgroundColor;

        // make drop down match color
        string colorName = Settings.NameForColor(cellPrefs.backgroundColor);
        int colorIndex = backgroundColorDropdown.options.FindIndex(option => option.text == colorName);
        backgroundColorDropdown.value = colorIndex;
    }

    public void CellColorChanged(int cellIndex)
    {
        string colorName = backgroundColorDropdown.options[cellIndex].text;
        Color color = Settings.ColorForName(colorName);
        cellPrefs.backgroundColor = color;
        UpdateCellColor();
    }


    public void UpdateVideoTitle()
    {
        videoTitleText.text = cellPrefs.videoTitle;
        videoTitleInputField.text = cellPrefs.videoTitle;
    }

    public void VideoTitleChanged(string title)
    {
        cellPrefs.videoTitle = title;
        UpdateVideoTitle();
    }


    public void UpdateImageVisibility()
    {
        imageGraphic.enabled = cellPrefs.includeGraphic;
        includeImageToggle.isOn = cellPrefs.includeGraphic;
    }

    public void ImageVisibilityChanged(bool isOn)
    {
        cellPrefs.includeGraphic = isOn;
        UpdateImageVisibility();
    }


    public void UpdateImage()
    {
        imageGraphic.sprite = SettingsManager.Instance?.currentUser.GetImageForLabel(cellPrefs.mentalCommandLabel);
    }


    public void VideoChanged(string path)
    {
        cellPrefs.videoPath = "";   // TODO - put the video path here
        UpdateVideoThumbnail();
    }

    public void UpdateVideoThumbnail()
    {
        // TODO - Replace with actual video clip, loaded from the file chosen by the user.
        // The temporaryVideoClip is here just to test that a thumbnail appears.
        //
        // videoClip = LoadVideoClipSomehow(videoCellPrefs.videoPath)
        //
        videoPlayer.clip = temporaryVideoClip;

        StartCoroutine(LoadPreview());
    }

    IEnumerator LoadPreview()
    {
        videoPlayer.Prepare();
        while (!videoPlayer.isPrepared)
        {
            yield return null;
        }

        videoPlayer.Play();
        videoThumbnailButton.GetComponent<RawImage>().texture = videoPlayer.texture;

        // TODO - figure out a way to capture the texture instead of leaving the player running.
        // If the player is stopped or destroyed, the thumbnail will disappear.
        videoPlayer.Pause();
    }


    public void ThumbNailButtonPressed()
    {
        // TODO - the SPO may also want to call this method to select the video

        // signal to parent that this video was chosen and provide details for playback
        VideoCellSelected?.Invoke(cellPrefs.cellNumber);
    }
}
