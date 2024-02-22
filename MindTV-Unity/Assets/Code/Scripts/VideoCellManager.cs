using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

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
    [SerializeField]    private TMP_Dropdown mentalCommandLabel;
    [SerializeField]    private TMP_Text mentalCommandName;
     
    public static event Action VideoCellChanged;

    public RawImage previewImage; // Assign in Inspector
    private VideoPlayer videoPlayer; // Used for loading video frames
    private int videoIndex; // Index of the video clip this cell represents
    private static int instanceCount = 0; // Keep track of instantiated VideoSelectorCells

    void Awake()
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

        // Initialize Listeners
        InitializeListeners();

    }

    public void InitializeListeners()
    {
        backgroundColorDropdown.onValueChanged.AddListener(delegate { UpdateCellColor(); });
        videoTitleInputField.onEndEdit.AddListener(delegate { UpdateVideoTitle(); });
        includeImageToggle.onValueChanged.AddListener(delegate { UpdateImage(); });
        //videoClipDropdown.onValueChanged.AddListener(delegate { ChangeVideoClip(); });
        mentalCommandLabel.onValueChanged.AddListener(delegate { UpdateMentalCommand(); });
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

        // Set the mental command label
        mentalCommandName.text = videoCell.mentalCommandLabel;
        mentalCommandLabel.value = mentalCommandLabel.options.FindIndex(option => option.text == videoCell.mentalCommandLabel);
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
        backgroundCell.color = color;
        videoCell.backgroundColor = color;
        VideoCellChanged();
    }

    // Update the Video Title of the cell
    public void UpdateVideoTitle()
    {
        videoTitleText.text = videoTitleInputField.text;
        videoCell.videoTitle = videoTitleInputField.text;
        VideoCellChanged();
    }

    // Include Image in the cell
    public void UpdateImage()
    {
        imageGraphic.gameObject.GetComponent<Image>().enabled = includeImageToggle.isOn;
        videoCell.includeGraphic = includeImageToggle.isOn;
        VideoCellChanged();
    }

    // Update the Mental Command Label of the cell
    public void UpdateMentalCommand()
    {
        string mentalCommand = mentalCommandLabel.options[mentalCommandLabel.value].text;
        mentalCommandName.text = mentalCommand;
        videoCell.mentalCommandLabel = mentalCommand;
        VideoCellChanged();
    }
}
