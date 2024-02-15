using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// TrainingTabManager is an egregious hack to update the training tabs to current settings
/// 
/// This is necessary because TrainingPage and Tab objects are not in the same hierarchy, and
/// creation order of the objects is hard to manage reliably.  There's probably a better way, but
/// there has been much suffering trying to make this work properly.
/// 
/// </summary>
public class TrainingTabManager : MonoBehaviour
{
    void Start()
    {
        tabGroup = GetComponent<TabGroup>() ?? new TabGroup();
        user = SettingsManager.Instance?.currentUser ?? new Settings.User();
    }

    void Update()
    {
        foreach (Tab tab in GameObject.Find("TabArea").GetComponent<TabGroup>().tabs)
        {
            // use tab index to look up correct training preference set
            Settings.TrainingPrefs prefs = user.trainingPrefs[tab.transform.GetSiblingIndex()];
            tab.SetLabel(prefs.labelText);
        }
    }

    private Settings.User user;
    private TabGroup tabGroup;

}
