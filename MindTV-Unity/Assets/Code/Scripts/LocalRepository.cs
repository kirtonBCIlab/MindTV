using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;


/// <summary>
/// LocalRepository is responsible for reading/writing user data to the persistent
/// data path.  This data will persist between application installations, etc.
/// </summary>
public static class LocalRepository
{
    public static string SettingsFile = "UserData.dat";

    public static bool StoreSettings(string json)
    {
        bool success = false;
        var fullPath = Path.Combine(Application.persistentDataPath, SettingsFile);
        try
        {
            File.WriteAllText(fullPath, json);
            Debug.Log("LocalRepository: writing settings to " + fullPath);
            success = true;
        }
        catch (Exception e)
        {
            Debug.LogError($"LocalRepository: Failed to write to {fullPath} with exception {e}");
        }
        return success;
    }

    public static bool LoadSettings(out string json)
    {
        json = "";
        bool success = false;
        var fullPath = Path.Combine(Application.persistentDataPath, SettingsFile);
        try
        {
            json = File.ReadAllText(fullPath);
            success = true;
        }
        catch (Exception e)
        {
            Debug.LogError($"LocalRepository: Failed to read from {fullPath} with exception {e}");
        }
        return success;
    }


    // TODO - use this to support a database of images for an inventory display
    // public static List<string> GetTrainingImageNames()
    // {
    //     // lookup training image pngs stored in the repository, assume they're all "training images"
    //     string[] pngFiles = Directory.GetFiles(Application.persistentDataPath, "*.png");
    //     List<string> names = new List<string>();

    //     // Strip out the path and just return the filenames
    //     foreach(string file in pngFiles)
    //     {
    //         names.Add(Path.GetFileName(file));
    //     }
    //     return names;
    // }

    // public static bool ImportTrainingImage(string pngFilePath, out string imageName)
    // {
    //     // stores a png file to the local repository and return name of image
    //     imageName = Path.GetFileName(pngFilePath);
    //     bool success = false;
    //     try
    //     {
    //         string repositoryImagePath = Path.Combine(Application.persistentDataPath, imageName);
    //         File.Copy(pngFilePath, repositoryImagePath);
    //         success = true;
    //     }
    //     catch (Exception e)
    //     {
    //         Debug.LogError($"LocalRepository: Failed to import {pngFilePath} with exception {e}");
    //     }
    //     return success;
    // }

    // public static bool GetTrainingImage(string imageName, out Texture2D texture)
    // {
    //     // returns a texture for the given image name, caller is responsible to resize / convert to sprite as needed
    //     texture = null;
    //     bool success = false;
    //     try
    //     {
    //         string repositoryImagePath = Path.Combine(Application.persistentDataPath, imageName);
    //         byte[] imageData = File.ReadAllBytes(repositoryImagePath);

    //         texture = new(1, 1);
    //         texture.LoadImage(imageData);
    //         success = true;
    //     }
    //     catch (Exception e)
    //     {
    //         Debug.LogError($"LocalRepository: Failed to find {imageName} with exception {e}");
    //     }

    //     return success;
    // }
}