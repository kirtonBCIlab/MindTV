using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    //TO DO: develop method to turn headsetConnectionStatus to true when Headset is connected through LSL

    //manages the interactability of the start training button
    public bool headsetConnectionStatus = false;
    public Button enterTrainingBtn;

    //autiomatically sets the resolution of application to the display's resolution
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnBeforeSceneLoadRuntimeMethod()
    {
        Screen.SetResolution(Display.main.systemWidth, Display.main.systemHeight, Screen.fullScreen);
    }

    public void Update()
    {
        //updates the interactability of the start training button based on headsetConnectionStatus
        if (headsetConnectionStatus)
        {
            enterTrainingBtn.interactable = true;
        }
        else
        {
            enterTrainingBtn.interactable = false;
        }
    }

    //enters training scene when start training button is clicked
    public void startTraining()
    {
        SceneManager.LoadScene("Training");
    }

    //quits the app when close icon is clicked
    public void QuitGame()
    {
        Application.Quit();
    }
}
