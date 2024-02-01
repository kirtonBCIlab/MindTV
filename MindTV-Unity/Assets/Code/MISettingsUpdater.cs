using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MISettingsUpdater : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // When this object is enabled, update the MI Settings from the BCI Controller Object
    private void OnEnable()
    {
        print("I am enabled!");

        // // Get the BCI Controller Object
        // GameObject bciController = GameObject.Find("BciControllerManager");
        // if (bciController != null)
        // {
        //     print("Found BCI Controller");
        //     // Get the BCI Controller script
        //     bciControllerScript = bciController.GetComponent<MIControllerBehavior>();
        //     // if (bciControllerScript != null)
        //     // {
        //     //     // Update the MI Settings from the BCI Controller
        //     //     MIManager.Instance.miSettings = bciControllerScript.miSettings;
        //     // }
        // }

        // Write the settings to the menu

    }

    // // When the update button is pressed, push MI Settings to the BCI Controller Object
    // private void PushSettings()
    // {
    //     // Get the BCI Controller Object
    //     GameObject bciController = GameObject.Find("BCIController");
    //     if (bciController != null)
    //     {
    //         // Get the BCI Controller script
    //         BCIController bciControllerScript = bciController.GetComponent<BCIController>();
    //         if (bciControllerScript != null)
    //         {
    //             // Push the MI Settings to the BCI Controller
    //             bciControllerScript.miSettings = MIManager.Instance.miSettings;
    //         }
    //     }
    // }
}   
