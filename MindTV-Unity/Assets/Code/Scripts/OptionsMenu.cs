using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BCIEssentials.Controllers;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    public bool _enableHotKeys = true;

    [SerializeField] private TMP_Dropdown _behaviorDropdown;

    private void Start()
    {

        _behaviorDropdown.onValueChanged.AddListener(UpdateBCIBehavior);
    }

    public void OnToggleSelected()
    {
        if (_enableHotKeys == true)
        {
            _enableHotKeys = false;
        }
        else
        {
            _enableHotKeys = true;
        }
        UpdateBCI();
    }

    public void UpdateBCI()
    {
        if (_enableHotKeys == true)
            BCIController.EnableDisableHotkeys(true);
        else 
            BCIController.EnableDisableHotkeys(false);
    }

     public void UpdateBCIBehavior(int valueChanged)
    {
        string newBehavior = _behaviorDropdown.options[valueChanged].text;

        switch (newBehavior)
        {
            case "Mental Imagery":
                Debug.Log("Switching to Motor Imagery");
                BCIController.ChangeBehavior(BCIBehaviorType.MI);
                break;
            case "P300":
                Debug.Log("Switching to P300");
                BCIController.ChangeBehavior(BCIBehaviorType.P300);
                break;
            case "SSVEP":
                Debug.Log("Switching to SSVEP");
                BCIController.ChangeBehavior(BCIBehaviorType.SSVEP);
                break;
            default:
                Debug.Log("Invalid BCI Behavior");
                break;
        }
    }
}
