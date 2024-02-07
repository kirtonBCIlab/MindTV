using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoPlaybackManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;  // Reference to the VideoPlayer game object
    public GameObject videoSelectionPanel;  // Reference to the panel that contains the video selection UI
    public GameObject videoPlaybackPanel;  // Reference to the panel that contains the video playback UI
    public Button playButton, pauseButton, stopButton, chooseVideoButton;

    private void Awake()
    {
        // Setup button listeners
        playButton.onClick.AddListener(PlayVideo);
        pauseButton.onClick.AddListener(PauseVideo);
        stopButton.onClick.AddListener(StopVideo);
        chooseVideoButton.onClick.AddListener(GoToVideoSelectionPanel);
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

    private void GoToVideoSelectionPanel()
    {
        StopVideo();
        videoPlaybackPanel.SetActive(false);
        videoSelectionPanel.SetActive(true);

    }



    
}
