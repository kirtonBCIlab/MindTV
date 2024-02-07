using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoSelectionManager : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Assign in Inspector
    public GameObject videoSelectionPanel;  // Reference to the panel that contains the video selection UI
    public GameObject videoPlaybackPanel;  // Reference to the panel that contains the video playback UI
    public VideoClip[] videoClips; // Assign VideoClips in Inspector instead of paths

    // Function to be called by button, index corresponds to button pressed
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
        // ResetVideoToFirstFrame(); // This might need adjustments for VideoClips
        GoToVideoPlaybackPanel();
    }

    private void GoToVideoPlaybackPanel()
    {
        videoSelectionPanel.SetActive(false);
        videoPlaybackPanel.SetActive(true);
        ResetVideoToFirstFrame();

        // If you want to automatically play the video upon switching panels
        // videoPlayer.Play();
    }

    // Adjusted to directly use VideoClips, might need further changes for your use case
    private void ResetVideoToFirstFrame()
    {
        videoPlayer.Stop(); // Ensure the player is stopped before changing the clip
        videoPlayer.Play();
        videoPlayer.Pause();
    }
}