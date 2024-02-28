using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System;
using System.Collections.Generic;

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

    // Event to signal when preferences have been changed. Think about changing this to UnityEvent instead of just Action.
    public static event Action VideoPrefsChanged;

    // Reference to training settings
    private List<Settings.VideoCellPrefs> videoCellPrefs;

    private void Start()
    {
        InitializeListeners();
        InitializeSettings();
        InitializeViews();
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
        playButton.onClick.AddListener(PlayVideo);
        pauseButton.onClick.AddListener(PauseVideo);
        stopButton.onClick.AddListener(StopVideo);
        chooseVideoButton.onClick.AddListener(ShowSelectionPanel);

        addVideoCellButton.onClick.AddListener(AddVideoCell);
        VideoCellManager.VideoCellSelected += ShowVideoForCell;
    }

    private void InitializeViews()
    {
        ShowSelectionPanel();

        // Create a new video cell for each video in the user's videoCells list
        foreach (Settings.VideoCellPrefs pref in videoCellPrefs)
        {
            GameObject videoCell = Instantiate(videoCellPrefab, videoCellParent, false);
            videoCell.GetComponent<VideoCellManager>().SetVideoCellPrefs(pref);
        }
    }


    private void ShowSelectionPanel()
    {
        StopVideo();
        HideVideo();
        videoPlaybackPanel.SetActive(false);

        videoSelectionPanel.SetActive(true);
        ShowVideoCellAddButton();
    }

    private void ShowPlaybackPanel()
    {
        videoSelectionPanel.SetActive(false);
        HideVideoCellAddButton();

        videoPlaybackPanel.SetActive(true);
        ShowVideo();
    }


    private void AddVideoCell()
    {
        // Add a new video cell prefs to settings, then create a video cell
        Settings.VideoCellPrefs pref = SettingsManager.Instance?.currentUser.AddVideoCell();
        GameObject videoCell = Instantiate(videoCellPrefab, videoCellParent, false);
        videoCell.GetComponent<VideoCellManager>().SetVideoCellPrefs(pref);

        // Hide the video cell button if we have too many now
        ShowVideoCellAddButton();
    }

    private void ShowVideoCellAddButton()
    {
        addVideoCellButton.gameObject.SetActive(videoCellPrefs.Count < 4);
    }

    private void HideVideoCellAddButton()
    {
        addVideoCellButton.gameObject.SetActive(false);
    }


    private void ShowVideoForCell(int cellNumber)
    {
        ShowPlaybackPanel();

        // If this isn't working, check that perisited settings have correct cell number
        // It may be necessary to remove UserData.dat and start again.
        Debug.Log("Showing video for cell number " + cellNumber);
        videoPlayer.url = videoCellPrefs[cellNumber].videoPath;
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
}

