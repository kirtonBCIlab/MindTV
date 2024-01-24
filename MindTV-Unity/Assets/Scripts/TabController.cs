using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TabController : MonoBehaviour
{
    [SerializeField] Transform headsetTab;
    [SerializeField] Transform settingsTab;
    [SerializeField] TMP_Text headsetHeading;
    [SerializeField] TMP_Text settingsHeading;

    //changes the bolding of tab selection text in MainMenu (Headset, Settings)
    public void selectTab(TMP_Text tabHeading)
    {
        headsetHeading.fontStyle = FontStyles.Normal;
        settingsHeading.fontStyle = FontStyles.Normal;

        tabHeading.fontStyle = FontStyles.Bold;
    }
}
