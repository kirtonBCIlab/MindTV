using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BCIEssentials.Controllers;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private GameObject _bciControllerGO;

    public BCIController bci;
    void Start()
    {
        _bciControllerGO = GameObject.FindWithTag("BCIController");
        bci = _bciControllerGO.GetComponent<BCIController>();

    }

    
    public void UpdateBCI()
    {
        if (bci._readyToStart == true)
            bci._readyToStart = false;
        else 
            bci._readyToStart = true;
    }
}
