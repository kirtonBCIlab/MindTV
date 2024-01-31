using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class OpenNewTab : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        OnClickOpen();
    }
    public GameObject tabPrefab;
    public GameObject tabGroup;
    public string tabText;

    public void OnClickOpen()
    {
        GameObject newTab = Instantiate(tabPrefab, tabGroup.transform);
        newTab.GetComponentInChildren<TextMeshProUGUI>().text = tabText;
        newTab.SetActive(true);

        // Move the new tab to the position before this tab
        int currentIndex = transform.GetSiblingIndex();
        newTab.transform.SetSiblingIndex(currentIndex);

        // // Get the sibling index of the button
        // int buttonIndex = this.transform.GetSiblingIndex();

        // // If the button is not the first child
        // if (buttonIndex > 0)
        // {
        //     // Set the sibling index of the new tab to be one less than the button's
        //     newTab.transform.SetSiblingIndex(buttonIndex - 1);
        // }
        // else
        // {
        //     // If the button is the first child, just set the new tab to be the first child
        //     newTab.transform.SetSiblingIndex(0);
        // }


    }

}
