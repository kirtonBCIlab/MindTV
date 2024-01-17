using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BessyConnectionManager : MonoBehaviour
{
    public Image bessyStatusIndicator;
    public TextMeshProUGUI bessyStatusText;

    private bool isBessyConnected = false;

    // Call this method when the Bessy Connect button is clicked
    public void ButtonBessyConnection()
    {
        isBessyConnected = !isBessyConnected;
        UpdateBessyConnectionStatus();
    }

    // Update the Bessy connection status UI
    private void UpdateBessyConnectionStatus()
    {
        if (isBessyConnected)
        {
            bessyStatusIndicator.color = Color.green; // Unity's predefined green color
            bessyStatusText.text = "Connected";
            bessyStatusText.color = Color.green; // Text color changed to green
        }
        else
        {
            bessyStatusIndicator.color = Color.red; // Unity's predefined red color
            bessyStatusText.text = "Not Connected";
            bessyStatusText.color = Color.red; // Text color changed to red
        }
    }
}
