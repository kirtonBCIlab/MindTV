using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using BCIEssentials.Controllers;
using BCIEssentials;
using BCIEssentials.LSLFramework;

public class MentalCommandOnOffSwitch : MonoBehaviour
{
    public bool MentalCommandOn { get; private set; }

    [SerializeField] private Image _mentalCommandOnStatusIndicator;
    [SerializeField] private TextMeshProUGUI _mentalCommandOnStatusText;
    [SerializeField] private GameObject _bciControllerGO;
    //[SerializeField] private float _BessyCheckDelay = 0.1f; // In case we want a delay between the Bessy command and the UI update
    //[SerializeField] private LSLResponseStream _lslResponseStream;

    // Start is called before the first frame update
    private void Start()
    {

        //Handle what happens if BCIController Instance is not set to Motor Imagery
        if (BCIController.Instance == null)
        {
            Debug.LogWarning("BCIController Instance is not set to any BCI type. Please set it to Motor Imagery");
            return;
        }
        if(BCIController.Instance.ActiveBehavior.BehaviorType != BCIBehaviorType.MI)
        {
            Debug.LogWarning("BCIController Instance is not set to Motor Imagery, so this button is mislabeled");
            return;
        }

        // Turn off Mental Command by default
        MentalCommandOn = false;
    }

    public void ToggleMentalCommandOnOff()
    {
        if (MentalCommandOn)
        {
            // Mental Command is currently ON
            // Now, Turn OFF Mental Command
            Debug.Log("Turning mental commands OFF");
            ResetAllSliders();
            MentalCommandOn = false;
            // Theoretically, this should stop the running stimulus from the BCIController active behavior
            Debug.Log("Stopping the active behavior's stimulus run");
            BCIController.Instance.ActiveBehavior.StartStopStimulusRun();
        }
        else
        {
            // Mental Command is currently OFF
            // Now, Turn ON Mental Command
            Debug.Log("Turning mental commands ON");
            Debug.Log("Starting the active behavior's stimulus run");
            BCIController.Instance.ActiveBehavior.StartStopStimulusRun();
            MentalCommandOn = true;
        }

        // Send commands to Bessy and update the UI
        // UpdateMentalCommandStatusInBessy();
        // Invoke("UpdateMentalCommandStatusInUI", _BessyCheckDelay);  // If we want a delay between the Bessy command and the UI update
        UpdateMentalCommandStatusInUI();
    }

    public void ToggleMentalCommandOff()
    {
        if (MentalCommandOn)
        {
            // Mental Command is currently ON
            // Now, Turn OFF Mental Command
            Debug.Log("Turning mental commands OFF");
            ResetAllSliders();
            MentalCommandOn = false;
            // Theoretically, this should stop the running stimulus from the BCIController active behavior
            Debug.Log("Stopping the active behavior's stimulus run");
            BCIController.Instance.ActiveBehavior.StartStopStimulusRun();
        }
        else
        {
            // Mental Command is currently OFF
            // Now, Turn ON Mental Command
            return;
        }

        // Send commands to Bessy and update the UI
        // UpdateMentalCommandStatusInBessy();
        // Invoke("UpdateMentalCommandStatusInUI", _BessyCheckDelay);  // If we want a delay between the Bessy command and the UI update
        UpdateMentalCommandStatusInUI();
    }

    private void UpdateMentalCommandStatusInBessy()
    {
        // Update the Mental Command status in Bessy
        if (MentalCommandOn)
        {
            // Turn on Mental Command
            Debug.Log("Tell Bessy to turn Mental Commands ON (NOT IMPLEMENTED)");
            // ToDo: Bessy Command to turn ON listening for Mental Commands
        }
        else
        {
            // Turn off Mental Command
            Debug.Log("Tell Bessy to turn Mental Commands OFF (NOT IMPLEMENTED)");
            // ToDo: Bessy Command to turn OFF listening for Mental Commands
        }
    }

    private void UpdateMentalCommandStatusInUI()
    {
        Debug.Log("Updating the Mental Command status in UI");
        if (MentalCommandOn)
        {
            _mentalCommandOnStatusIndicator.color = Color.green; // Unity's predefined green color
            _mentalCommandOnStatusText.text = "Mental Commands On";
        }
        else
        {
            _mentalCommandOnStatusIndicator.color = Color.red; // Unity's predefined red color
            _mentalCommandOnStatusText.text = "Mental Commands Off";
        }
    }

    private void ResetAllSliders()
    {
        // Find all Slider objects in the scene
        Slider[] sliders = FindObjectsOfType<Slider>();

        // Loop through each slider and set its value to zero
        foreach (Slider slider in sliders)
        {
            slider.value = 0f;
        }
    }
}
