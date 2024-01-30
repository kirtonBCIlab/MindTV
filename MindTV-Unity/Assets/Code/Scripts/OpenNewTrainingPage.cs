using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OpenNewTrainingPage : MonoBehaviour,IPointerClickHandler,ISaveable
{
    public void OnPointerClick(PointerEventData eventData)
    {
        OnClickOpenPage();
    }
    public GameObject pagePrefab;
    public GameObject pageGroup;

    public GameObject tabGroup;

    public void OnClickOpenPage()
    {
        GameObject newPage = Instantiate(pagePrefab, pageGroup.transform);
        
        //Set the propoerties of the page here I think.

        newPage.SetActive(true);

        // Move the new tab to the position before this tab
        int currentIndex = transform.GetSiblingIndex();
        newPage.transform.SetSiblingIndex(currentIndex);

    }

    public void PopulateSaveData(SaveData saveData)
    {
        throw new System.NotImplementedException();
    }

    public void LoadFromSaveData(SaveData saveData)
    {
        throw new System.NotImplementedException();
    }

    
}
