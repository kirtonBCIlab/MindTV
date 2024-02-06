using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityCell : MonoBehaviour
{
    [SerializeField] private CellObject cellObject;
    public string label;
    public bool useInTraining;
    public CellObjectType cellObjectType;
    
    public ActivityCell(CellObject cellObject)
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

    public void GoToVideoScene()
    {
        Debug.Log("Go to a scene with a video");
    }

    public void GoToNavigationScene()
    {
        Debug.Log("Go to navigation scene");
    }

    public void PlayVideo()
    {
        Debug.Log("Play video");
    }

    public void PauseVideo()
    {
        Debug.Log("Pause video");
    }

    public void StopVideo()
    {
        Debug.Log("Stop video");
    }



}
