using BCIEssentials.ControllerBehaviors;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TrainingMenuController : MonoBehaviour
{
    //TO DO: add the workflow for the reject training

    [SerializeField] Transform neutral;
    [SerializeField] Transform push;
    [SerializeField] Transform pull;
    [SerializeField] Transform lift;
    [SerializeField] Transform drop;

    [SerializeField] Transform actionText;
    [SerializeField] Transform timer;
    [SerializeField] GameObject backButton;
    [SerializeField] GameObject MIController;
    [SerializeField] GameObject inventoryFrame;

    public string currentAction = "Neutral";
    public float currentTrainingSessionTime;

    public Dictionary<string, Transform> commands;
    private GameObject[] inventory;

    // Start is called before the first frame update
    void Start()
    {
        commands =
        new Dictionary<string, Transform>(){
            {"Neutral", neutral},
            {"Push", push},
            {"Pull", pull},
            {"Lift", lift},
            {"Drop", drop}
        };
    }

    //returns MainMenu scene
    public void goHome()
    {
        SceneManager.LoadScene("MainMenu");
    }

    //returns LiveMode scene
    public void goLive()
    {
        SceneManager.LoadScene("LiveMode");
    }

    //set current training action according to the user selection in TraningOptionList page
    public void SetCurrentAction(TMP_Text action)
    {
        currentAction = action.text;
        actionText.GetComponent<TMP_Text>().SetText(string.Concat("Action: ", action.text));
    }

    // Send the time to the timer
    public void SendTime(TMP_InputField timeInput)
    {
        if (float.TryParse(timeInput.text, out float result))
        {
            timer.GetComponent<Timer>().SetTime(result);
            currentTrainingSessionTime = result;
        }
    }

    // Increase the count for the number of sessions trained for the action
    public void AddSessionTrained()
    {
        Transform command = commands[currentAction];
        TMP_Text sessionCount = command.Find("TrainedSessions").GetComponent<TMP_Text>();
        Button deleteButton = command.Find("DeleteButton").GetComponent<Button>();

        int count = 0;

        if (int.TryParse(sessionCount.text, out count))
        {
            count = count + 1;
            sessionCount.SetText(count.ToString());
            deleteButton.interactable = true;
        }
    }

    // Clear the trained sessions for the action
    public void ClearTrainedSessions(TMP_Text sessionCount)
    {
        sessionCount.SetText("0");
    }

    public void SetReturnTrainingMenu(bool visible)
    {
        backButton.SetActive(visible);
    }

    //input boundary checking for training session duration, acceptable range: 6-9
    public void TrainingTimeChangeCheck(TMP_InputField timeInput)
    {
        if (int.TryParse(timeInput.text.ToString(), out int result))
        {
            if (result < 6 || result > 10)
            {
                timeInput.text = "8";
            }
        }
    }

    //set number of training window frames to be training session duration - countdown time
    public void UpdateControllerFrame()
    {
        MIControllerBehavior behavior = MIController.GetComponent<MIControllerBehavior>();
        if (behavior != null)
        {
            behavior.numTrainWindows = (int)(currentTrainingSessionTime - 3);
        }
    }

    //when user clicks stop, training and marker stream are interruptes
    public void InterruptTraining()
    {
        MIControllerBehavior behavior = MIController.GetComponent<MIControllerBehavior>();
        behavior.StopTraining();
        behavior.StopStimulusRun();
    }

    //highlights the selected/currentlt used training object in the inventory
    public void HighlightSelectedSprite(GameObject inventorySlot)
    {
        inventory = GameObject.FindGameObjectsWithTag("InventorySlot");

        foreach (GameObject slot in inventory)
        {
            GameObject frame = slot.transform.Find("Frame").gameObject;
            frame.SetActive(false);
        }

        GameObject selectedFrame = inventorySlot.transform.Find("Frame").gameObject;
        selectedFrame.SetActive(true);
    }

    //shows/hides the inventory as user clicks inventory icon
    public void ToggleInventoryVisibility()
    {
        inventoryFrame.SetActive(!inventoryFrame.activeSelf);
    }
}
