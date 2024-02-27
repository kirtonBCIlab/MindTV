using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;
using System; 
using System.IO; 
using System.Text; 

public class VideoSelection : MonoBehaviour
{
    // public TextMeshProUGUI VideoLabel; 
    string filename = null; 
    string strPath = null; 
    string defaultDir = null;

    // set the default folder's directory. we can change this later if we change the location of the folder 
    string projDirectory = System.IO.Directory.GetCurrentDirectory();
    
    public void ButtonPressed()
    {
        defaultDir = projDirectory + "\\Assets\\Videos";
        strPath = EditorUtility.OpenFilePanel("Choose video file ...", defaultDir, "mp4");
        Debug.Log(strPath);
        // check for the correct file format 
        // if (strPath.Contains(".mp4"))
        // {
        filename = Path.GetFileName(strPath); 
        if (!string.IsNullOrWhiteSpace(strPath))
        {
            System.IO.File.Copy(strPath, defaultDir + "\\" + filename , true);  // copy file into assets/video folder and overwrite if it already exists 
        } 
        // }

    }


}
