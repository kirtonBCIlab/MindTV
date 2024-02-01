using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TMP_InputField))]
public class UpdateTabLabel : MonoBehaviour
{

    [SerializeField] private TabGroup tabGroup;

    private void Start()
    {
        GetComponent<TMP_InputField>().onEndEdit.AddListener(delegate { UpdateTabLabelWithInput(); });
        tabGroup = GameObject.Find("TabArea").GetComponent<TabGroup>();
    }

    private void UpdateTabLabelWithInput()
    {
        // Get the user's input
        string userInput = GetComponent<TMP_InputField>().text;


        // Update the tab label with the user's input
        
        if(tabGroup.selectedTab != null)
        {
            tabGroup.selectedTab.GetComponentInChildren<TextMeshProUGUI>().text = userInput;
        }
        else
        {
            Debug.Log("No tab selected");
        }

    }
}
