using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CellObjectType
{
    TrainingCell,
    VideoCell,
    NavigationCell
}

[CreateAssetMenu(fileName = "New Cell Object", menuName = "Scriptable Objects/Cell Object")]
public class CellObject : ScriptableObject
{
    public Sprite sprite;
    public Vector2 prefabSize;
    public Color prefabColor;
    public bool includeOutline;
    public Color outlineColor;
    public AudioClip startSound;
    public bool saveAutomatically;
    public bool usedInTrainingSequence;
    public AnimationClip[] feedbackAnimations;
    public string label;
    public CellObjectType cellObjectType;
    public string cellObjectID;
    public string cellObjectPath;
    public GameObject prefab;

    [TextArea (5,10)]
    public string description;
}
