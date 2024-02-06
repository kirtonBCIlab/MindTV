using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoPlaybackManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public Button playButton, pauseButton, stopButton;

    private void Awake()
    {
        // Set the video to the first frame
        ResetVideoToFirstFrame();

        // Setup button listeners
        playButton.onClick.AddListener(PlayVideo);
        pauseButton.onClick.AddListener(PauseVideo);
        stopButton.onClick.AddListener(StopVideo);
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
        if (!videoPlayer.isPlaying)
        {
            videoPlayer.Play();
        }
    }

    public void PauseVideo()
    {
        if (videoPlayer.isPlaying)
        {
            videoPlayer.Pause();
        }
    }

    public void StopVideo()
    {
        if (videoPlayer.isPlaying || videoPlayer.isPaused)
        {
            videoPlayer.Stop();
            ResetVideoToFirstFrame();
            // videoPlayer.frame = 0;
            // // Optionally, seek to the start to immediately show the first frame
            // videoPlayer.Play();
            // videoPlayer.Pause();
            // videoPlayer.frame = 0;
        }
    }

    
}
