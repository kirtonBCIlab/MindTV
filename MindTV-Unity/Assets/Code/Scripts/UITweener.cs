using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public enum UIAnimationTypes
{
    Move,
    Scale,
    Rotate,
    ScaleX,
    ScaleY,
    Shake,
    Bounce,
    Grow,
    Wiggle,
    Pulse,
}

public class UITweener : MonoBehaviour
{

    public GameObject objectToAnimate;

    public UIAnimationTypes animationTypes;

    public LeanTweenType easeType;

    public float duration = 1f;
    public float delay = 0f;
    public float tweenXScale = 1.7f;
    public float tweenYScale = 1.7f;
    public float tweenZScale = 1.7f;
    public float tweenRotation = 360f;
    public bool tweenClockwiseRotation = true;
    public float tweenNegScale = 0.2f;

    public float tweenXTranslation = 100f;
    public float tweenYTranslation = 100f;
    public float tweenZTranslation = 100f;

    public bool loop;
    public bool pingPong;

    // public bool startPositionOffset;
    // public Vector3 fromPosition;
    // public Vector3 toPosition;
    // public bool startScaleOffset;
    // public Vector3 fromScale;
    // public Vector3 toScale;
    // public bool startRotationOffset;
    // public Vector3 fromRotation;
    // public Vector3 toRotation;
    // public bool startAlphaOffset;
    // public float fromAlpha;
    // public float toAlpha;
    private LTDescr _tweenObject;

    public bool showOnEnable = false;
    public bool showOnDisable = false;

    public void OnEnable()
    {
        if (showOnEnable)
        {
            Show();
        }
    }

    public void Show()
    {
        HandleTween();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            HandleTween();
        }
    }

    public void HandleTween()
    {

        if (objectToAnimate == null)
        {
            objectToAnimate = gameObject;
        }

        switch (animationTypes)
        {
            case UIAnimationTypes.Move:
                LeanTween.move(objectToAnimate, new Vector3(objectToAnimate.transform.position.x + tweenXTranslation, objectToAnimate.transform.position.y + tweenYTranslation, objectToAnimate.transform.position.z+tweenZTranslation), duration).setEase(easeType);
                break;
            case UIAnimationTypes.Scale:
                LeanTween.scale(objectToAnimate, new Vector3(objectToAnimate.transform.localScale.x * tweenXScale, objectToAnimate.transform.localScale.y * tweenYScale, objectToAnimate.transform.localScale.z * tweenYScale), duration).setEase(easeType);
                break;
            case UIAnimationTypes.Rotate:
                if (tweenClockwiseRotation)
                    LeanTween.rotateAround(objectToAnimate, Vector3.forward, -tweenRotation, duration).setEase(easeType);
                else
                    LeanTween.rotateAround(objectToAnimate, Vector3.forward, tweenRotation, duration).setEase(easeType);
                break;
            case UIAnimationTypes.ScaleX:
                LeanTween.scaleX(objectToAnimate, objectToAnimate.transform.localScale.x * tweenXScale, duration).setEase(easeType);
                break;
            case UIAnimationTypes.ScaleY:
                LeanTween.scaleY(objectToAnimate, objectToAnimate.transform.localScale.y * tweenYScale, duration).setEase(easeType);
                break;
            case UIAnimationTypes.Shake:
                LeanTween.moveX(objectToAnimate, objectToAnimate.transform.position.x + tweenXTranslation, duration).setEase(easeType);
                LeanTween.moveX(objectToAnimate, objectToAnimate.transform.position.x -tweenXTranslation, duration).setEase(easeType);
                break;
            case UIAnimationTypes.Bounce:
                LeanTween.scale(objectToAnimate, new Vector3(objectToAnimate.transform.localScale.x * tweenXScale, objectToAnimate.transform.localScale.y * tweenYScale, objectToAnimate.transform.localScale.z * tweenZScale), duration).setEase(easeType);
                break;
            case UIAnimationTypes.Grow:
                LeanTween.scale(objectToAnimate, new Vector3(objectToAnimate.transform.localScale.x * tweenXScale, objectToAnimate.transform.localScale.y * tweenYScale, objectToAnimate.transform.localScale.z * tweenZScale), duration).setEase(easeType);
                LeanTween.moveX(objectToAnimate, objectToAnimate.transform.position.x + tweenXTranslation, duration).setEase(easeType);
                break;
            case UIAnimationTypes.Wiggle:
                LeanTween.moveX(objectToAnimate, objectToAnimate.transform.position.x + tweenXTranslation, duration).setEase(easeType);
                LeanTween.moveX(objectToAnimate, objectToAnimate.transform.position.x - tweenXTranslation, duration).setEase(easeType);
                break;
            case UIAnimationTypes.Pulse:
                LeanTween.scale(objectToAnimate, new Vector3(objectToAnimate.transform.localScale.x * tweenXScale, objectToAnimate.transform.localScale.y * tweenYScale, objectToAnimate.transform.localScale.z * tweenZScale), duration).setEase(easeType);
                LeanTween.scale(objectToAnimate, new Vector3(objectToAnimate.transform.localScale.x * tweenNegScale, objectToAnimate.transform.localScale.y * tweenNegScale, objectToAnimate.transform.localScale.z * tweenNegScale), duration).setEase(easeType);
                break;
            
        }

        if(loop)
        {
            _tweenObject.loopCount = int.MaxValue;
        }

        if(pingPong)
        {
            _tweenObject.setLoopPingPong();
        }

        // if (startPositionOffset)
        // {
        //     objectToAnimate.transform.position = fromPosition;
        // }

        // if (startScaleOffset)
        // {
        //     objectToAnimate.transform.localScale = fromScale;
        // }

        // if (startRotationOffset)
        // {
        //     objectToAnimate.transform.eulerAngles = fromRotation;
        // }


    }


}
