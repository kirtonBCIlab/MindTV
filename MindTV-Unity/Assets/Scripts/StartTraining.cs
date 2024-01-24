using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BCIEssentials.Controllers;

public class StartTraining : MonoBehaviour
{
    [SerializeField]
    private GameObject controllerManager;
    [SerializeField]
    private BCIController bciController;
    private void Awake()
    {
        //bind with the BCI controller game object
        if (bciController == null)
        {
            controllerManager = GameObject.FindGameObjectWithTag("ControllerManager");
            Debug.Log("No BCI Controller Found. Assigning one now.");
            StartCoroutine(InitCoroutine());
        }
    }

    //starts MI iterative training when user clicks the train button in ActiveTraining tab
    public void StartAutoTraining()
    {
        bciController.ActiveBehavior.StartTraining(BCITrainingType.Iterative);
    }

    //retrieving the bciController
    private IEnumerator InitCoroutine()
    {
        yield return new WaitForEndOfFrame();
        Debug.Log("Going to assign the value now to bciController for Start Training Button");
        bciController = controllerManager.GetComponent<BCIController>();
    }
}
