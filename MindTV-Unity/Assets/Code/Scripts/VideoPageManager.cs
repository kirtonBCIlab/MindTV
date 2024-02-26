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
    public VideoClip[] videoClips = new VideoClip[4]; // Assign VideoClips in Inspector instead of paths
    public RawImage[] videoThumbnails = new RawImage[4]; // Assign RawImages in Inspector to display video thumbnails
    public VideoPlayer previewVideoPlayer; // Reference to the VideoPlayer game object for the video preview
    public RawImage previewVideoRawImage; // Reference to the RawImage component that the video preview displays on

    // Event to signal when preferences have been changed. Think about changing this to UnityEvent instead of just Action.
    public static event Action VideoPrefsChanged;

    // Reference to training settings
    private List<Settings.VideoCellPrefs> videoCellPrefs;
    private Settings.TrainingPrefs trainingPrefs;
    [SerializeField] private GameObject videoCellPrefab;
    [SerializeField] private Transform videoCellParent;
    private void Awake()
    {
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

        // Hides the RawImage component of the video previewer to stop displaying the video
        // previewVideoRawImage.enabled = false;
        // StartCoroutine(GenerateAllPreviews()); 
    }

    private void InitializeSettings()
    {
        // this contains all video cell preferences
        videoCellPrefs = SettingsManager.Instance?.currentUser.videoCellPrefs;
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

    // IEnumerator GenerateAllPreviews()
    // {
    //     for (int i = 0; i < videoClips.Length; i++)
    //     {
    //         // yield return StartCoroutine(GeneratePreview(i));
    //         Debug.Log("Generating preview for video " + i);
    //         yield return GeneratePreview(i);
    //     }
    // }

    // IEnumerator GeneratePreview(int index)
    // {
    //     if (index < 0 || index >= videoClips.Length || index >= videoThumbnails.Length)
    //     {
    //         Debug.LogError("Index out of range for generating video preview.");
    //         yield break;
    //     }

    //     // Dispose of the previous texture to prevent memory leaks
    //     if (videoThumbnails[index].texture != null)
    //     {
    //         Destroy(videoThumbnails[index].texture);
    //     }

    //     previewVideoPlayer.clip = videoClips[index];
    //     previewVideoPlayer.frame = 0;
    //     previewVideoPlayer.Play();
    //     previewVideoPlayer.Pause();

    //     // Wait until the video player has prepared the frame
    //     // while (!previewVideoPlayer.isPrepared)
    //     // {
    //     //     yield return null;
    //     // }

    //     // Wait until the video player has prepared the frame
    //     yield return new WaitUntil(() => previewVideoPlayer.isPrepared);

    //     // Debug.Log("Video " + index + " is being clipped.");
    //     // Debug.Log(previewVideoPlayer.texture.width + " " + previewVideoPlayer.texture.height);
    //     // Now that the frame is ready, assign it to the corresponding RawImage
    //     // videoThumbnails[index].texture = previewVideoPlayer.texture;

    //     // Now create a new Texture2D and copy the current frame
    //     Texture2D frameTexture = new Texture2D(previewVideoPlayer.texture.width, previewVideoPlayer.texture.height, TextureFormat.RGBA32, false);
    //     RenderTexture.active = previewVideoPlayer.texture as RenderTexture;
    //     frameTexture.ReadPixels(new Rect(0, 0, frameTexture.width, frameTexture.height), 0, 0);
    //     frameTexture.Apply();

    //     // Assign this new texture to the corresponding RawImage
    //     videoThumbnails[index].texture = frameTexture;

    //     previewVideoPlayer.Stop();
    // }

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

