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

   // public UIAnimationTypes animationTypes;
    public UIAnimationTypes selectedType;

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

    public void SetTypeFromString(string input)
    {
        if (input == "Move")
            selectedType = UIAnimationTypes.Move;
        else if (input == "Scale")
            selectedType = UIAnimationTypes.Scale;
        else if (input == "Rotate")
            selectedType = UIAnimationTypes.Rotate;
        else if (input == "ScaleX")
            selectedType = UIAnimationTypes.ScaleX;
        else if (input == "ScaleY")
            selectedType = UIAnimationTypes.ScaleY;
        else if (input == "Shake")
            selectedType = UIAnimationTypes.Shake;
        else if (input == "Bounce")
            selectedType = UIAnimationTypes.Bounce;
        else if (input == "Grow")
            selectedType = UIAnimationTypes.Grow;
        else if (input == "Wiggle")
            selectedType = UIAnimationTypes.Wiggle;
        else if (input == "Pulse")
            selectedType = UIAnimationTypes.Pulse;
    }
    
    public void HandleTween()
    {

        if (objectToAnimate == null)
        {
            objectToAnimate = gameObject;
        }

        if(selectedType == UIAnimationTypes.Move)
            LeanTween.move(objectToAnimate, new Vector3(objectToAnimate.transform.position.x + 1f, objectToAnimate.transform.position.y, objectToAnimate.transform.position.z), duration).setEase(LeanTweenType.linear);
        else if (selectedType == UIAnimationTypes.Scale)
            LeanTween.scale(objectToAnimate, new Vector3(objectToAnimate.transform.localScale.x * 1.7f, objectToAnimate.transform.localScale.y * 1.7f, objectToAnimate.transform.localScale.z * 1.7f), duration).setEase(easeType);
        else if(selectedType == UIAnimationTypes.Rotate)
            LeanTween.rotateAround(objectToAnimate, Vector3.forward, -360f, duration);
        else if(selectedType == UIAnimationTypes.ScaleX)
            LeanTween.scaleX(objectToAnimate, objectToAnimate.transform.localScale.x * 1.7f, duration).setEase(easeType);
        else if (selectedType == UIAnimationTypes.ScaleY)
            LeanTween.scaleY(objectToAnimate, objectToAnimate.transform.localScale.y * 1.7f, duration).setEase(easeType);
        else if (selectedType == UIAnimationTypes.Shake)
            LeanTween.moveX(objectToAnimate, objectToAnimate.transform.position.x + 1f, duration).setEase(easeType);
        else if (selectedType == UIAnimationTypes.Bounce)
            LeanTween.scale(objectToAnimate, new Vector3(objectToAnimate.transform.localScale.x * 1.7f, objectToAnimate.transform.localScale.y * 1.7f, objectToAnimate.transform.localScale.z * 1.7f), duration).setEase(LeanTweenType.easeOutBounce);
        else if (selectedType == UIAnimationTypes.Grow)
        {
            LeanTween.scale(objectToAnimate, new Vector3(objectToAnimate.transform.localScale.x * 1.7f, objectToAnimate.transform.localScale.y * 1.7f, objectToAnimate.transform.localScale.z * 1.7f), duration).setEase(LeanTweenType.easeOutBounce);
            LeanTween.moveX(objectToAnimate, objectToAnimate.transform.position.x + 1f, duration).setEase(LeanTweenType.easeOutBounce);
        }
        else if (selectedType == UIAnimationTypes.Wiggle)
        {
            LeanTween.moveX(objectToAnimate, objectToAnimate.transform.position.x + 1f, duration).setEase(LeanTweenType.easeOutBounce);
            LeanTween.moveX(objectToAnimate, objectToAnimate.transform.position.x - 1f, duration).setEase(LeanTweenType.easeOutBounce);
        }
        else if (selectedType == UIAnimationTypes.Pulse)
        {
            LeanTween.scale(objectToAnimate, new Vector3(objectToAnimate.transform.localScale.x * 1.7f, objectToAnimate.transform.localScale.y * 1.7f, objectToAnimate.transform.localScale.z * 1.7f), duration).setEase(LeanTweenType.easeOutBounce);
            LeanTween.scale(objectToAnimate, new Vector3(objectToAnimate.transform.localScale.x * 0.2f, objectToAnimate.transform.localScale.y * 0.2f, objectToAnimate.transform.localScale.z * 0.2f), duration).setEase(LeanTweenType.easeOutBounce);
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


}
