using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class SettingsTab : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMP_Dropdown fullScreenDropdown;
    Resolution[] resolutions;

    private void Awake()
    {
        resolutionDropdown.ClearOptions();
        List<string> options = new();
        int currentResolutionIndex = 0;

        // Initialize the resolution dropdown
        resolutions = Screen.resolutions.Select(resolution => new Resolution { width = resolution.width, height = resolution.height }).Distinct().ToArray();

        int index = 0;

        foreach (Resolution resolution in resolutions)
        {
            string option = resolution.width + "x" + resolution.height;
            options.Add(option);

            if (resolution.width == Screen.currentResolution.width && resolution.height == Screen.currentResolution.height)
            {
                currentResolutionIndex = index;
            }

            index += 1;
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;

        // Initialize the fullscreen dropdown
        if (Screen.fullScreen)
        {
            fullScreenDropdown.value = fullScreenDropdown.options.FindIndex((i) => { return i.text.Equals("On"); });
        }
        else
        {
            fullScreenDropdown.value = fullScreenDropdown.options.FindIndex((i) => { return i.text.Equals("Off"); });
        }
    }

    //toggles the application display to be fullscreen or windowed
    public void SetFullScreen()
    {
        string selection = fullScreenDropdown.options[fullScreenDropdown.value].text;

        if (selection == "On")
        {
            Screen.fullScreen = true;
        }
        else if (selection == "Off")
        {
            Screen.fullScreen = false;
        }
        
    }

    //sets the application resolution
    public void SetResolution()
    {
        Resolution res = resolutions[resolutionDropdown.value];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }
}
