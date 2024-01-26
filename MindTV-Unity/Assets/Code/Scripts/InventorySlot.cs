using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] Canvas canvas;

    private StimulusManager stimulusManager;
    private TrainingMenuController trainingMenuController; 

    private TrainingItem trainingItem; // Contains information about the sprite inside the selected slot
    private Button slotButton; // The button component of the slot

    // Start is called before the first frame update
    void Start()
    {
        canvas = transform.root.GetComponent<Canvas>();
        stimulusManager = canvas.GetComponent<StimulusManager>();
        trainingMenuController = canvas.GetComponent<TrainingMenuController>();

        slotButton = gameObject.GetComponent<Button>();
        trainingItem = transform.Find("TrainingItem").GetComponent<TrainingItem>();

        // When the inventory slot is clicked the sprite of the training object is assigned to the selected sprite and the slot is highlighted
        slotButton.onClick.AddListener(() => stimulusManager.SetTrainingObject(trainingItem.sprite));
        slotButton.onClick.AddListener(() => trainingMenuController.HighlightSelectedSprite(gameObject));
    }

}
