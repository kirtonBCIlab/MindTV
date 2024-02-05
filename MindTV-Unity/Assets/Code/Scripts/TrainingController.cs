using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using BCIEssentials.Controllers;

public class TrainingController : MonoBehaviour
{
    [SerializeField] private GameObject controllerManager;
    [SerializeField] private BCIController bciController;
    [SerializeField] private GameObject startTrainingButton; // Assign in the Inspector
    [SerializeField] private GameObject cancelCountdownButton; // Assign in the Inspector
    [SerializeField] private AudioSource audioSource; // Assign in the Inspector
    [SerializeField] private AudioClip countdownBeepFile; // Assign in the Inspector
    [SerializeField] private AudioClip startBeepFile; // Assign in the Inspector
    private float countdownBeepVolume = 1f; // Example volume level for quiet beep
    private float startBeepVolume = 1f; // Example volume level for loud beep
    [SerializeField] private TMP_Text countdownText; // Assign in the Inspector
    [SerializeField] private int numberOfCountdownSeconds = 3; // Number of seconds to countdown from
    [SerializeField] private string startTrainingMessage = "Go!"; // Text to display when training starts
    [SerializeField] private TMP_Text trainTimeText; // Assign in the Inspector
    [SerializeField] private int trainingLengthSeconds = 8; // Number of seconds to do training

    private void Awake()
    {
        if (bciController == null)
        {
            controllerManager = GameObject.FindGameObjectWithTag("ControllerManager");
            Debug.Log("No BCI Controller Found. Assigning one now.");
            StartCoroutine(InitCoroutine());
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

    void PlayBeep(AudioClip clip, float volume)
    {
        audioSource.PlayOneShot(clip, volume);
    }

    private void StartTraining()
    {
        // Start the timer for the training
        StartCoroutine(TrainingTimer(trainingLengthSeconds));

        // Start the actual training
        // Needs to be updated
        // bciController.ActiveBehavior.StartTraining(BCITrainingType.Iterative);
    }

    IEnumerator TrainingTimer(int trainingSeconds)
    {
        int timeLeft = trainingSeconds;
        while (timeLeft > 0)
        {
            trainTimeText.text = timeLeft.ToString(); // Show the remaining training time
            yield return new WaitForSeconds(1);
            timeLeft--;
        }

        trainTimeText.text = "0"; // Indicate that the training is complete

        // Wait a bit before clearing the training time text and re-showing the start button
        yield return new WaitForSeconds(0.5f);
        trainTimeText.text = ""; // Clear the training time text

        startTrainingButton.SetActive(true); // Re-show the start button
    }

    private IEnumerator InitCoroutine()
    {
        yield return new WaitForEndOfFrame();
        Debug.Log("Going to assign the value now to bciController for Start Training Button");
        // bciController = controllerManager.GetComponent<BCIController>();
    }
}
