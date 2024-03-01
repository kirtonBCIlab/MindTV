using System.Collections;
using System.Collections.Generic;
using BCIEssentials.StimulusEffects;
using UnityEngine;
using UnityEngine.UI;
// using UnityEngine.UIElements;

public class VideoCellP300Effect : StimulusEffect
{
    [SerializeField] 
    [Tooltip("The background which will flash with P300 on the cell")]
    private Image _backgroundCellImage;

    [Header("Flash Settings")]
    [SerializeField]
    [Tooltip("Color to assign while flashing is on")]
    private Color _flashOnColor = Color.red;

    //This might be the wrong way to go about this.
    public Color _flashOffColor = Color.gray;

    [SerializeField]
    [Tooltip("If the flash on color is applied on start or the flash off color.")]
    private bool _startOn;
    
    [SerializeField]
    [Min(0)]
    private float _flashDurationSeconds = 0.2f;

    [SerializeField]
    [Min(1)]
    private int _flashAmount = 3;

    public bool IsPlaying => _effectRoutine != null;


    private Coroutine _effectRoutine;


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
        AssignBackgroundColor(_startOn ? _flashOnColor: _flashOffColor);
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

//This play method is not used in the current implementation, but it is left here for future use.
    public void Play()
    {
        Stop();
        _effectRoutine = StartCoroutine(RunEffect());
    }

    private void Stop()
    {
        if (!IsPlaying)
        {
            return;
        }

        SetOff();
        StopCoroutine(_effectRoutine);
        _effectRoutine = null;
    }

    private IEnumerator RunEffect()
    {
        if (_backgroundCellImage != null)
        {
            IsOn = true;
            
            for (var i = 0; i < _flashAmount; i++)
            {
                //Deliberately not using SetOn and SetOff here
                //to avoid excessive null checking
                
                AssignBackgroundColor(_flashOnColor);
                yield return new WaitForSecondsRealtime(_flashDurationSeconds);

                AssignBackgroundColor(_flashOffColor);
                yield return new WaitForSecondsRealtime(_flashDurationSeconds);
            }
        }

        SetOff();
        _effectRoutine = null;
    }

    private void AssignBackgroundColor(Color color)
    {
        _backgroundCellImage.color = color;
    }
    
}
