using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using BCIEssentials.Controllers;

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
    [SerializeField] private TMP_Text trainNumberText;

    [SerializeField] private GameObject controllerManager;
    [SerializeField] private BCIController bciController;

    public int numberOfCountdownSeconds = 3;
    public string startTrainingMessage = "Go!";

    private int numberOfTrainingsDone = 0;
    private float countdownBeepVolume = 1f; // Example volume level for quiet beep
    private float startBeepVolume = 1f; // Example volume level for loud beep
    private float uiUpdateDelay = 0.5f; // Delay for updating UI elements

    // Reference to training settings
    private Settings.TrainingPrefs trainingPrefs;

    private void Awake()
    {
        if (bciController == null)
        {
            controllerManager = GameObject.FindGameObjectWithTag("ControllerManager");
            StartCoroutine(InitCoroutine());
        }
    }

    public void Start()
    {
        InitializeSettings();
    }

    private void InitializeSettings()
    {
        // Use the TrainingPage sibling index as the "label number".  This is needed to choose the correct
        // TrainingPrefs object from the data model.  Use a dummy TrainingPrefs if one is not found.
        int labelNumber = transform.GetSiblingIndex();
        trainingPrefs = SettingsManager.Instance?.currentUser.trainingPrefs[labelNumber] ?? new Settings.TrainingPrefs();
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
        // bciController.ActiveBehavior.StartTraining(BCITrainingType.Iterative); // Start the actual training

        StartCoroutine(StartMyTraining()); // Start the actual training

        // Wait a bit before removing the countdown text
        yield return new WaitForSeconds(1);
        countdownText.text = ""; // Clear the countdown text
        // Wait a bit before re-showing the start button - TEMPORARY
        yield return new WaitForSeconds(1);
        startTrainingButton.SetActive(true); // Re-show the start button
    }

    void PlayBeep(AudioClip clip, float volume)
    {
        audioSource.PlayOneShot(clip, volume);
    }

    private IEnumerator InitCoroutine()
    {
        yield return new WaitForEndOfFrame();
        // bciController = controllerManager.GetComponent<BCIController>();
    }

    IEnumerator StartMyTraining()
    {
        Debug.Log("Starting training...");

        // Find the SPOToyBox object in the scene get the SPO
        SPOToyBox spoToyBox = FindObjectOfType<SPOToyBox>();

        // Get settings for the training session
        int labelNumber = trainingPrefs.labelNumber;
        string labelName = trainingPrefs.labelName;
        float windowLength = trainingPrefs.windowLength;
        float trialLength = trainingPrefs.trialLength;

        // Calculate number of windows from trial length and window length
        int windowCount = Mathf.RoundToInt(trialLength / windowLength);

        if (string.IsNullOrEmpty(labelName))
        {
            labelName = "Unknown";
        }

        // Assign the SPO object ID to be the same as the page number
        Debug.Log("Starting training on label: " + labelName + " (" + labelNumber + ")");
        Debug.Log("Trial length is " + trialLength + " (" + windowCount + " windows of " + windowLength + " seconds)");
        Debug.Log("SPO is " + _SPO);

        // TODO - this is a null reference if application not started from main scene
        spoToyBox.SetSPO(labelNumber, _SPO, labelName);

        float trainingLengthSeconds = windowCount * windowLength;
        // int trainingLengthSecondsInt = (int)trainingLengthSeconds;

        // Update the length of the animation duration
        Debug.Log("_SPO.UITweener: Setting animation duration to " + trainingLengthSeconds);
        // uiTweener.duration = trainingLengthSeconds;


        // Do the actual training
        // Anup: I turned this off to get around package issues
        Debug.Log("BCIController: Do Single Training (Currently off)");
        // BCIController.WhileDoSingleTraining(_SPO, windowLength, windowCount);

        // Start the animation
        UITweener uiTweener = _SPO.GetComponent<UITweener>();
        if (uiTweener != null)
        {
            uiTweener.HandleTween(); // Call the tween handling method for animation
            Debug.Log("Calling _SPO.UITweener.HandleTween() to start animation");
        }

        // Start the timer for the training
        // StartCoroutine(TrainingTimer(trainingLengthSecondsInt));
        StartCoroutine(TrainingTimer((int)trainingLengthSeconds));

        // Wait to finish the training
        yield return new WaitForSeconds(trainingLengthSeconds);

        // Needs to be updated
        // bciController.ActiveBehavior.SetLabel(userInput); // Needs to be updated
        // bciController.ActiveBehavior.StartTraining(BCITrainingType.Iterative); // Needs to be updated

        // Update the number of trainings done with the windowCount
        yield return new WaitForSeconds(uiUpdateDelay);
        UpdateNumberOfTrainingsDone(windowCount);

        // // Anup: I turned this off to get around package issues
        // if (numberOfTrainingsDone >= 5) // Needs to be updated
        // {
        //     // UpdateTheClassifier();
        //     Debug.Log("BCIController: Update Classifier (Currently off)");
        // }

        yield return null;
    }

    IEnumerator TrainingTimer(int trainingSeconds)
    {
        Debug.Log("Running TrainingTimer coroutine");
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

    private void UpdateNumberOfTrainingsDone(int newWindowCount)
    {
        numberOfTrainingsDone += newWindowCount;
        trainNumberText.text = "Number of Trainings: " + numberOfTrainingsDone;

        // TODO - not sure what this is doing here
        BessyTrainClassifier parentScript = GetComponentInParent<BessyTrainClassifier>();
        if (parentScript != null)
        {
            parentScript.CheckTotalTrainingWindows();
        }
        else
        {
            Debug.LogError("BessyTrainClassifier script not found on parent!");
        }
    }



}
