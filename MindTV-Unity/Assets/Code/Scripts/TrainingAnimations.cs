using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingAnimations : MonoBehaviour
{
    // public float bounceDuration = 5f;
    // public float bounceXScale = 1.7f;
    // public float bounceYScale = 1.7f;
    // public float bounceZScale = 1.7f;

    public float tweenDuration = 5f;
    public float tweenXScale = 1.7f;
    public float tweenYScale = 1.7f;
    public float tweenZScale = 1.7f;

    public LeanTweenType easeType;

    private void Start()
    {
        TweenScaleAnimation();
    }
    // public void BounceAnimation()
    // {
    //     //Stop other action at the moment
    //     LeanTween.cancel(gameObject);

    //     //set the initial scale to 1
    //     // transform.localScale = Vector3.one;

    //     //Bounce

    //     //Bounce animation
    //     LeanTween.scale(gameObject, new Vector3(transform.localScale.x*bounceXScale, transform.localScale.y*bounceYScale, transform.localScale.z*bounceZScale), bounceDuration).setEase(LeanTweenType.easeOutBounce);
    // }

    public void TweenScaleAnimation()
    {
        //Stop other action at the moment
        LeanTween.cancel(gameObject);

        //Do some animation 
        LeanTween.scale(gameObject, new Vector3(transform.localScale.x*tweenXScale, transform.localScale.y*tweenYScale, transform.localScale.z*tweenZScale), tweenDuration).setEase(easeType);
    }

    public void TweenTranslateAnimation()
    {
        //Stop other action at the moment
        LeanTween.cancel(gameObject);

        //Run the animation
        LeanTween.moveX(gameObject, transform.position.x + 1f, tweenDuration).setEase(easeType);

    }

}
