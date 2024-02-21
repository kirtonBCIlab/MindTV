using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoCellManager : MonoBehaviour
{
    //Settings for the video cell
    public Settings.VideoCell videoCell;

    [SerializeField]
    private Image backgroundCell;
    [SerializeField]
    

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
    public void UpdateBackgroundColor()
    {

    }    
}
