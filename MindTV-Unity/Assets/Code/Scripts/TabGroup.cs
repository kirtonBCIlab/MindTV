using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    public List<Tab> tabs;
    public Sprite tabIdle;
    public Color tabIdleColor;
    public Sprite tabHover;
    public Color tabHoverColor;
    public Sprite tabSelected;
    public Color tabSelectedColor;
    public Tab selectedTab;
    public List<GameObject> objectsToSwap;

    //Match the tab color to the page color if this is true
    public bool matchPageColor;

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
            tab.background.color = tabHoverColor;
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

        if (!matchPageColor)
        {
            tab.background.sprite = tabSelected;
            tab.background.color = tabSelectedColor;
        }
        else
        {
            //Hardcoding this right now to the first child, and getting that image
            tab.background.sprite = tabSelected;
            for(int i=0; i<objectsToSwap.Count;i++)
            {
                if(i==index)
                {
                    Color newTabcolor = objectsToSwap[i].GetComponentInChildren<Image>().color;
                    tab.background.color = newTabcolor;
                }
            }
        }

    }

    public void ResetTabs()
    {
        foreach(Tab singleTab in tabs)
        {
            if(selectedTab!=null && singleTab == selectedTab) {continue;}
            singleTab.background.sprite = tabIdle;
            singleTab.background.color = tabIdleColor;
        }
    }

    public void UpdateTabColor()
    {
        foreach(Tab singleTab in tabs)
        {
            if (singleTab == selectedTab)
            {
                singleTab.background.sprite = tabSelected;
                singleTab.background.color = tabSelectedColor;
            }
            else
            {
                singleTab.background.sprite = tabIdle;
                singleTab.background.color = tabIdleColor;
            }
        }
    }
}
