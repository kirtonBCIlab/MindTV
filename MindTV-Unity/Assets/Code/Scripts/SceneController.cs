using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // Track the currently loaded additive scene
    private string currentAdditiveScene = "";

    // Don't destroy this on load
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    // This method toggles the additive scene based on the one requested.
    public void ToggleScene(string sceneName)
    {
        // If the current scene is the one requested, unload it.
        if (currentAdditiveScene == sceneName)
        {
            SceneManager.UnloadSceneAsync(sceneName);
            currentAdditiveScene = "";
        }
        else
        {
            // If there is a different scene currently loaded, unload it.
            if (!string.IsNullOrEmpty(currentAdditiveScene))
            {
                SceneManager.UnloadSceneAsync(currentAdditiveScene);
            }
            // Load the requested scene.
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            // Update the current scene tracker.
            currentAdditiveScene = sceneName;
        }
    }

    // Switch Scenes completely based on the Scene Name
    public void SwitchScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }


}