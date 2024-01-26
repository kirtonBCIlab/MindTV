using BCIEssentials.ControllerBehaviors;
using BCIEssentials.StimulusObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StimulusManager : MonoBehaviour
{
    //TO DO: provide training feedback (classification accuracy) for then end of each training session

    [SerializeField] private TrainingMenuController trainingController;
    [SerializeField] private TMP_Text countDownText;
    [SerializeField] private GameObject _SPO;

    private MIControllerBehavior controllerBehaviour;
    private LTDescr currentTween;
    private float originalScale = 50.0f;
    private Vector3 originalPosition;
    private bool isCurrentAnimationCountdown;
    private bool isCurrentAnimationCountdownEnabled;

    // Start is called before the first frame update
    void Start()
    {
        trainingController = transform.Find("TrainingMenu").GetComponent<TrainingMenuController>();
        originalPosition = _SPO.transform.position;
    }

    //called when user clicks train in ActiveTraining tab
    //carried out simultaneously with marker generation
    public void StartAnimation()
    {
        StartCoroutine(StartCountDownAnimation());
    }

    //coroutine that shows 3 second countdown animation
    //countDownText is changed, shown, and hidden for animation
    IEnumerator StartCountDownAnimation()
    {
        isCurrentAnimationCountdown = true;
        isCurrentAnimationCountdownEnabled = true;

        countDownText.text = "3";
        yield return CountdownUpdate(1.0f);
        //section below is used for pausing animation when user clicks stop
        if (!isCurrentAnimationCountdownEnabled) {
            countDownText.text = "";
            yield break;
        }

        countDownText.text = "2";
        yield return CountdownUpdate(1.0f);
        if (!isCurrentAnimationCountdownEnabled)
        {
            countDownText.text = "";
            yield break;
        }

        countDownText.text = "1";
        yield return CountdownUpdate(1.0f);
        if (!isCurrentAnimationCountdownEnabled)
        {
            countDownText.text = "";
            yield break;
        }

        //when countdown animation completes, training animation is activated with current training action
        countDownText.text = "";
        Debug.Log("Countdown completed!");
        StartActionAnimation(trainingController.currentAction);
    }

    //used to keep track of countdown elapsed time
    IEnumerator CountdownUpdate(float duration)
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    //resets the position and scale of the traning object
    void ResetSPO()
    {
        _SPO.transform.position = originalPosition;
        _SPO.transform.localScale = new Vector3(originalScale, originalScale, originalScale);
    }

    //handles the training animation according to current training action
    public void StartActionAnimation(string action)
    {
        isCurrentAnimationCountdown = false;
        //animation time = training session duration (input by user in TrainingListOptions) - countdown time
        float animationTime = trainingController.currentTrainingSessionTime - 3;

        currentTween?.pause();

        //specific animation for each training action
        switch (action)
        {
            //performs the animation using LeanTween
            //on complete reset the training object to original position and scale
            case "Push":
                currentTween = LeanTween.scale(_SPO, new Vector3(25f, 25f, 25f), animationTime)
                    .setOnComplete(ResetSPO);
                break;
            case "Pull":
                currentTween = LeanTween.scale(_SPO, new Vector3(100f, 100f, 100f), animationTime)
                    .setOnComplete(ResetSPO);
                break;
            case "Lift":
                currentTween = LeanTween.moveY(_SPO, transform.position.y + 20.0f, animationTime)
                    .setOnComplete(ResetSPO);
                break;
            case "Drop":
                currentTween = LeanTween.moveY(_SPO, transform.position.y - 20.0f, animationTime)
                    .setOnComplete(ResetSPO);
                break;
            default:
                Debug.Log(action);
                break;
        }
    }

    //stops the training animation when user clicks stop, resets training object
    public void InterruptAnimation()
    {
        if (isCurrentAnimationCountdown)
        {
            isCurrentAnimationCountdownEnabled = false;
        } else
        {
            currentTween?.pause();
            ResetSPO();
        }
        trainingController.InterruptTraining();
    }

    //changes the training object image property
    public void SetTrainingObject(Sprite sprite)
    {
        _SPO.GetComponent<SpriteRenderer>().sprite = sprite;
    }
}
