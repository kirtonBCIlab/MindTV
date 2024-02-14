using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BCIEssentials.Controllers;
using BCIEssentials.LSLFramework;

public class MentalCommandOnOffSwitch : MonoBehaviour
{
    public bool MentalCommandOn { get; private set; }

    [SerializeField] private Image _mentalCommandOnStatusIndicator;
    [SerializeField] private TextMeshProUGUI _mentalCommandOnStatusText;
    [SerializeField] private GameObject _bciControllerGO;
    [SerializeField] private float _BessyCheckDelay = 0.1f; // In case we want a delay between the Bessy command and the UI update

    // Start is called before the first frame update
    private void Awake()
    {
        // Get the BCIController GameObject and the LSLResponseStream component
        Debug.Log("BCI controller and lsl response stream are not implemented yet");
        // _bciControllerGO = GameObject.FindWithTag("BCIController");
        // _lslResponseStream = _bciControllerGO.GetComponent<LSLResponseStream>();

        // Turn off Mental Command by default
        // Because our functions toggle between the states, we will explicitly set the state to ON (true) and then toggle it off
        MentalCommandOn = true;
        ToggleMentalCommandOnOff();
    }

    public void ToggleMentalCommandOnOff()
    {
        if (MentalCommandOn)
        {
            // Mental Command is currently ON
            // Now, Turn OFF Mental Command
            Debug.Log("Turning mental commands OFF");
            MentalCommandOn = false;
        }
        else
        {
            // Mental Command is currently OFF
            // Now, Turn ON Mental Command
            Debug.Log("Turning mental commands ON");
            MentalCommandOn = true;
        }

        // Send commands to Bessy and update the UI
        UpdateMentalCommandStatusInBessy();
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

    // private void TurnOnMentalCommands()
    // {
    //     // Turn on Mental Command
    //     Debug.Log("Mental Commands On (NOT IMPLEMENTED)");
    //     // ToDo: Bessy Command to turn ON listening for Mental Commands

    //     // MAYBE: Logic to check that mental commands have been turned off, if Bessy does provide some sort of feedback
    //     // Update status
    //     MentalCommandOn = true;
    //     UpdateMentalCommandStatus();

    //     // Note: We could combine this with TurnOffMentalCommands() and pass in a boolean to determine the state if 
    // }

    // private void TurnOffMentalCommands()
    // {
    //     // Turn off Mental Command
    //     Debug.Log("Mental Commands OFF (NOT IMPLEMENTED)");
    //     // ToDo: Bessy Command to turn OFF listening for Mental Commands

    //     // MAYBE: Logic to check that mental commands have been turned off, if Bessy does provide some sort of feedback
    //     // Update status
    //     MentalCommandOn = false;
    //     UpdateMentalCommandStatus();
    // }

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
}
