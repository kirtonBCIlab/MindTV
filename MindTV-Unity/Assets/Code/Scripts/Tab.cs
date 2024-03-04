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
    public int number;
    
    void Start()
    {
        number = transform.GetSiblingIndex();
        background = GetComponent<Image>();

        tabGroup.Subscribe(this);
    }


    public void SetLabel(string label)
    {
        GetComponentInChildren<TextMeshProUGUI>().text = label;
    }

    public void SetColor(Color color)
    {
        background.color = color;
    }

    public void SetColorAlpha(float alpha)
    {
        Color color = background.color;
        color.a = alpha;
        background.color = color;
    }

    public void SetSprite(Sprite sprite)
    {
        background.sprite = sprite;
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
}
