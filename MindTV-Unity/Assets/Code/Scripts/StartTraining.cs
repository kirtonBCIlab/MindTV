using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using BCIEssentials.Controllers;

public class StartTraining : MonoBehaviour
{
    [SerializeField] private GameObject controllerManager;
    [SerializeField] private GameObject startTrainingButton;
    [SerializeField] private GameObject cancelCountdownButton;
    [SerializeField] private BCIController bciController;
    [SerializeField] private TrainingPageManager trainingPageManager;
    [SerializeField] private TMP_Text countdownText; // Assign in the Inspector
    [SerializeField] private AudioSource audioSource; // Assign in the Inspector
    [SerializeField] private AudioClip countdownBeepFile; // Assign in the Inspector
    [SerializeField] private AudioClip startBeepFile; // Assign in the Inspector
    private float countdownBeepVolume = 1f; // Example volume level for quiet beep
    private float startBeepVolume = 1f; // Example volume level for loud beep
    [SerializeField] private int numberOfCountdownSeconds = 3; // Number of seconds to countdown from
    [SerializeField] private string startTrainingMessage = "Go!"; // Text to display when training starts
    [SerializeField] private TMP_Text trainRemainingTimeText; // Assign in the Inspector
    private float uiUpdateDelay = 0.5f; // Delay for updating UI elements

    private void Awake()
    {
        if (bciController == null)
        {
            controllerManager = GameObject.FindGameObjectWithTag("ControllerManager");
            StartCoroutine(InitCoroutine());
        }
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
        // Find the SPOToyBox object in the scene
        SPOToyBox spoToyBox = FindObjectOfType<SPOToyBox>();
        int labelNumber = transform.GetSiblingIndex();
        //get the SPO object from the training page manager
        GameObject _SPO = trainingPageManager.GetTrainingObject();
        string trainingLabel = trainingPageManager.GetTrainingLabel();
        float windowLength = trainingPageManager.GetWindowLength();
        int windowCount = trainingPageManager.GetWindowCount();

        if (string.IsNullOrEmpty(trainingLabel))
        {
            trainingLabel = "Unknown";
        }

        // Assign the SPO object ID to be the same as the page number
        // _SPO.ObjectID = labelNumber;
        spoToyBox.SetSPO(labelNumber, _SPO, trainingLabel);


        Debug.Log("Starting training on label: " + trainingLabel);

        Debug.Log("SPO is " + _SPO);

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
        trainingPageManager.UpdateNumberOfTrainingsDone(windowCount);

        int numberOfTrainingsDone = trainingPageManager.GetNumberOfTrainingsDone();

        // Anup: I turned this off to get around package issues
        if (numberOfTrainingsDone >= 5) // Needs to be updated
        {
            // UpdateTheClassifier();
            Debug.Log("BCIController: Update Classifier (Currently off)");
        }

        yield return null;
    }

        private void UpdateTrainingWindowCount(int newWindowCount)
    {
        // Get the current number of trainings done
        int numberOfTrainingsDone = trainingPageManager.GetNumberOfTrainingsDone();
        // Update the number of trainings done based on the new window count
        numberOfTrainingsDone += newWindowCount;

        //Now set them in the training page manager
        trainingPageManager.SetNumberOfTrainingsDone(numberOfTrainingsDone);

        // Update the number of trainings done text - handle this elsewhere
        //numberOfTrainingsText.text = numberOfTrainingsDone.ToString();


        // Find the BessyTrainClassifier script in the parent and call CheckTotalTrainingWindows
        // This will show the Finish Training button if the conditions are met

        //This lives on TrainingPageArea and I don't know why....
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
}
