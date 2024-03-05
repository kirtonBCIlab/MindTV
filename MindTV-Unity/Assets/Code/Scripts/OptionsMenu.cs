using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BCIEssentials.Controllers;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    public bool _enableHotKeys = true;

    [SerializeField] private TMP_Dropdown _behaviorDropdown;

    private Resolution[] resolutions;

    [SerializeField] private TMP_Dropdown resolutionDropdown;

    private int _currentResolutionIndex = 0;

    private void Start()
    {

        _behaviorDropdown.onValueChanged.AddListener(UpdateBCIBehavior);

        //Deal with Resolutions
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> optionsRes = new List<string>();
        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            optionsRes.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                _currentResolutionIndex = i;
            }

        }
        resolutionDropdown.AddOptions(optionsRes);
        resolutionDropdown.value = _currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void OnToggleSelected()
    {
        if (_enableHotKeys == true)
        {
            _enableHotKeys = false;
        }
        else
        {
            _enableHotKeys = true;
        }
        UpdateBCI();
    }

    public void UpdateBCI()
    {
        if (_enableHotKeys == true)
            BCIController.EnableDisableHotkeys(true);
        else 
            BCIController.EnableDisableHotkeys(false);
    }

     public void UpdateBCIBehavior(int valueChanged)
    {
        string newBehavior = _behaviorDropdown.options[valueChanged].text;

        switch (newBehavior)
        {
            case "Mental Imagery":
                Debug.Log("Switching to Motor Imagery");
                BCIController.ChangeBehavior(BCIBehaviorType.MI);
                break;
            case "P300":
                Debug.Log("Switching to P300");
                BCIController.ChangeBehavior(BCIBehaviorType.P300);
                break;
            case "SSVEP":
                Debug.Log("Switching to SSVEP");
                BCIController.ChangeBehavior(BCIBehaviorType.SSVEP);
                break;
            default:
                Debug.Log("Invalid BCI Behavior");
                break;
        }
    }

    
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
}
