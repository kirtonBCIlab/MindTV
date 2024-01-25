using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LiveController : MonoBehaviour
{
    //TO DO: 
    //generate a output stream for classification results (push..) for HomeBCI to pick up (optional, can also be done in python end)
    //link the classification with the text and animation of the live mode

    //returns training scene
    public void ReturnTraining()
    {
        SceneManager.LoadScene("Training");
    }
}
