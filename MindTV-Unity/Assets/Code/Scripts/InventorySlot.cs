using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TrainingPageManager trainingPageManager;

    [SerializeField] private CursorManager cursor;

    [SerializeField] private TrainingItem trainingItem; // Contains information about the sprite inside the selected slot

    void Start()
    {
        // TODO - remove coupling to TrainingPageManager, ex: emit an image changed event
        // or have manager attach listeners to InventorySlots.  This way InventorySlot 
        // doesn't have to care about were image goes.
        GameObject myTrainPanel = GameObject.FindGameObjectWithTag("TrainingPanel");
        trainingPageManager = myTrainPanel.GetComponent<TrainingPageManager>();
        trainingItem = this.GetComponentInChildren<TrainingItem>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        trainingPageManager.ImageChanged(trainingItem.sprite);
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
