using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    public List<Tab> tabs;
    public Sprite tabIdle;
    //public Color tabIdleColor;
    public float tabIdleAlpha;
    public Sprite tabHover;
    //public Color tabHoverColor;
    public float tabHoverAlpha;
    public Sprite tabSelected;
    //public Color tabSelectedColor;
    public float tabSelectedAlpha;
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
            Color tempColor = tab.background.color;
            tempColor.a = tabHoverAlpha;
            tab.background.color = tempColor;
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
            Color tempColor = tab.background.color;
            tempColor.a = tabSelectedAlpha;
            tab.background.color = tempColor;
            //tab.background.color = tabSelectedColor;
        }
        else
        {
            
            tab.background.sprite = tabSelected;
            MatchPageColor();
        }

    }

    public void MatchPageColor()
    {
        if(selectedTab!=null)
        {
            for(int i=0; i<objectsToSwap.Count;i++)
            {
                if(i==selectedTab.transform.GetSiblingIndex())
                {
                    //Hardcoding this right now to the first child, and getting that image
                    Color newTabcolor = objectsToSwap[i].GetComponentInChildren<Image>().color;
                    selectedTab.background.color = newTabcolor;
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
            Color tempColor = singleTab.background.color;
            tempColor.a = tabIdleAlpha;
            singleTab.background.color = tempColor;
            //singleTab.background.color = tabIdleColor;
        }
    }

    public void UpdateTabColor()
    {
        foreach(Tab singleTab in tabs)
        {
            if (singleTab == selectedTab)
            {
                singleTab.background.sprite = tabSelected;
                //singleTab.background.color = tabSelectedColor;
                Color tempColor = singleTab.background.color;
                tempColor.a = tabSelectedAlpha;
                singleTab.background.color = tempColor;
            }
            else
            {
                singleTab.background.sprite = tabIdle;
                Color tempColor = singleTab.background.color;
                tempColor.a = tabIdleAlpha;
                singleTab.background.color = tempColor;
                //singleTab.background.color = tabIdleColor;
            }
        }
    }
}
