using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

/// <summary>
/// TrainingTabManager keeps the TabGroup matched to the TrainingPages
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

    // TODO - it would be more efficient to trigger this from an event, ex: Settings value change.
    void Update()
    {
        MatchTabsToTrainingPages();
    }

    void MatchTabsToTrainingPages()
    {
        foreach (Settings.TrainingPrefs pref in user.trainingPrefs)
        {
            tabGroup.SetTabAppearance(pref.labelNumber, pref.labelText, pref.backgroundColor);
        }
    }

    private Settings.User user;
    private TabGroup tabGroup;

}
