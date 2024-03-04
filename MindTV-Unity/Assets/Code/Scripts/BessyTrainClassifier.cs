using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using BCIEssentials.Controllers;

public class BessyTrainClassifier : MonoBehaviour
{
    // [SerializeField] private List<TMP_Text> numberTimesLabelsTrainedCount = new List<TMP_Text>(4); // Assign in Inspector
    // [SerializeField] private TMP_Text numberTimesTrainedCountLabel1;
    // [SerializeField] private TMP_Text numberTimesTrainedCountLabel2;
    [SerializeField] private GameObject finishTrainingButton;  // Assign in Inspector
    [SerializeField] private TrainingController[] trainingControllers; // Reference to all TrainingController components in children

    [SerializeField] private int minWindowsPerClass = 5;
    [SerializeField] private int minClasses = 2;
    // [SerializeField] private GameObject _bciControllerGO;


    private void Awake() {
        finishTrainingButton.SetActive(false);

        // trainingControllers = GetComponentsInChildren<TrainingController>();

        // Add CheckTotalTrainingWindows as a listener to the event on each TrainingController
        foreach (var controller in trainingControllers)
        {
            Debug.Log("Adding listener to " + controller.gameObject.name);
            controller.onTrainingNumberUpdated.AddListener(CheckTotalTrainingWindows);
        }
    }

    public void CheckTotalTrainingWindows()
    {
        // Check if a sufficient number of windows have been trained
        // Based on the set amount of Windows per class and the minimum number of classes
        // If it is, then enable the Finish Training button
        // If it is not, then disable the Finish Training button

        int numSufficientClasses = 0; // Track the number of classes meeting the minimum window requirement

        // Loop through each TrainingController, retrieve the trainNumberCount value, and check if it meets the minimum window requirement
        foreach (var controller in trainingControllers)
        {
            if (int.TryParse(controller.trainNumberCount.text, out int windowCount) && windowCount >= minWindowsPerClass)
            {
                numSufficientClasses++; // Increment counter if this class has sufficient windows
            }
        }

        // Check if the number of classes with sufficient windows is >= minClasses
        if (numSufficientClasses >= minClasses)
        {
            finishTrainingButton.SetActive(true); // Enable the Finish Training button
            Debug.Log("Enabling Finish Training button");
        }
        else
        {
            finishTrainingButton.SetActive(false); // Disable the Finish Training button
            Debug.Log("Not enough classes with sufficient training windows. Finish Training button remains disabled.");
        }
    }

    public void FinishTraining()
    {
        UpdateClassifier();

        // Could also add Scene Controller code here to move to the next scene
    }

    private void UpdateClassifier()
    {
        // Send a training complete marker and tell Bessy Python to update the classifier (do ML model training)
        Debug.Log("Telling Bessy to Update the classifier");
        BCIController.UpdateClassifier();
    }
}
