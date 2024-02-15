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
    [SerializeField] GameObject trainingOptionsFrame;
    [SerializeField] GameObject displayStartTrainingButton;
    [SerializeField] GameObject displayNumberOfTimesTrained;
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
        trainingOptionsFrame.SetActive(!trainingOptionsFrame.activeSelf);
        displayStartTrainingButton.SetActive(!displayStartTrainingButton.activeSelf);
        displayNumberOfTimesTrained.SetActive(!displayNumberOfTimesTrained.activeSelf);
    }
}
