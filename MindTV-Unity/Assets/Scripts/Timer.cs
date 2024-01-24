using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] TMP_Text timeText;
    [SerializeField] Button acceptButton;
    [SerializeField] Button rejectButton;
    [SerializeField] Button cancelButton;

    private float time = 8;
    private float timeRemaining = 8;
    private bool timerIsRunning = false;


    // Update is called once per frame
    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;

                acceptButton.gameObject.SetActive(true);
                rejectButton.gameObject.SetActive(true);
                gameObject.SetActive(false);
                cancelButton.gameObject.SetActive(false);
            }
        }
    }

    //set the time to display for the training session
    private void DisplayTime (float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);    
    }

    public void SetTime(float time)
    {
        this.time = time;
        timeRemaining = time;
    }

    public void ResetTimer()
    {
        timeRemaining = time;
    }

    public void StartTimer()
    {
        timerIsRunning = true;
        ResetTimer();
    }

    public void StopTimer()
    {
        timerIsRunning = false;
    }
}
