using System.Collections;
using System.Collections.Generic;
using BCIEssentials.StimulusEffects;
using BCIEssentials.StimulusObjects;
using BCIEssentials.Controllers;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(SPO))]
public class VideoPanelButtonEffect : StimulusEffect
{
    // Start is called before the first frame update
    [SerializeField] 
    [Tooltip("The background which will flash with P300 on the cell")]
    private Image _backgroundCellImage;

    [Header("Flash Settings")]
    [SerializeField]
    [Tooltip("Color to assign while flashing is on")]
    private Color _flashOnColor = Color.red;

    //This is just hard coded right now.
    public Color _flashOffColor;

    [SerializeField]
    [Tooltip("If the flash on color is applied on start or the flash off color.")]
    private bool _startOn;

    // private UnityAction _startStimulusAction;
    // private UnityAction _stopStimulusAction;

        private void Awake()
    {
        if (_backgroundCellImage == null && gameObject.GetComponentInChildren<Image>() == null)
        {
            Debug.LogWarning($"No Image component found for {gameObject.name} or children");
            return;
        }

        if (_backgroundCellImage == null)
        {
            Debug.LogWarning($"No Image component set for {gameObject.name}.");
        }

    }

    private void Start()
    {
        // Make sure this only runs if P300 is the active behavior
        if (BCIController.Instance.ActiveBehavior.BehaviorType != BCIBehaviorType.P300)
        {
            Debug.LogWarning("P300 is not the active behavior, so this effect will not run");
            //TODO - We could make an effect for the other stimulus types if we would like.
            return;
        }
        else
        {
            AssignBackgroundColor(_startOn ? _flashOnColor: _flashOffColor);
            gameObject.GetComponent<SPO>().StartStimulusEvent.AddListener(SetOn);
            // Getting errors going this way, probably am missing something
            //gameObject.GetComponent<SPO>().StartStimulusEvent += TurnOnBackground;
            gameObject.GetComponent<SPO>().StopStimulusEvent.AddListener(SetOff);
        }


    }

    public override void SetOn()
    {
        if (_backgroundCellImage == null)
        {
            return;
        }

        AssignBackgroundColor(_flashOnColor);
        IsOn = true;
    }

    public override void SetOff()
    {
        if (_backgroundCellImage == null )
        {
            return;
        }
        
        AssignBackgroundColor(_flashOffColor);
        IsOn = false;
    }

    private void TurnOnBackground()
    {
        AssignBackgroundColor(_flashOnColor);
    }

    private void AssignBackgroundColor(Color color)
    {
        _backgroundCellImage.color = color;
    }
}
