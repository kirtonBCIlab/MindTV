using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public enum UIAnimationTypes
{
    Move,
    Rotate,
    ScaleX,
    ScaleY,
    Shake,
    Bounce,
    BouncePingPong,
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

    [SerializeField] private int numShakes = 8;
    [SerializeField] private float shakeSpeed = 0.05f;
    [SerializeField] private float shakeDistance = 3f;

    [SerializeField] private int numWiggles = 4;
    [SerializeField] private float wiggleSpeed = 0.1f;
    [SerializeField] private float wiggleRotAngle = 45f;

    // public bool loop;
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
            case UIAnimationTypes.Grow:
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
                ShakeAnim();
                break;
            case UIAnimationTypes.Bounce:
                BounceAnim();
                break;
            case UIAnimationTypes.BouncePingPong:
                BouncePingPong();
                break;
            case UIAnimationTypes.Wiggle:
                WiggleAnim();
                break;
            case UIAnimationTypes.Pulse:
                LeanTween.scale(objectToAnimate, new Vector3(objectToAnimate.transform.localScale.x * tweenXScale, objectToAnimate.transform.localScale.y * tweenYScale, objectToAnimate.transform.localScale.z * tweenZScale), duration).setEase(easeType);
                LeanTween.scale(objectToAnimate, new Vector3(objectToAnimate.transform.localScale.x * tweenNegScale, objectToAnimate.transform.localScale.y * tweenNegScale, objectToAnimate.transform.localScale.z * tweenNegScale), duration).setEase(easeType);
                break;
            
        }

        // //These don't seem to work!
        // if(loop)
        // {
        //     _tweenObject.loopCount = int.MaxValue;
        // }

        // if(pingPong)
        // {
        //     _tweenObject.setLoopPingPong();
        // }


    }

    // public void MoveAbsolute()
    // {
    //     objectToAnimate.GetComponent<RectTransform>().anchoredPosition = fromPosition;
    //     _tweenObject = LeanTween.move(objectToAnimate.GetComponent<RectTransform>(), toPosition, duration).setEase(easeType);
    // }
    public void BounceAnim()
    {
        _tweenObject = LeanTween.moveLocalY(objectToAnimate, objectToAnimate.transform.position.y + tweenYTranslation, duration).setEase(LeanTweenType.punch);
    }

    public void BouncePingPong()
    {
        _tweenObject = LeanTween.moveLocalY(objectToAnimate, objectToAnimate.transform.position.y + tweenYTranslation, duration).setLoopPingPong();

    }

    public void GrowAnim()
    {
        if(pingPong)
        {
            _tweenObject = LeanTween.scale(objectToAnimate, new Vector3(objectToAnimate.transform.localScale.x * tweenXScale, objectToAnimate.transform.localScale.y * tweenYScale, objectToAnimate.transform.localScale.z * tweenYScale), duration).setLoopPingPong();
        }
        else
        {
            _tweenObject = LeanTween.scale(objectToAnimate, new Vector3(objectToAnimate.transform.localScale.x * tweenXScale, objectToAnimate.transform.localScale.y * tweenYScale, objectToAnimate.transform.localScale.z * tweenYScale), duration).setEase(easeType);
        }

    }

    public void ShakeAnim()
    {
            _tweenObject = LeanTween.moveX(objectToAnimate, objectToAnimate.transform.position.x + shakeDistance, shakeSpeed).setLoopCount(numShakes).setLoopPingPong();
            ResetObjectToOriginal();
    }

    public void WiggleAnim()
    {
        _tweenObject = LeanTween.rotateZ(objectToAnimate, objectToAnimate.transform.rotation.z + wiggleRotAngle, wiggleSpeed).setLoopCount(numWiggles).setLoopPingPong();
        ResetObjectToOriginal();
    }


    public void ResetObjectToOriginal()
    {
        objectToAnimate.transform.SetPositionAndRotation(new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
    }

}
