using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System;
using System.Collections.Generic;
using UnityEditor.SceneManagement;

public class VideoPageManager : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;  // Reference to the VideoPlayer game object
    [SerializeField] private RawImage videoPlayerRawImage;  // Reference to the RawImage component that the video displays on
    [SerializeField] private GameObject videoSelectionPanel;  // Reference to the panel that contains the video selection UI
    [SerializeField] private GameObject videoPlaybackPanel;  // Reference to the panel that contains the video playback UI
    [SerializeField] private Button playButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button stopButton;
    [SerializeField] private Button chooseVideoButton;

    [SerializeField] private Button addVideoCellButton;
    [SerializeField] private Transform videoCellParent;
    [SerializeField] private GameObject videoCellPrefab;

    // TODO - remove when we're looking up the video from videoCellPrefs (ie: user selection)
    public VideoClip[] videoClips = new VideoClip[4]; // Assign VideoClips in Inspector instead of paths

    // Event to signal when preferences have been changed. Think about changing this to UnityEvent instead of just Action.
    public static event Action VideoPrefsChanged;

    // Reference to training settings
    private List<Settings.VideoCellPrefs> videoCellPrefs;

    private void Start()
    {
        InitializeListeners();
        InitializeSettings();
        InitializeViews();

        // Hide the video playback raw image component to prevent displaying the video before it's selected
        HideVideo();
        videoSelectionPanel.SetActive(true);
        videoPlaybackPanel.SetActive(false);

        // Setup button listeners
        playButton.onClick.AddListener(PlayVideo);
        pauseButton.onClick.AddListener(PauseVideo);
        stopButton.onClick.AddListener(StopVideo);
        chooseVideoButton.onClick.AddListener(GoToSelectionPanel);
    }

    private void OnDisable()
    {
        VideoCellManager.VideoCellSelected -= ShowVideoForCell;
    }

    private void InitializeSettings()
    {
        // this contains all video cell preferences
        videoCellPrefs = SettingsManager.Instance?.currentUser.videoCellPrefs;
    }

    public void InitializeListeners()
    {
        addVideoCellButton.onClick.AddListener(AddVideoCell);
        VideoCellManager.VideoCellSelected += ShowVideoForCell;
    }

    private void InitializeViews()
    {
        // Create a new video cell for each video in the user's videoCells list
        foreach (Settings.VideoCellPrefs pref in videoCellPrefs)
        {
            GameObject videoCell = Instantiate(videoCellPrefab, videoCellParent, false);
            videoCell.GetComponent<VideoCellManager>().SetVideoCellPrefs(pref);
        }

        // Hide add cell if there's already four video cells
        addVideoCellButton.gameObject.SetActive(videoCellPrefs.Count < 4);
    }


    private void AddVideoCell()
    {
        // Add a new video cell prefs to settings, then create a video cell
        Settings.VideoCellPrefs pref = SettingsManager.Instance?.currentUser.AddVideoCell();
        GameObject videoCell = Instantiate(videoCellPrefab, videoCellParent, false);
        videoCell.GetComponent<VideoCellManager>().SetVideoCellPrefs(pref);

        // Hide the video cell button if we have too many now
        addVideoCellButton.gameObject.SetActive(videoCellPrefs.Count < 4);
    }


    private void ShowVideoForCell(int tileNumber)
    {
        videoSelectionPanel.SetActive(false);
        videoPlaybackPanel.SetActive(true);

        // TODO - VideoPageManager can look up video info it needs from videoCellPrefs
        // using the provided tileNumber.  This function should call the same thing the BCI 
        // integration does when a label is detected.
        Debug.Log("Showing video for cell number " + tileNumber);
    }


    private void ResetVideoToFirstFrame()
    {
        // Resets the displayed frame to the first frame of the video
        videoPlayer.frame = 0;
        videoPlayer.Play();
        videoPlayer.Pause();
        videoPlayer.frame = 0;
    }

    private void PlayVideo()
    {
        if (!videoPlayer.isPlaying)
        {
            videoPlayer.Play();
        }
    }

    private void PauseVideo()
    {
        if (videoPlayer.isPlaying)
        {
            videoPlayer.Pause();
        }
    }

    private void StopVideo()
    {
        videoPlayer.Stop();
        ResetVideoToFirstFrame();
        // if (videoPlayer.isPlaying || videoPlayer.isPaused)
        // {
        //     videoPlayer.Stop();
        //     ResetVideoToFirstFrame();
        // }
    }

    private void HideVideo()
    {
        // Hides the RawImage component to stop displaying the video
        videoPlayerRawImage.enabled = false;
    }

    private void ShowVideo()
    {
        // Shows the RawImage component to start displaying the video
        videoPlayerRawImage.enabled = true;
    }

    private void GoToSelectionPanel()
    {
        StopVideo();
        HideVideo();
        videoPlaybackPanel.SetActive(false);
        videoSelectionPanel.SetActive(true);
    }

    private void GoToPlaybackPanel()
    {
        videoSelectionPanel.SetActive(false);
        videoPlaybackPanel.SetActive(true);
        ShowVideo();
    }

    public void LoadSelectedVideo(int index)
    {
        if (videoClips == null || videoClips.Length == 0)
        {
            Debug.LogError("No video clips assigned.");
            return;
        }

        Debug.Log("Loading video " + index + " of " + videoClips.Length + " videos.");
        if (index < 0 || index >= videoClips.Length)
        {
            Debug.LogError("Selected video index out of range.");
            return;
        }

        // Assign the selected VideoClip to the VideoPlayer
        videoPlayer.clip = videoClips[index];

        // Optionally, you might want to reset and play the video here
        ResetVideoToFirstFrame();
        GoToPlaybackPanel();
    }
}

