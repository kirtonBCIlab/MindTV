using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BCIEssentials.Controllers;
using BCIEssentials.LSLFramework;

public class HeadsetConnectManager : MonoBehaviour
{
    public bool HeadsetConnected { get; private set; }

    [SerializeField] private Image _headsetStatusIndicator;
    [SerializeField] private TextMeshProUGUI _headsetStatusText;
    [SerializeField] private GameObject _bciControllerGO;
    [SerializeField] private float _headsetCheckDelay = 3.0f;

    private LSLResponseStream _lslResponseStream;

    private void Awake()
    {
        HeadsetConnected = false;
        _bciControllerGO = GameObject.FindWithTag("BCIController");
        _lslResponseStream = _bciControllerGO.GetComponent<LSLResponseStream>();
    }


    // Call this method when the Headset Connect button is clicked
    public void UpdateButtonHeadsetConnection()
    {
            UpdateHeadsetConnectionStatus();
    }

    public void SubscribeLSLResponseStream()
    {
        //This is depricated behavior
        _lslResponseStream.Connect();
        //Check the headset connectivity after the headset check delay
        Debug.Log("Waiting for a second to update the connection status...");
        Invoke("UpdateHeadsetConnectionStatus", _headsetCheckDelay);
    }

    public void UnsubscribeLSLResponseStream()
    {
        //This is depricated behavior
        _lslResponseStream.Disconnect();
        //Check the headset connectivity after the headset check delay
        Debug.Log("Waiting for a second to update the connection status...");
        Invoke("UpdateHeadsetConnectionStatus", _headsetCheckDelay);
    }

    // Update the headset connection status UI
    private void UpdateHeadsetConnectionStatus()
    {
        Debug.Log("Updating the headset connection...");
        if (_lslResponseStream.Connected)
        {
            _headsetStatusIndicator.color = Color.green; // Unity's predefined green color
            _headsetStatusText.text = "Connected";
            _headsetStatusText.color = Color.green; // Text color changed to green
        }
        else
        {
            _headsetStatusIndicator.color = Color.red; // Unity's predefined red color
            _headsetStatusText.text = "Not Connected";
            _headsetStatusText.color = Color.red; // Text color changed to red
        }
    }
}