using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    public List<Tab> tabs;
    public List<GameObject> objectsToSwap;

    public Tab selectedTab;

    public Sprite tabIdle;
    public float tabIdleAlpha;

    public Sprite tabHover;
    public float tabHoverAlpha;

    public Sprite tabSelected;
    public float tabSelectedAlpha;


    public void Subscribe(Tab tab)
    {
        if(tabs == null)
        {
            tabs = new List<Tab>();
        }
        tabs.Add(tab);
    }

    public void SetTabAppearance(int number, string label, Color color)
    {
        if (tabs.Count > number)
        {
            // The tabs list may not match the order shown in the hierarchy
            // Find the correct tab by searching for tab number insead.
            Tab tab = tabs.Find(tab => tab.number == number);

            tab.SetLabel(label);
            tab.SetColor(color);
        }
        ResetTabs();
    }

    public void OnTabEnter(Tab tab)
    {
        ResetTabs();
        if (selectedTab == null || tab != selectedTab)
        {
            tab.SetSprite(tabHover);
            tab.SetColorAlpha(tabHoverAlpha);
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

        tab.SetSprite(tabSelected);
        tab.SetColorAlpha(tabSelectedAlpha);
    }

    public void ResetTabs()
    {
        foreach(Tab tab in tabs)
        {
            if(selectedTab!=null && tab == selectedTab) {continue;}
            tab.SetSprite(tabIdle);
            tab.SetColorAlpha(tabIdleAlpha);
        }
    }
}
