using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using BCIEssentials.Controllers;

public class StartTraining : MonoBehaviour
{
    [SerializeField] private GameObject controllerManager;
    [SerializeField] GameObject displayStartTrainingButton;
    [SerializeField] private BCIController bciController;
    [SerializeField] private TMP_Text countdownText; // Assign in the Inspector
    [SerializeField] private AudioSource audioSource; // Assign in the Inspector
    [SerializeField] private AudioClip countdownBeepFile; // Assign in the Inspector
    [SerializeField] private AudioClip startBeepFile; // Assign in the Inspector
    private float countdownBeepVolume = 1f; // Example volume level for quiet beep
    private float startBeepVolume = 1f; // Example volume level for loud beep

    private void Awake()
    {
        if (bciController == null)
        {
            controllerManager = GameObject.FindGameObjectWithTag("ControllerManager");
            Debug.Log("No BCI Controller Found. Assigning one now.");
            StartCoroutine(InitCoroutine());
        }
    }

    public void StartTrainingCountdown()
    {
        displayStartTrainingButton.SetActive(false);  // Hide the start training button
        StartCoroutine(Countdown(3));
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

        countdownText.text = "Go!";
        PlayBeep(startBeepFile, startBeepVolume);
        bciController.ActiveBehavior.StartTraining(BCITrainingType.Iterative); // Start the actual training

        // Wait a bit before removing the countdown text
        yield return new WaitForSeconds(1);
        countdownText.text = ""; // Clear the countdown text

        // Wait a bit before re-showing the start button - TEMPORARY
        yield return new WaitForSeconds(1);

        displayStartTrainingButton.SetActive(true); // Re-show the start button
    }

    void PlayBeep(AudioClip clip, float volume)
    {
        audioSource.PlayOneShot(clip, volume);
    }

    private IEnumerator InitCoroutine()
    {
        yield return new WaitForEndOfFrame();
        Debug.Log("Going to assign the value now to bciController for Start Training Button");
        bciController = controllerManager.GetComponent<BCIController>();
    }
}
