using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class VideoAddCellManager : MonoBehaviour
{
    [SerializeField] private Button addButton;

    // Signal that add button was pressed
    public static event Action VideoAddCellClicked;

    void Start()
    {
        addButton.onClick.AddListener(AddButtonClicked);
    }

    void AddButtonClicked()
    {
        VideoAddCellClicked?.Invoke();
    }
}
