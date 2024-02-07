using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;  // Reference to the VideoPlayer game object
    public RawImage videoPlayerRawImage;  // Reference to the RawImage component that the video displays on
    public GameObject videoSelectionPanel;  // Reference to the panel that contains the video selection UI
    public GameObject videoPlaybackPanel;  // Reference to the panel that contains the video playback UI
    public Button playButton, pauseButton, stopButton, chooseVideoButton;
    public VideoClip[] videoClips; // Assign VideoClips in Inspector instead of paths

    private void Awake()
    {
        // Setup button listeners
        playButton.onClick.AddListener(PlayVideo);
        pauseButton.onClick.AddListener(PauseVideo);
        stopButton.onClick.AddListener(StopVideo);
        chooseVideoButton.onClick.AddListener(GoToSelectionPanel);
        HideVideo();
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

