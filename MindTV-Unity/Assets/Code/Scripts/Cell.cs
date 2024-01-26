using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private CellObject cellObject;
    public string label;
    public bool useInTraining;
    public CellObjectType cellObjectType;
    
    public Cell(CellObject cellObject)
    {
        ApplyCellObjectProperties(cellObject);
    }

    private void ApplyCellObjectProperties(CellObject cellObject)
    {
        this.cellObject = cellObject;
        
        gameObject.name = cellObject.name;
        gameObject.transform.localScale = cellObject.prefabSize;
        gameObject.GetComponent<SpriteRenderer>().color = cellObject.prefabColor;
        gameObject.GetComponent<AudioSource>().clip = cellObject.startSound;
        useInTraining = cellObject.useInTraining;
        cellObjectType = cellObject.cellObjectType;
    }

}
