using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TrainingItem : MonoBehaviour
{
    public Sprite sprite;

    //holds the image property of the current training object
    void Start()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
    }
}