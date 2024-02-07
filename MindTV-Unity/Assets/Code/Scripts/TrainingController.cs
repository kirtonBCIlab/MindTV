using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using BCIEssentials.Controllers;

public class TrainingController : MonoBehaviour
{
    [SerializeField] private GameObject controllerManager;
    [SerializeField] private BCIController bciController; // Assign in the Inspector -- Needs to be updated?
    [SerializeField] private GameObject trainingObjectSPO; // Assign in the Inspector
    [SerializeField] TMP_InputField trainingLabelInputField; // Assign in the Inspector
    [SerializeField] private GameObject startTrainingButton; // Assign in the Inspector
    [SerializeField] private GameObject cancelCountdownButton; // Assign in the Inspector
    [SerializeField] private AudioSource audioSource; // Assign in the Inspector
    [SerializeField] private AudioClip countdownBeepFile; // Assign in the Inspector
    [SerializeField] private AudioClip startBeepFile; // Assign in the Inspector
    private float countdownBeepVolume = 1f; // Example volume level for quiet beep
    private float startBeepVolume = 1f; // Example volume level for loud beep
    [SerializeField] private TMP_Text countdownText; // UI element to show number of seconds remaining in countdown before training starts
    [SerializeField] private int numberOfCountdownSeconds = 3; // Number of seconds to countdown from
    [SerializeField] private string startTrainingMessage = "Go!"; // Text to display when training starts
    [SerializeField] private TMP_Text trainRemainingTimeText; // UI element to indicate the number of seconds remaining in training
    [SerializeField] private int trainingLengthSeconds = 8; // Number of seconds to do training
    [SerializeField] private TMP_Text numberOfTrainingsText; // UI element to indicate number of trainings done
    private string trainingLabel;
    private int numberOfTrainingsDone = 0; // Number of trainings done

    private void Awake()
    {
        if (bciController == null)
        {
            controllerManager = GameObject.FindGameObjectWithTag("ControllerManager");
            Debug.Log("No BCI Controller Found. Assigning one now.");
            StartCoroutine(InitCoroutine());

            // FOR BRIAN: Number of trainings done set to 0
            numberOfTrainingsText.text = numberOfTrainingsDone.ToString();
        }
    }

    public void StartCountdown()
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
        yield return new WaitForSeconds(0.5f); // Wait a bit before starting the countdown

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

        // StartCoroutine(TrainingTimer(trainingLengthSeconds));
        StartTraining(); // Start the actual training

        // // Wait a bit before removing the countdown text
        yield return new WaitForSeconds(1);
        countdownText.text = ""; // Clear the countdown text
    }

    private void PlayBeep(AudioClip clip, float volume)
    {
        audioSource.PlayOneShot(clip, volume);
    }

    private void StartTraining()
    {
        trainingLabel = trainingLabelInputField.text;
        if (string.IsNullOrEmpty(trainingLabel))
        {
            trainingLabel = "unassigned";
        }
        Debug.Log("Starting training on label: " + trainingLabel);

        Debug.Log("SPO is " + trainingObjectSPO);

        // Start the timer for the training
        StartCoroutine(TrainingTimer(trainingLengthSeconds));

        // Start the actual training
        // Needs to be updated
        // bciController.ActiveBehavior.SetLabel(userInput); // Needs to be updated
        // bciController.ActiveBehavior.StartTraining(BCITrainingType.Iterative); // Needs to be updated

        // FOR BRIAN: TEMPORARY CODE TO SHOW NUMBER OF TRAININGS COUNTER IS UPDATED
        numberOfTrainingsDone++;
        numberOfTrainingsText.text = numberOfTrainingsDone.ToString();
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
        yield return new WaitForSeconds(0.5f);
        trainRemainingTimeText.text = ""; // Clear the training time text

        startTrainingButton.SetActive(true); // Re-show the start button
    }

    private IEnumerator InitCoroutine()
    {
        yield return new WaitForEndOfFrame();
        Debug.Log("Going to assign the value now to bciController for Start Training Button");
        // bciController = controllerManager.GetComponent<BCIController>(); // Needs to be updated
    }
}
