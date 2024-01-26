using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

[RequireComponent(typeof(Image))]
public class CloseTab : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        OnClickClose(this.GetComponentInParent<Tab>());
    }
    public void OnClickClose(Tab tab)
    {
        tab.gameObject.SetActive(false);
    }

}
