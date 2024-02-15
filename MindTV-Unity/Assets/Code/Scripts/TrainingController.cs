using TMPro;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using BCIEssentials.Controllers;
using BCIEssentials.StimulusObjects;

public class TrainingController : MonoBehaviour
{
    [SerializeField] private SPO trainingObjectSPO; // Assign in the Inspector
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
    [SerializeField] private TMP_Text numberOfTrainingsText; // UI element to indicate number of trainings done
    private float windowLength = 2.0f; // Window length in seconds for training. Set to 2 seconds. THIS IS WHERE YOU CHANGE WINDOW LENGTH IF NEEDED
    int windowCount = 3; // Number of windows per training
    private float trainingLengthSeconds; // Total length of the training in seconds
    // [SerializeField] TMP_Dropdown windowLengthDropdown; // Assign in the Inspector
    // [SerializeField] TMP_Dropdown windowCountDropdown; // Assign in the Inspector
    [SerializeField] TMP_Dropdown trainingTrialLengthDropdown; // Assign in the Inspector
    private string trainingLabel;
    private int numberOfTrainingsDone = 0; // Number of trainings done
    private float uiUpdateDelay = 0.5f; // Delay for updating UI elements

    private void Awake()
    {
        // if (bciController == null)
        // {
        //     bciController = GameObject.FindGameObjectWithTag("BCIController");
        //     // //Get the proper behaviour
        //     // bciController.GetComponent<BCIController>().ChangeBehavior(MI);

        //     // FOR BRIAN: Number of trainings done set to 0
        //     numberOfTrainingsText.text = numberOfTrainingsDone.ToString();
        // }

        // BCIController.ChangeBehavior(BCIBehaviorType.MI);
        ChangeTrainingTrialLength();  // Initialize the Training Trial Length to the default (first value in dropdown) – also sets animation duration
    }

    // private void Start()
    // {
    //     // Initialize the uiTweener reference
    //     uiTweener = trainingObjectSPO.GetComponent<UITweener>();
    //     Debug.Log("UITweener: " + uiTweener);
    //     if (uiTweener == null)
    //     {
    //         Debug.LogError("UITweener component not found on the trainingObjectSPO");
    //     }
    // }

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

        // StartCoroutine(TrainingTimer(trainingLengthSeconds));
        StartCoroutine(StartTraining()); // Start the actual training

        // // Wait a bit before removing the countdown text
        yield return new WaitForSeconds(1);  // Wait 1 second before removing the Countdown text (Length matches the startBeepFile length)
        countdownText.text = ""; // Clear the countdown text
    }

    private void PlayBeep(AudioClip clip, float volume)
    {
        audioSource.PlayOneShot(clip, volume);
    }

    IEnumerator StartTraining()
    {
        trainingLabel = trainingLabelInputField.text;
        if (string.IsNullOrEmpty(trainingLabel))
        {
            trainingLabel = "Unknown";
        }

        Debug.Log("Starting training on label: " + trainingLabel);

        Debug.Log("SPO is " + trainingObjectSPO);

        float trainingLengthSeconds = windowCount * windowLength;
        // int trainingLengthSecondsInt = (int)trainingLengthSeconds;

        // Update the length of the animation duration
        Debug.Log("trainingObjectSPO.UITweener: Setting animation duration to " + trainingLengthSeconds);
        // uiTweener.duration = trainingLengthSeconds;


        // Do the actual training
        // Anup: I turned this off to get around package issues
        Debug.Log("BCIController: Do Single Training (Currently off)");
        // BCIController.WhileDoSingleTraining(trainingObjectSPO, windowLength, windowCount);

        // Start the animation
        UITweener uiTweener = trainingObjectSPO.GetComponent<UITweener>();
        if (uiTweener != null)
        {
            uiTweener.HandleTween(); // Call the tween handling method for animation
            Debug.Log("Calling trainingObjectSPO.UITweener.HandleTween() to start animation");
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
        UpdateTrainingWindowCount(windowCount);

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
        numberOfTrainingsDone += newWindowCount;
        numberOfTrainingsText.text = numberOfTrainingsDone.ToString();

        // Find the BessyTrainClassifier script in the parent and call CheckTotalTrainingWindows
        // This will show the Finish Training button if the conditions are met
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

    private IEnumerator InitCoroutine()
    {
        yield return new WaitForEndOfFrame();
        Debug.Log("Going to assign the value now to bciController for Start Training Button");
        // bciController = bciController.GetComponent<BCIController>(); // Needs to be updated
    }

    public void ChangeTrainingTrialLength()
    {
        // Get the string label of the TMP dropdown and convert it to a float
        float targetTrialLengthSeconds = 0.0f;
        string targetTrialLengthString = trainingTrialLengthDropdown.options[trainingTrialLengthDropdown.value].text;

        // Use regex to find numbers followed by " s" in the string
        Match match = Regex.Match(targetTrialLengthString, @"(\d+)\s*s");
        if (match.Success)
        {
            // Convert the matched value to float
            targetTrialLengthSeconds = float.Parse(match.Groups[1].Value);

            // Use targetTrialLength as needed
            Debug.Log("Extracted float value for Training Trial Length: " + targetTrialLengthSeconds);
        }
        else
        {
            Debug.Log("No matching numbers found in the string.");
        }

        // Calculate windowCount by dividing trainingLengthSeconds by windowLength, rounding the result, and converting to int
        windowCount = Mathf.RoundToInt(targetTrialLengthSeconds / windowLength);

        Debug.Log("windowCount is: " + windowCount + " for targetTrialLengthSeconds: " + targetTrialLengthSeconds + " using windowLength: " + windowLength);

        // Update the length of the animation duration
        // Access the UITweener script attached to the TrainingObjectSPO
        UITweener uiTweener = trainingObjectSPO.GetComponent<UITweener>();
        if (uiTweener != null)
        {
            uiTweener.duration = targetTrialLengthSeconds; // Update duration
            Debug.Log("trainingObjectSPO.UITweener: Setting animation duration to " + targetTrialLengthSeconds);
        }
    }

    // // Oudated code for changing window length and count – kept for reference.
    // // Update with regex approach for retrieving values in dropdown strings in ChangeTrainingTrialLength()
    // // Also requires uncommenting the TMP_Dropdown fields for WindowLength and WindowCount above and assigning them in the Inspector
    // public void ChangeWindowLength()
    // {
    //     // Get the string label of the TMP dropdown and convert it to a float
    //     string newWindowLength = windowLengthDropdown.options[windowLengthDropdown.value].text;
    //     Debug.Log("newWindowLength: " + newWindowLength);
    //     char windowLengthChar = newWindowLength[0];
    //     Debug.Log("windowLengthChar: " + windowLengthChar);

    //     windowLength = (float)windowLengthChar - 48.0f;
    // }

    // public void ChangeWindowCount()
    // {
    //     // Get the string label of the TMP dropdown and convert it to an int
    //     string newWindowCount = windowCountDropdown.options[windowCountDropdown.value].text;
    //     Debug.Log("newWindowCount: " + newWindowCount);
    //     char windowCountChar = newWindowCount[0];
    //     Debug.Log("windowCountChar: " + windowCountChar);

    //     windowCount = (int)windowCountChar - 48;
    // }
}
