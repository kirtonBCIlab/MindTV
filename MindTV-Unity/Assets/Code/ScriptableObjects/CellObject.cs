using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CellObjectType
{
    DefaultCell,
    TrainingCell,
    VideoCell,
    NavigationCell
}

public abstract class CellObject : ScriptableObject
{
    public string cellObjectName; //I don't know if this is needed.
    public CellObjectType cellObjectType;
    //Description of what this Cell Object is
    [TextArea (5,10)]
    public string description;
    public GameObject prefabModel;
    public Sprite sprite;
    public Vector2 prefabSize;
    public Color prefabColor;
    public AudioClip startSound;
    public bool saveAutomatically;
    public bool useInTraining;
    public AnimationClip[] feedbackAnimations;

 
}
