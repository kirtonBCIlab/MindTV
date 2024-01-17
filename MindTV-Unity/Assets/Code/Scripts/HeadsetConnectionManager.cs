using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HeadsetConnectManager : MonoBehaviour
{
    public Image headsetStatusIndicator;
    public TextMeshProUGUI headsetStatusText;

    private bool isHeadsetConnected = false;

    // Call this method when the Headset Connect button is clicked
    public void ButtonHeadsetConnection()
    {
        isHeadsetConnected = !isHeadsetConnected;
        UpdateHeadsetConnectionStatus();
    }

    // Update the headset connection status UI
    private void UpdateHeadsetConnectionStatus()
    {
        if (isHeadsetConnected)
        {
            headsetStatusIndicator.color = Color.green; // Unity's predefined green color
            headsetStatusText.text = "Connected";
            headsetStatusText.color = Color.green; // Text color changed to green
        }
        else
        {
            headsetStatusIndicator.color = Color.red; // Unity's predefined red color
            headsetStatusText.text = "Not Connected";
            headsetStatusText.color = Color.red; // Text color changed to red
        }
    }
}