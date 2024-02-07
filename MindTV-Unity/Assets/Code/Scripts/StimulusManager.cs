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



    public TMP_Dropdown colorDropdown;
    public TMP_Dropdown animDropdown;
    private UITweener tweener;

    private GameObject activeTraining;
    
    private MIControllerBehavior controllerBehaviour;

    //Exposing this so that we can change the base size of the training object
    public float originalBaseSize = 100.0f;
    private float currentBaseSize;
    public float targetImageResolution = 512f;
    private Vector3 originalPosition;
    public Slider baseSizeSlider;
    // private bool isCurrentAnimationCountdown;
    // private bool isCurrentAnimationCountdownEnabled;

    // Start is called before the first frame update
    void Start()
    {
        trainingController = GameObject.FindGameObjectWithTag("TrainingPanel").GetComponent<TrainingMenuController>();
        originalPosition = _SPO.transform.position;

        currentBaseSize = originalBaseSize;
        _SPO.transform.localScale = new Vector3(currentBaseSize, currentBaseSize, currentBaseSize);
        baseSizeSlider.value = currentBaseSize;
    }

    // //called when user clicks train in ActiveTraining tab
    // //carried out simultaneously with marker generation
    // public void StartAnimation()
    // {
    //     StartCoroutine(StartCountDownAnimation());
    // }

    // //coroutine that shows 3 second countdown animation
    // //countDownText is changed, shown, and hidden for animation
    // IEnumerator StartCountDownAnimation()
    // {
    //     isCurrentAnimationCountdown = true;
    //     isCurrentAnimationCountdownEnabled = true;

    //     countDownText.text = "3";
    //     yield return CountdownUpdate(1.0f);
    //     //section below is used for pausing animation when user clicks stop
    //     if (!isCurrentAnimationCountdownEnabled) {
    //         countDownText.text = "";
    //         yield break;
    //     }

    //     countDownText.text = "2";
    //     yield return CountdownUpdate(1.0f);
    //     if (!isCurrentAnimationCountdownEnabled)
    //     {
    //         countDownText.text = "";
    //         yield break;
    //     }

    //     countDownText.text = "1";
    //     yield return CountdownUpdate(1.0f);
    //     if (!isCurrentAnimationCountdownEnabled)
    //     {
    //         countDownText.text = "";
    //         yield break;
    //     }

    //     //when countdown animation completes, training animation is activated with current training action
    //     countDownText.text = "";
    //     Debug.Log("Countdown completed!");
    //     StartActionAnimation(trainingController.currentAction);
    // }

    // //used to keep track of countdown elapsed time
    // IEnumerator CountdownUpdate(float duration)
    // {
    //     float elapsedTime = 0.0f;

    //     while (elapsedTime < duration)
    //     {
    //         elapsedTime += Time.deltaTime;
    //         yield return null;
    //     }
    // }

    //resets the position and scale of the traning object
    void ResetSPO()
    {
        _SPO.transform.position = originalPosition;
        // ResetBaseSize();
    }

    // //handles the training animation according to current training action
    // public void StartActionAnimation(string action)
    // {
    //     isCurrentAnimationCountdown = false;
    //     //animation time = training session duration (input by user in TrainingListOptions) - countdown time
    //     float animationTime = trainingController.currentTrainingSessionTime - 3;

    //     currentTween?.pause();

    //     //specific animation for each training action
    //     switch (action)
    //     {
    //         //performs the animation using LeanTween
    //         //on complete reset the training object to original position and scale
    //         case "Push":
    //             currentTween = LeanTween.scale(_SPO, new Vector3(25f, 25f, 25f), animationTime)
    //                 .setOnComplete(ResetSPO);
    //             break;
    //         case "Pull":
    //             currentTween = LeanTween.scale(_SPO, new Vector3(100f, 100f, 100f), animationTime)
    //                 .setOnComplete(ResetSPO);
    //             break;
    //         case "Lift":
    //             currentTween = LeanTween.moveY(_SPO, transform.position.y + 20.0f, animationTime)
    //                 .setOnComplete(ResetSPO);
    //             break;
    //         case "Drop":
    //             currentTween = LeanTween.moveY(_SPO, transform.position.y - 20.0f, animationTime)
    //                 .setOnComplete(ResetSPO);
    //             break;
    //         default:
    //             Debug.Log(action);
    //             break;
    //     }
    // }

    //stops the training animation when user clicks stop, resets training object
    // public void InterruptAnimation()
    // {
    //     if (isCurrentAnimationCountdown)
    //     {
    //         isCurrentAnimationCountdownEnabled = false;
    //     } else
    //     {
    //         currentTween?.pause();
    //         ResetSPO();
    //     }
    //     trainingController.InterruptTraining();
    // }

    //changes the training object image property
    public void SetTrainingObject(Sprite image_sprite)
    {
        ResetSPO();
        _SPO.GetComponent<SpriteRenderer>().sprite = image_sprite;

        // If we want the newly set image to be reset to the original size, use this: (uncomment the line below)
        ResetBaseSize();

        // If we want the newly set image to retain the same scaled size as the previous image, use this: (uncomment the line below)
        // ModifyBaseSizeWithSlider();
    }

    private float UniformImageSizeScaleFactor(SpriteRenderer spriteRenderer)
    // Calculate the scale factor needed to resize the longest dimension to targetImageResolution (512x512)
    {
        float width = spriteRenderer.sprite.texture.width;
        float height = spriteRenderer.sprite.texture.height;
        float maxDimension = Mathf.Max(width, height);
        float scaleFactor = targetImageResolution / maxDimension;
        return scaleFactor;
    }

    private void SetBaseSize(float size)
    // Sets the base size of the training object
    {
        SpriteRenderer spriteRenderer = _SPO.GetComponent<SpriteRenderer>();
        float uniformScaleFactor = UniformImageSizeScaleFactor(spriteRenderer);
        float scaledSize = size * uniformScaleFactor;
        _SPO.transform.localScale = new Vector3(scaledSize, scaledSize, scaledSize);
    }

    public void ModifyBaseSizeWithSlider()
    //changes the training object base size
    {
        currentBaseSize = baseSizeSlider.value;
        //Debug.Log("Base size changed to " + currentBaseSize);
        SetBaseSize(currentBaseSize);
    }

    public void ResetBaseSize()
    //resets the base size
    {
        currentBaseSize = originalBaseSize;
        baseSizeSlider.value = currentBaseSize;
        SetBaseSize(currentBaseSize);
        //Also going to reset the position of the training object
        ResetSPO();
    }
    public void ChangeBackgroundColor()
    {
        // //Emily's way
        // get the first transform game object in child
        activeTraining = transform.GetChild(0).gameObject;
        Image imageComponent = activeTraining.GetComponent<Image>();
        string colorText = colorDropdown.options[colorDropdown.value].text;
        Color color = ColorByName.colors[colorText];
        //Alternative way to get the color name without needing a static ref, but using a scriptable object. Could be good for persisting changes.
        //Color color = trainingPageSO.colors[colorText];
        imageComponent.color = color;
    }

    public void SetAnimationOnSelection()
    {
        string animText = animDropdown.options[animDropdown.value].text;
        tweener = _SPO.GetComponent<UITweener>();
        
       // string[] values = {};
        //values  = new string[7];
        //int n = 0;
        //foreach (var value in Enum.GetValues(typeof(UIAnimationTypes)))
       // {
        //    values[n] = value.ToString();
        //    n++;
        //}

        //foreach (string val in values)
        //{
        //    if (animText == val)
        tweener.SetTweenFromString(animText);
        //}
        Debug.Log("end of method in stim manager");
    }
}
