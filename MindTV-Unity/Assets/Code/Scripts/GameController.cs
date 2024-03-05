using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    Resolution[] resolutions;

    public TMP_Dropdown resolutionDropdown;

    public Slider volumeSlider;

    private void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> optionsRes = new List<string>();
        int currentResolutionIndex = 0;
        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            optionsRes.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }

        }
        resolutionDropdown.AddOptions(optionsRes);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void Update()
    {
        // Quit application if you are in the main menu
        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name == "MainMenu")
        {
            QuitGame();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name != "MainMenu")
        {
            LoadMainMenu();
        }
    }
    // Quit application if you are in the main menu
    public void QuitGame()
    {
        Application.Quit();
    }

    // Load the main menu scene
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void SetFullScreenResolution(int width, int height)
    {
        Screen.SetResolution(width, height, true);
    }

    public void SetWindowedResolution(int width, int height)
    {
        Screen.SetResolution(width, height, false);
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
