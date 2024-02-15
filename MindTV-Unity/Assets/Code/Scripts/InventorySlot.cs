using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{

    [SerializeField] GameObject myTrainPanel;

    [SerializeField] private TrainingPageManager stimulusManager;
    [SerializeField] private TrainingMenuController trainingMenuController;
    [SerializeField] private CursorManager cursor;

    [SerializeField] private TrainingItem trainingItem; // Contains information about the sprite inside the selected slot
    // Start is called before the first frame update
    
    //I kind of hate how this is functioning.
    void Start()
    {
        // canvas = transform.root.GetComponent<Canvas>();
        myTrainPanel = GameObject.FindGameObjectWithTag("TrainingPanel");
        stimulusManager = myTrainPanel.GetComponent<TrainingPageManager>();
        trainingMenuController = myTrainPanel.GetComponent<TrainingMenuController>();
        trainingItem = this.GetComponentInChildren<TrainingItem>();

        // When the inventory slot is clicked the sprite of the training object is assigned to the selected sprite and the slot is highlighted
        // slotButton.onClick.AddListener(() => stimulusManager.SetTrainingObject(trainingItem.sprite));
        // slotButton.onClick.AddListener(() => trainingMenuController.HighlightSelectedSprite(gameObject));
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        stimulusManager.SetTrainingObject(trainingItem.sprite);
        trainingMenuController.HighlightSelectedSprite(gameObject);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        cursor.OnCursorEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        cursor.OnCursorExit();
    }

}
