using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    public List<Tab> tabs;
    public Sprite tabIdle;
    public Sprite tabHover;
    public Sprite tabSelected;
    public Tab selectedTab;

    public List<GameObject> objectsToSwap;

    public void Subscribe(Tab tab)
    {
        if(tabs == null)
        {
            tabs = new List<Tab>();
        }

        tabs.Add(tab);
    }

    public void OnTabEnter(Tab tab)
    {
        ResetTabs();
        if (selectedTab == null || tab !=selectedTab)
        {
            tab.background.sprite = tabHover;
        }
    }

    public void OnTabExit(Tab tab)
    {
        ResetTabs();
    }

    public void OnTabSelected(Tab tab)
    {

        if(selectedTab !=null)
        {
            selectedTab.Deselect();
        }

        selectedTab = tab;

        selectedTab.Select();

        ResetTabs();
        tab.background.sprite = tabSelected;
        int index = tab.transform.GetSiblingIndex();
        for(int i=0; i<objectsToSwap.Count;i++)
        {
            if(i==index)
            {
                objectsToSwap[i].SetActive(true);
            }
            else
            {
                objectsToSwap[i].SetActive(false);
            }
        }
    }

    public void ResetTabs()
    {
        foreach(Tab singleTab in tabs)
        {
            if(selectedTab!=null && singleTab == selectedTab) {continue;}
            singleTab.background.sprite = tabIdle;
        }
    }

}
