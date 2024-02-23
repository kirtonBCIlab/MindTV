using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public enum UIAnimationTypes
{
    None,
    Shake,
    Bounce,
    Grow,
    Wiggle,
    Rotate,
    RotatePunch,
    Left,
    Right,
    Up,
    Down
}

public class UITweener : MonoBehaviour
{

    public GameObject objectToAnimate;

    public UIAnimationTypes animationTypes;

    public Transform targetLeft;
    public Transform targetRight;
    public Transform targetUp;
    public Transform targetDown;

    public float duration = 2f;
    public float tweenXScale = 1.7f;
    public float tweenYScale = 1.7f;
    public float tweenZScale = 1.7f;
    public float tweenRotation = 360f;
    public bool tweenClockwiseRotation = true;
    public float bounceHeight = 100f;
    public int numShakes = 8;
    public float shakeSpeed = 0.05f;
    public float shakeDistance = 3f;

    public int numWiggles = 4;
    public float wiggleSpeed = 0.1f;
    public float wiggleRotAngle = 45f;

    private Vector3 _origPosition;
    private Quaternion _origRotation;

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

        _origPosition = objectToAnimate.transform.position;
        _origRotation = objectToAnimate.transform.rotation;
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
            case UIAnimationTypes.None:
                Debug.Log("none selected");
                break;
            case UIAnimationTypes.Grow:
                GrowAnim();
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
            case UIAnimationTypes.Rotate:
                RotateAnim();
                break;
            case UIAnimationTypes.RotatePunch:
                RotatePunchAnim();
                break;
            case UIAnimationTypes.Left:
                MoveToTarget(targetLeft);
                break;
            case UIAnimationTypes.Right:
                MoveToTarget(targetRight);
                break;
            case UIAnimationTypes.Up:
                MoveToTarget(targetUp);
                break;
            case UIAnimationTypes.Down:
                MoveToTarget(targetDown);
                break;
        }
        Debug.Log("end of handle");
    }

    // public void MoveAbsolute()
    // {
    //     objectToAnimate.GetComponent<RectTransform>().anchoredPosition = fromPosition;
    //     _tweenObject = LeanTween.move(objectToAnimate.GetComponent<RectTransform>(), toPosition, duration).setEase(easeType);
    // }
    public void BounceAnim()
    {
        _tweenObject = LeanTween.moveLocalY(objectToAnimate, objectToAnimate.transform.position.y - bounceHeight, duration).setEase(LeanTweenType.punch);
    }

    public void RotateAnim()
    {
        if (tweenClockwiseRotation)
            LeanTween.rotateAround(objectToAnimate, Vector3.forward, -tweenRotation, duration);
        else
            LeanTween.rotateAround(objectToAnimate, Vector3.forward, tweenRotation, duration);
    }

    public void RotatePunchAnim()
    {
        if (tweenClockwiseRotation)
            LeanTween.rotateAround(objectToAnimate, Vector3.forward, -tweenRotation, duration).setEase(LeanTweenType.punch);
        else
            LeanTween.rotateAround(objectToAnimate, Vector3.forward, tweenRotation, duration).setEase(LeanTweenType.punch);
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

    public void MoveToTarget(Transform target)
    {
        LeanTween.move(objectToAnimate, target.position, duration).setOnComplete(ResetObjectToOriginal);
    }


    //This isn't being called, and I'm not sure why. 
    public void ResetObjectToOriginal()
    {
        Debug.Log("Resetting object to original position" + _origPosition + " " + _origRotation);
        LeanTween.move(objectToAnimate, _origPosition, 0.25f);
        LeanTween.rotate(objectToAnimate, _origRotation.eulerAngles, 0.25f);
        
        //objectToAnimate.transform.SetPositionAndRotation(_origPosition, _origRotation);
    }

    public string GetTweenAnimation()
    {
        return animationTypes.ToString();
    }

    public void StopTween()
    {
        _tweenObject?.pause();
    }

    public void SetTweenFromString(string selected)
    {
        if (selected == "Shake")
            animationTypes = UIAnimationTypes.Shake;
        else if (selected == "Grow")
            animationTypes = UIAnimationTypes.Grow;
        else if (selected == "Bounce")
            animationTypes = UIAnimationTypes.Bounce;
        else if (selected == "Wiggle")
            animationTypes = UIAnimationTypes.Wiggle;
        else if (selected == "Rotate")
            animationTypes = UIAnimationTypes.Rotate;
        else if (selected == "RotatePunch")
            animationTypes = UIAnimationTypes.RotatePunch;
        else if (selected == "Left")
            animationTypes = UIAnimationTypes.Left;
        else if (selected == "Right")
            animationTypes = UIAnimationTypes.Right;
        else if (selected == "Up")
            animationTypes = UIAnimationTypes.Up;
        else if (selected == "Down")
            animationTypes = UIAnimationTypes.Down;
        else if (selected == "None")
            animationTypes = UIAnimationTypes.None;
    }


}
