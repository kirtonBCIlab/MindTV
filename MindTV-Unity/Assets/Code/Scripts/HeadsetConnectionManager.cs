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
        Debug.Log("Waiting for a second to update the conneciton status...");
        Invoke("UpdateHeadsetConnectionStatus", _headsetCheckDelay);
    }

    public void UnsubscribeLSLResponseStream()
    {
        //This is depricated behavior
        _lslResponseStream.Disconnect();
        //Check the headset connectivity after the headset check delay
        Debug.Log("Waiting for a second to update the conneciton status...");
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

    public void LaunchExecutable(string fileName)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            UnityEngine.Debug.LogError("Filename is not provided. Please assign a filename in the Inspector.");
            return;
        }

        // Append '.exe' for Windows platforms if not already present
        #if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        if (!fileName.EndsWith(".exe"))
        {
            fileName += ".exe";
        }
        #endif

        // The relative directory path to the script or executable
        string relativeDirectoryPath = @"../MindTV-Python/"; // Adjust the directory path as necessary

        // Combine the directory path with the filename
        string fullPath = System.IO.Path.Combine(System.IO.Path.GetFullPath(relativeDirectoryPath), fileName);

        ProcessStartInfo processStartInfo = new ProcessStartInfo
        {
            FileName = fullPath,
            CreateNoWindow = false, // Set to true if you don't want to show a console window
            UseShellExecute = true, // Must be true to use FileName as a document on macOS
            RedirectStandardOutput = false, // Set to true to capture output (if needed)
            WorkingDirectory = System.IO.Path.GetDirectoryName(fullPath)
        };

        // Start the process
        Process process = new Process
        {
            StartInfo = processStartInfo
        };

        try
        {
            process.Start();
        }
        catch (System.Exception e)
        {
            UnityEngine.Debug.LogError("Failed to start the process with the given filename: " + e.Message);
        }

        // Optional: Wait for the process to finish
        // process.WaitForExit();
    }
}