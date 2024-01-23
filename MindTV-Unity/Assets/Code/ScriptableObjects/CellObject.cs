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
    public Sprite sprite;
    public Vector2 prefabSize;
    public Color prefabColor;
    public bool includeOutline;
    public Color outlineColor;
    public AudioClip startSound;
    public bool saveAutomatically;
    public bool useInTraining;
    public AnimationClip[] feedbackAnimations;
    public CellObjectType cellObjectType;
    public GameObject prefab;

    [TextArea (5,10)]
    public string description;
}
