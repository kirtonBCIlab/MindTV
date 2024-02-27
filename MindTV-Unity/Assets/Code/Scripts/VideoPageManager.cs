using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System;
using System.Collections.Generic;

public class VideoPageManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;  // Reference to the VideoPlayer game object
    public RawImage videoPlayerRawImage;  // Reference to the RawImage component that the video displays on
    public GameObject videoSelectionPanel;  // Reference to the panel that contains the video selection UI
    public GameObject videoPlaybackPanel;  // Reference to the panel that contains the video playback UI
    public Button playButton, pauseButton, stopButton, chooseVideoButton;

    // TODO - remove when we're looking up the video from videoCellPrefs (ie: user selection)
    public VideoClip[] videoClips = new VideoClip[4]; // Assign VideoClips in Inspector instead of paths

    // Event to signal when preferences have been changed. Think about changing this to UnityEvent instead of just Action.
    public static event Action VideoPrefsChanged;

    // Reference to training settings
    private List<Settings.VideoCellPrefs> videoCellPrefs;
    [SerializeField] private GameObject videoCellPrefab;
    [SerializeField] private Transform videoCellParent;

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
        VideoCellManager.VideoCellSelected += ShowVideoForCell;
    }

    private void InitializeViews()
    {
        // Create a new video cell for each video in the user's videoCells list
        foreach (Settings.VideoCellPrefs videoCell in videoCellPrefs)
        {
            GameObject newVideoCell = Instantiate(videoCellPrefab, videoCellParent, false);
            newVideoCell.GetComponent<VideoCellManager>().SetVideoCellPrefs(videoCell);
        }
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

