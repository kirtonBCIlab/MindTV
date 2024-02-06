using System.Collections;
using System.Collections.Generic;
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

    public bool loop;
    public bool pingPong;

    public bool startPositionOffset;
    public Vector3 fromPosition;
    public Vector3 toPosition;
    public bool startScaleOffset;
    public Vector3 fromScale;
    public Vector3 toScale;
    public bool startRotationOffset;
    public Vector3 fromRotation;
    public Vector3 toRotation;
    public bool startAlphaOffset;
    public float fromAlpha;
    public float toAlpha;
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


    public void HandleTween()
    {

        if (objectToAnimate == null)
        {
            objectToAnimate = gameObject;
        }

        switch (animationTypes)
        {
            case UIAnimationTypes.Move:
                LeanTween.move(objectToAnimate, new Vector3(objectToAnimate.transform.position.x + 1f, objectToAnimate.transform.position.y, objectToAnimate.transform.position.z), duration).setEase(LeanTweenType.linear);
                break;
            case UIAnimationTypes.Scale:
                LeanTween.scale(objectToAnimate, new Vector3(objectToAnimate.transform.localScale.x * 1.7f, objectToAnimate.transform.localScale.y * 1.7f, objectToAnimate.transform.localScale.z * 1.7f), duration).setEase(easeType);
                break;
            case UIAnimationTypes.Rotate:
                LeanTween.rotateAround(objectToAnimate, Vector3.forward, -360f, duration);
                break;
            case UIAnimationTypes.ScaleX:
                LeanTween.scaleX(objectToAnimate, objectToAnimate.transform.localScale.x * 1.7f, duration).setEase(easeType);
                break;
            case UIAnimationTypes.ScaleY:
                LeanTween.scaleY(objectToAnimate, objectToAnimate.transform.localScale.y * 1.7f, duration).setEase(easeType);
                break;
            case UIAnimationTypes.Shake:
                LeanTween.moveX(objectToAnimate, objectToAnimate.transform.position.x + 1f, duration).setEase(easeType);
                break;
            case UIAnimationTypes.Bounce:
                LeanTween.scale(objectToAnimate, new Vector3(objectToAnimate.transform.localScale.x * 1.7f, objectToAnimate.transform.localScale.y * 1.7f, objectToAnimate.transform.localScale.z * 1.7f), duration).setEase(LeanTweenType.easeOutBounce);
                break;
            case UIAnimationTypes.Grow:
                LeanTween.scale(objectToAnimate, new Vector3(objectToAnimate.transform.localScale.x * 1.7f, objectToAnimate.transform.localScale.y * 1.7f, objectToAnimate.transform.localScale.z * 1.7f), duration).setEase(LeanTweenType.easeOutBounce);
                LeanTween.moveX(objectToAnimate, objectToAnimate.transform.position.x + 1f, duration).setEase(LeanTweenType.easeOutBounce);
                break;
            case UIAnimationTypes.Wiggle:
                LeanTween.moveX(objectToAnimate, objectToAnimate.transform.position.x + 1f, duration).setEase(LeanTweenType.easeOutBounce);
                LeanTween.moveX(objectToAnimate, objectToAnimate.transform.position.x - 1f, duration).setEase(LeanTweenType.easeOutBounce);
                break;
            case UIAnimationTypes.Pulse:
                LeanTween.scale(objectToAnimate, new Vector3(objectToAnimate.transform.localScale.x * 1.7f, objectToAnimate.transform.localScale.y * 1.7f, objectToAnimate.transform.localScale.z * 1.7f), duration).setEase(LeanTweenType.easeOutBounce);
                LeanTween.scale(objectToAnimate, new Vector3(objectToAnimate.transform.localScale.x * 0.2f, objectToAnimate.transform.localScale.y * 0.2f, objectToAnimate.transform.localScale.z * 0.2f), duration).setEase(LeanTweenType.easeOutBounce);
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

        if (startPositionOffset)
        {
            objectToAnimate.transform.position = fromPosition;
        }

        if (startScaleOffset)
        {
            objectToAnimate.transform.localScale = fromScale;
        }

        if (startRotationOffset)
        {
            objectToAnimate.transform.eulerAngles = fromRotation;
        }


    }

    public void Move()
    {
        LeanTween.move(objectToAnimate, new Vector3(objectToAnimate.transform.position.x + 1f, objectToAnimate.transform.position.y, objectToAnimate.transform.position.z), duration).setEase(LeanTweenType.linear);
    }

    public void Scale()
    {
        LeanTween.scale(objectToAnimate, new Vector3(objectToAnimate.transform.localScale.x * 1.7f, objectToAnimate.transform.localScale.y * 1.7f, objectToAnimate.transform.localScale.z * 1.7f), duration).setEase(easeType);
    }   

    public void Rotate()
    {
        LeanTween.rotateAround(objectToAnimate, Vector3.forward, -360f, duration);
    }

    public void Fade()
    {
        if(gameObject.GetComponent<CanvasGroup>() == null)
        {
            gameObject.AddComponent<CanvasGroup>();
        }

        if(startPositionOffset)
        {
            objectToAnimate.GetComponent<CanvasGroup>().alpha = fromPosition.x;
        }
        _tweenObject = LeanTween.alphaCanvas(objectToAnimate.GetComponent<CanvasGroup>(), toPosition.x, duration);
    }


}
