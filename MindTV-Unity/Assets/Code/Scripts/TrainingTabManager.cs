using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    private void Start()
    {
        // Subscribe to training pref changes
        TrainingPageManager.TrainingPrefsChanged += MatchTabsToTrainingPages;

        // TODO - this won't work as the child tabs don't exist yet.  A better solution is to
        // have the tab manager create the tabs from data rather than relying on 4 tabs existing.
        // This class could handle it, possibly asking TabManager to add the tabs.

        // Initial update to sync tabs to settings
        //MatchTabsToTrainingPages();
    }

    private void OnDisable()
    {
        // Remove listener to avoid dangling references (event is static and persists between scenes)
        TrainingPageManager.TrainingPrefsChanged -= MatchTabsToTrainingPages;
    }

    private void MatchTabsToTrainingPages()
    {
        Settings.User user = SettingsManager.Instance?.currentUser ?? new Settings.User();
        TabGroup tabGroup = GetComponent<TabGroup>();

        if (tabGroup != null)
        {
            for (int index = 0; index < user.trainingPrefs.Count; index++)
            {
                // the tab index aligns with the training page index
                Settings.TrainingPrefs pref = user.trainingPrefs[index];
                tabGroup.SetTabAppearance(index, pref.labelName, pref.backgroundColor);
            }
        }
    }
}
