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
        // Subscribe to training pref changes
        TrainingPageManager.TrainingPrefsChanged += MatchTabsToTrainingPages;

        // Initial update to sync tabs to settings
        MatchTabsToTrainingPages();
    }

    void OnDisable()
    {
        // Remove listener to avoid dangling references (event is static and persists between scenes)
        TrainingPageManager.TrainingPrefsChanged -= MatchTabsToTrainingPages;
    }

    void MatchTabsToTrainingPages()
    {
        Settings.User user = SettingsManager.Instance?.currentUser ?? new Settings.User();
        TabGroup tabGroup = GetComponent<TabGroup>() ?? new TabGroup();

        foreach (Settings.TrainingPrefs pref in user.trainingPrefs)
        {
            tabGroup.SetTabAppearance(pref.labelNumber, pref.labelText, pref.backgroundColor);
        }
    }
}
