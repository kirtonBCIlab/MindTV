using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Training Cell Object", menuName = "Scriptable Objects/Training Cell Object")]
public class TrainingCellObject : CellObject
{
    //Not sure if we set the label here or in another spot.
    public string _label;
    //On awake we set the cell object type to training cell, make the cell include the training bool, and set the cell object ID and label
    private void Awake()
    {
        cellObjectType = CellObjectType.TrainingCell;
        useInTraining = true;
    }
}
