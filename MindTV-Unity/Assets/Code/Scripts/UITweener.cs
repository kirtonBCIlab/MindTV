using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public enum UIAnimationTypes
{
    Rotate,
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
    public float tweenXScale = 1.7f;
    public float tweenYScale = 1.7f;
    public float tweenZScale = 1.7f;
    public float tweenRotation = 360f;
    public bool tweenClockwiseRotation = true;
    public float bounceHeight = 100f;

    [SerializeField] private int numShakes = 8;
    [SerializeField] private float shakeSpeed = 0.05f;
    [SerializeField] private float shakeDistance = 3f;

    [SerializeField] private int numWiggles = 4;
    [SerializeField] private float wiggleSpeed = 0.1f;
    [SerializeField] private float wiggleRotAngle = 45f;

    private Transform _origTransform;

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

    private void Start()
    {
        if (objectToAnimate == null)
        {
            objectToAnimate = gameObject;
        }

        _origTransform = objectToAnimate.transform;
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
            case UIAnimationTypes.Grow:
                GrowAnim();
                break;
            case UIAnimationTypes.Rotate:
                RotateAnim();
                break;
            case UIAnimationTypes.Shake:
                ShakeAnim();
                break;
            case UIAnimationTypes.Bounce:
                BounceAnim();
                break;
            case UIAnimationTypes.Wiggle:
                WiggleAnim();
                break;
            
        }

    }

    // public void MoveAbsolute()
    // {
    //     objectToAnimate.GetComponent<RectTransform>().anchoredPosition = fromPosition;
    //     _tweenObject = LeanTween.move(objectToAnimate.GetComponent<RectTransform>(), toPosition, duration).setEase(easeType);
    // }
    public void BounceAnim()
    {
        _tweenObject = LeanTween.moveLocalY(objectToAnimate, objectToAnimate.transform.position.y + bounceHeight, duration).setEase(LeanTweenType.punch);
    }

    public void RotateAnim()
    {
        if (tweenClockwiseRotation)
            LeanTween.rotateAround(objectToAnimate, Vector3.forward, -tweenRotation, duration).setEase(easeType);
        else
            LeanTween.rotateAround(objectToAnimate, Vector3.forward, tweenRotation, duration).setEase(easeType);
    }

    public void GrowAnim()
    {
        if(pingPong)
        {
            _tweenObject = LeanTween.scale(objectToAnimate, new Vector3(objectToAnimate.transform.localScale.x * tweenXScale, objectToAnimate.transform.localScale.y * tweenYScale, objectToAnimate.transform.localScale.z * tweenYScale), duration).setLoopCount(1).setLoopPingPong();
        }
        else
        {
            _tweenObject = LeanTween.scale(objectToAnimate, new Vector3(objectToAnimate.transform.localScale.x * tweenXScale, objectToAnimate.transform.localScale.y * tweenYScale, objectToAnimate.transform.localScale.z * tweenYScale), duration).setEase(LeanTweenType.punch);
        }

    }

    public void ShakeAnim()
    {
            _tweenObject = LeanTween.moveX(objectToAnimate, objectToAnimate.transform.position.x + shakeDistance, shakeSpeed).setLoopCount(numShakes).setLoopPingPong().setOnComplete(ResetObjectToOriginal);
    }

    //There is a bug here that causes the object to only wiggle to one side or the other, can't figure out best way to handle this other than doing a punch animation
    public void WiggleAnim()
    {
        _tweenObject = LeanTween.rotateZ(objectToAnimate, objectToAnimate.transform.rotation.z + wiggleRotAngle, wiggleSpeed).setLoopCount(numWiggles).setLoopPingPong().setOnComplete(ResetObjectToOriginal);
    }


    //This isn't being called, and I'm not sure why. 
    public void ResetObjectToOriginal()
    {
        Debug.Log("Resetting object to original position");
        objectToAnimate.transform.SetPositionAndRotation(_origTransform.position, _origTransform.rotation);
    }

}
