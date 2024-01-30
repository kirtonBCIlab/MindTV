using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BCIEssentials.Controllers;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private GameObject _controllerManager;
    public bool _enableHotKeys = true;
    public BCIController bci;
    void Start()
    {
        _controllerManager = GameObject.FindWithTag("BCIController");
        bci = _controllerManager.GetComponent<BCIController>();
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
}
