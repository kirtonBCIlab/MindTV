using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class LaunchExternalApplication : MonoBehaviour
{
    // The relative directory path to the script or executable
    // Relative to the Unity project folder
    // Adjust the directory path as necessary
    private string _relativeDirectoryPath = @"../MindTV-Python/";

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

        // Combine the directory path with the filename
        string fullPath = System.IO.Path.Combine(System.IO.Path.GetFullPath(_relativeDirectoryPath), fileName);

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
