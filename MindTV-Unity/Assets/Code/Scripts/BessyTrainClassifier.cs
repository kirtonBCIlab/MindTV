using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using BCIEssentials.Controllers;

public class BessyTrainClassifier : MonoBehaviour
{
    [SerializeField] private List<TMP_Text> numberTimesLabelsTrainedCount = new List<TMP_Text>(4); // Assign in Inspector
    [SerializeField] private GameObject finishTrainingButton;  // Assign in Inspector
    // [SerializeField] private TMP_Text numberTimesTrainedCountLabel1;
    // [SerializeField] private TMP_Text numberTimesTrainedCountLabel2;

    [SerializeField] private int minWindowsPerClass = 5;
    [SerializeField] private int minClasses = 2;

    private void Start()
    {
        finishTrainingButton.SetActive(false);
    }


    public void CheckTotalTrainingWindows()
    {
        // Check if a sufficient number of windows have been trained
        // Based on the set amount of Windows per class and the minimum number of classes
        // If it is, then enable the Finish Training button
        // If it is not, then disable the Finish Training button
        int numSufficientClasses = 0; // Track the number of classes meeting the minimum window requirement

        // Loop through each label, parse the text to an int, and check if it meets the minimum window requirement
        foreach (TMP_Text label in numberTimesLabelsTrainedCount)
        {
            Debug.Log("Checking label: " + label.text);
            Debug.Log("Label value: " + label.text);
            if (int.TryParse(label.text, out int windowCount) && windowCount >= minWindowsPerClass)
            {
                numSufficientClasses++; // Increment counter if this class has sufficient windows
            }
        }

        // Check if the number of classes with sufficient windows is >= minClasses
        if (numSufficientClasses >= minClasses)
        {
            // Enable the Finish Training button
            Debug.Log("Enabling Finish Training button");
            finishTrainingButton.SetActive(true);
            // Code to enable the Finish Training button goes here
        }
        else
        {
            // Disable the Finish Training button
            Debug.Log("Not enough classes with sufficient training windows. Finish Training button remains disabled.");
            // Code to disable the Finish Training button goes here
        }
    }

}
