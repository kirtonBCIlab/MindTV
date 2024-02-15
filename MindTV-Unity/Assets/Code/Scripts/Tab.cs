using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using TMPro;

[RequireComponent(typeof(Image))]
public class Tab : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler,IPointerExitHandler
{
    public TabGroup tabGroup;

    public Image background;
    
    public UnityEvent onTabSelected;
    public UnityEvent onTabDeselected;


    void Start()
    {
        background = GetComponent<Image>();
        tabGroup.Subscribe(this);
    }

    public void SetLabel(string label)
    {
        GetComponentInChildren<TextMeshProUGUI>().text = label;
    }

    public void SetColor(Color color)
    {
        GetComponentInChildren<Image>().color = color;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        tabGroup.OnTabSelected(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tabGroup.OnTabEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tabGroup.OnTabExit(this);
    }

    public void Select()
    {
        onTabSelected?.Invoke();
    }

    public void Deselect()
    {
        onTabDeselected?.Invoke();
    }

}
