using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

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


}
