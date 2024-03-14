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
    [SerializeField] private GameObject playButtonGO;
    [SerializeField] private GameObject pauseButtonGO;
    [SerializeField] private GameObject stopButtonGO;
    [SerializeField] private GameObject chooseVideoButtonGO;

    [SerializeField] private Button addVideoCellButton;
    [SerializeField] private Button removeVideoCellButton;
    [SerializeField] private Transform videoCellParent;
    [SerializeField] private GameObject videoCellPrefab;

    // Event to signal when preferences have been changed. Think about changing this to UnityEvent instead of just Action.
    //public static event Action VideoPrefsChanged;

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
        removeVideoCellButton.onClick.AddListener(RemoveVideoCell);
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


    public void ShowSelectionPanel()
    {
        ToggleOffMentalCommand();

        StopVideo();
        HideVideo();
        videoPlaybackPanel.SetActive(false);

        videoSelectionPanel.SetActive(true);
        ShowVideoCellAddRemoveButton();
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

        // Refresh button state
        ShowVideoCellAddRemoveButton();
    }

    private void RemoveVideoCell()
    {
        // The simplest way to do this is to just remove the last cell.
        // This avoids having to re-order the cellNumber, etc.
        SettingsManager.Instance?.currentUser.RemoveLastVideoCell();
        Destroy(videoCellParent.transform.GetChild(videoCellParent.childCount - 1).gameObject);

        // Refresh button state
        ShowVideoCellAddRemoveButton();
    }

    private void ShowVideoCellAddRemoveButton()
    {
        addVideoCellButton.gameObject.SetActive(videoCellPrefs.Count < 4);
        removeVideoCellButton.gameObject.SetActive(videoCellPrefs.Count > 0);
    }

    private void HideVideoCellAddButton()
    {
        addVideoCellButton.gameObject.SetActive(false);
        removeVideoCellButton.gameObject.SetActive(false);
    }


    private void ShowVideoForCell(int cellNumber)
    {
        // If this isn't working, check that perisited settings have correct cell number
        // It may be necessary to remove UserData.dat and start again.
        Debug.Log("Showing video for cell number " + cellNumber);
        videoPlayer.url = videoCellPrefs[cellNumber].videoPath;

        ShowPlaybackPanel();
    }


    private void ResetVideoToFirstFrame()
    {
        // Resets the displayed frame to the first frame of the video
        videoPlayer.frame = 0;
        videoPlayer.Play();
        videoPlayer.Pause();
        videoPlayer.frame = 0;
    }

    public void PlayVideo()
    {
        ToggleOffMentalCommand();

        if (!videoPlayer.isPlaying)
        {
            videoPlayer.Play();
        }
    }

    public void PauseVideo()
    {
        ToggleOffMentalCommand();

        if (videoPlayer.isPlaying)
        {
            videoPlayer.Pause();
        }
    }

    public void StopVideo()
    {
        ToggleOffMentalCommand();

        videoPlayer.Stop();
        ResetVideoToFirstFrame();
    }

    private void HideVideo()
    {
        // Hides the RawImage component to stop displaying the video
        videoPlayerRawImage.enabled = false;
    }

    private void ShowVideo()
    {
        ResetVideoToFirstFrame();
        // Shows the RawImage component to start displaying the video
        videoPlayerRawImage.enabled = true;
    }

    private void ToggleOffMentalCommand()
    {
        //Find the MentalCommandOnOffSwitch and turn it off
        MentalCommandOnOffSwitch mentalCommandOnOffSwitch = FindObjectOfType<MentalCommandOnOffSwitch>();
        mentalCommandOnOffSwitch.ToggleMentalCommandOff();
    }
}

