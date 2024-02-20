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
    public TextMeshProUGUI VideoLabel; 
    string filename = null; 
    string strPath = null; 
    string defaultDir = null;

    // set the default folder's directory. we can change this later if we change the location of the folder 
    string projDirectory = System.IO.Directory.GetCurrentDirectory();
    
    public void ButtonPressed()
    {
        defaultDir = projDirectory + "\\Assets\\VideoFiles";
        strPath = EditorUtility.OpenFilePanel("Choose video file ...", defaultDir, "avi");
        
        // check for the correct file format 
        if (strPath.Contains(".avi"))
        {
            filename = Path.GetFileName(strPath); 
            VideoLabel.GetComponent<TextMeshProUGUI>().text = filename;
        }
        // update the text label if not correct file format 
        else 
        {
            VideoLabel.GetComponent<TextMeshProUGUI>().text = "Choose valid file";
        }

    }


}
