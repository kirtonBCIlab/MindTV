using System.Collections;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;
using System.Collections.Generic;
using SimpleFileBrowser;

public class VideoCellManager : MonoBehaviour
{
    [SerializeField] private Image backgroundCell;
    [SerializeField] private TMP_Dropdown backgroundColorDropdown;
    [SerializeField] private TMP_InputField videoTitleInputField;
    [SerializeField] private TMP_Text videoTitleText;
    [SerializeField] private Toggle includeImageToggle;
    [SerializeField] private Image imageGraphic;
    [SerializeField] private Button videoSelectButton;
    [SerializeField] private TMP_Dropdown mentalCommandDropdown;
    [SerializeField] private TMP_Text mentalCommandName;

    [SerializeField] private GameObject videoThumbnailButton;

    // Signal to parent that this cell was selected
    public static event Action<int> VideoCellSelected;

    // TODO - used to create a thumbnail, this is a bit hacky as the player hangs around
    private VideoPlayer videoPlayer;

    // Cache settings for the video cell, assigned by SetVideoCellPrefs()
    public Settings.VideoCellPrefs videoCellPrefs = new Settings.VideoCellPrefs();

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
        videoSelectButton.onClick.AddListener(VideoSelectPressed);
        videoThumbnailButton.GetComponent<Button>().onClick.AddListener(ThumbNailButtonPressed);
    }

    public void InitializeVideoCell()
    {
        UpdateMentalCommandOptions();
        UpdateMentalCommand();
        UpdateCellColor();
        UpdateVideoTitle();
        UpdateImage();
        UpdateImageVisibility();

        // TOOD - this isn't working on Windows videoThumbnailButton is invisible and not clickable
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


    public void VideoSelectPressed()
    {
        FileBrowser.SetFilters(false, new FileBrowser.Filter("Videos", "mp4"));
        FileBrowser.ShowLoadDialog(VideoSelected, VideoSelectCancelled, FileBrowser.PickMode.Files, allowMultiSelection: false);
    }

    public void VideoSelectCancelled()
    {
        Debug.Log("File select cancelled");
    }

    public void VideoSelected(string[] paths)
    {
        if (paths.Length > 0)
        {
            // User is responsible for maintaining video files, we don't copy them (could be large, etc)
            videoCellPrefs.videoPath = paths[0];
            UpdateVideoThumbnail();
        }
    }

    public void UpdateVideoThumbnail()
    {
        videoPlayer.url = videoCellPrefs.videoPath;
        StartCoroutine(LoadPreview());
    }

    IEnumerator LoadPreview()
    {
        videoPlayer.Prepare();
        while (!videoPlayer.isPrepared)
        {
            yield return null;
        }

        // TODO - not sure why this isn't working on Windows
        // TODO - figure out a way to capture the texture instead of leaving the player running.
        // If the player is stopped or destroyed, the thumbnail will disappear.
        videoPlayer.Play();
        videoThumbnailButton.GetComponent<RawImage>().texture = videoPlayer.texture;
        videoPlayer.Pause();

        yield return null;
    }


    public void ThumbNailButtonPressed()
    {
        // TODO - the SPO may also want to call this method to select the video

        // signal to parent that this video was chosen and provide details for playback
        VideoCellSelected?.Invoke(videoCellPrefs.tileNumber);
    }
}
