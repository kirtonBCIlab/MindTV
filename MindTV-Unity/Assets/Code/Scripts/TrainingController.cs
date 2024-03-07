using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using BCIEssentials.Controllers;
using BCIEssentials.Utilities;
using BCIEssentials.StimulusObjects;
using BCIEssentials.ControllerBehaviors;

public class TrainingController : MonoBehaviour
{
    // UI elements within the TrainingPage prefab
    [SerializeField] private GameObject startTrainingButton;
    [SerializeField] private GameObject cancelCountdownButton;
    [SerializeField] private GameObject _SPO;
    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip countdownBeepFile;
    [SerializeField] private AudioClip startBeepFile;
    [SerializeField] private TMP_Text trainRemainingTimeText;
    public TMP_Text trainNumberCount;

    public int numberOfCountdownSeconds = 3;
    public string startTrainingMessage = "Go!";

    private int numberWindowsCompleted = 0;
    private float countdownBeepVolume = 1f; // Example volume level for quiet beep
    private float startBeepVolume = 1f; // Example volume level for loud beep
    private float uiUpdateDelay = 0.5f; // Delay for updating UI elements

    // Reference to training settings
    private Settings.TrainingPrefs trainingPrefs;
    public UnityEvent onTrainingNumberUpdated; // Event to trigger when the number of trainings is updated

    public void Start()
    {
        InitializeSettings();

        // Initialize Unity event for tracking number of training windows
        if (onTrainingNumberUpdated == null)
        {
            onTrainingNumberUpdated = new UnityEvent();
        }
    }

    private void InitializeSettings()
    {
        // Use the TrainingPage sibling index to look up the TrainingPrefs object from the data model.
        //  Use a dummy TrainingPrefs if one is not found.
        int pageIndex = transform.GetSiblingIndex();
        trainingPrefs = SettingsManager.Instance?.currentUser.trainingPrefs[pageIndex] ?? new Settings.TrainingPrefs();
    }


    public void StartTrainingCountdown()
    {
        startTrainingButton.SetActive(false);  // Hide the start training button
        cancelCountdownButton.SetActive(true); // Show the cancel countdown button
        StartCoroutine(Countdown(numberOfCountdownSeconds));
    }

    public void CancelCountdown()
    {
        StopAllCoroutines(); // Stop the countdown
        countdownText.text = ""; // Clear the countdown text
        cancelCountdownButton.SetActive(false); // Hide the cancel countdown button
        startTrainingButton.SetActive(true); // Re-show the start button
    }

    IEnumerator Countdown(int seconds)
    {
        yield return new WaitForSeconds(uiUpdateDelay); // Wait a bit before starting the countdown

        int count = seconds;

        while (count > 0)
        {
            countdownText.text = count.ToString();
            PlayBeep(countdownBeepFile, countdownBeepVolume);
            yield return new WaitForSeconds(1);
            count--;
        }

        cancelCountdownButton.SetActive(false); // Hide the cancel countdown button
        countdownText.text = startTrainingMessage;
        PlayBeep(startBeepFile, startBeepVolume);

        //TODO: This might need to be moved above the yield return statement - EKL
        // StartCoroutine(StartMyTraining()); // Start the actual training

        // Wait a bit before removing the countdown text
        yield return new WaitForSeconds(1);
        countdownText.text = ""; // Clear the countdown text
        //TODO: This might need to be moved above the yield return statement - EKL
        StartCoroutine(StartMyTraining()); // Start the actual training
    }

    void PlayBeep(AudioClip clip, float volume)
    {
        audioSource.PlayOneShot(clip, volume);
    }

    IEnumerator StartMyTraining()
    {
        Debug.Log("Starting training...");

        //Deal with scenarios when activate behavior isn't MI
        if (BCIController.Instance.ActiveBehavior.BehaviorType != BCIBehaviorType.MI)
        {
            Debug.Log("Active behavior is not MI.  Skipping training on this page.");
            yield break;
        }

        // Get settings for the training session
        int labelNumber = trainingPrefs.labelNumber;
        string labelName = trainingPrefs.labelName;
        float windowLength = trainingPrefs.windowLength;
        float trialLength = trainingPrefs.trialLength;

        // Calculate number of windows from trial length and window length
        int windowCount = Mathf.RoundToInt(trialLength / windowLength);

        // Assign the SPO object ID to be the same as the page number
        Debug.Log("Starting training on label: " + labelName + " (" + labelNumber + ")");
        Debug.Log("Trial length is " + trialLength + " (" + windowCount + " windows of " + windowLength + " seconds)");

        // Run the training
        if( BCIController.Instance.ActiveBehavior.SelectableSPOs.Count > 1)
        {
            BCIController.Instance.ActiveBehavior.StartTraining(BCITrainingType.Iterative);
        }
        else
        {
            BCIController.Instance.ActiveBehavior.StartTraining(BCITrainingType.Single);
        }
        
        // Start the animation
        UITweener uiTweener = _SPO.GetComponent<UITweener>();
        if (uiTweener != null)
        {
            uiTweener.HandleTween(); // Call the tween handling method for animation
        }

        // Start the timer for the training
        StartCoroutine(TrainingTimer((int)trialLength));

        // Wait to finish the training
        yield return new WaitForSeconds(trialLength);

        // Update the number of trainings done with the windowCount
        yield return new WaitForSeconds(uiUpdateDelay);
        UpdateNumberOfWindowsCompleted(windowCount);

        yield return null;
    }

    IEnumerator TrainingTimer(int trainingSeconds)
    {
        int timeLeft = trainingSeconds;
        while (timeLeft > 0)
        {
            trainRemainingTimeText.text = timeLeft.ToString(); // Show the remaining training time
            yield return new WaitForSeconds(1);
            timeLeft--;
        }

        trainRemainingTimeText.text = "0"; // Indicate that the training is complete

        // Wait a bit before clearing the training time text and re-showing the start button
        yield return new WaitForSeconds(uiUpdateDelay);
        trainRemainingTimeText.text = ""; // Clear the training time text

        startTrainingButton.SetActive(true); // Re-show the start button
    }

    private void UpdateNumberOfWindowsCompleted(int newWindowCount)
    {
        numberWindowsCompleted += newWindowCount;
        trainNumberCount.text = numberWindowsCompleted.ToString();
        
        // Emit the event to trigger the parent script to check the total training windows
        onTrainingNumberUpdated.Invoke();
    }



}
