using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateLabelMenu : MonoBehaviour
{
    TMP_InputField labelInputField;
    [SerializeField] SelectLabelMenu labelMenu;
    TMP_Text labelValidityMessage;

    public void SendLabel()
    {
        bool isUnique = true;

        // Get the input field with the profile name
        labelInputField = transform.Find("labelInputField").GetComponent<TMP_InputField>();
        
        // Get the text box to display the user profile name valididy message
        labelValidityMessage = transform.Find("labelValidityMessage").GetComponent<TMP_Text>(); ;
        
        // Check if the field is empty or if the profile name already exists
        if (labelInputField.text != "")
        {
            foreach (TMP_Dropdown.OptionData label in labelMenu.GetComponentInChildren<TMP_Dropdown>().options)
            {
                if (label.text == labelInputField.text)
                {
                    isUnique = false;
                }
            }
            
            if (isUnique)
            {
                // Add the profile name to the dropdown and start training
                labelValidityMessage.text = "";
                labelMenu.AddLabel(labelInputField.text);
                labelMenu.RemoveTitle();
            } 
            else
            {
                Debug.Log("Label is already present");
                labelValidityMessage.text = "Label is already present";
            }
        } 
        else
        {
            Debug.Log("Label cannot be empty");
            labelValidityMessage.text = "Label cannot be empty";
        }
    }
}