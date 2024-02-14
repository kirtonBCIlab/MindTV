using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class VideoPageOptionsController : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Dropdown mentalCommandDropdown;
    public TMP_Text mentalCommandText;
    public TMP_Dropdown backgroundColorDropdown;
    public GameObject videoCellBackground;
    public TMP_Text videoTitleText;
    public TMP_InputField videoTitleInputField;
   
    
    void Start()
    {
        
    }

    public void ChangeCellColor()
    {
        Image background = videoCellBackground.GetComponent<Image>();
        string colorText = backgroundColorDropdown.options[backgroundColorDropdown.value].text;
        Color color = ColorByName.colors[colorText];
        background.color = color;
        Debug.Log(colorText);
    }

    public void ChangeMentalCommand()
    {
        string mentalCommand = mentalCommandDropdown.options[mentalCommandDropdown.value].text;
        mentalCommandText.text = mentalCommand;
        Debug.Log(mentalCommand);
    }

    public void ChangeVideoTitle()
    {
        videoTitleText.text = videoTitleInputField.text;
    }

    public void ToggleImageOn()
    {
        
    }

}
