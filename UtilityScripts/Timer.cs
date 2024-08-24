/*
 * Timer.cs
 * 
 * A simple timer class with a built in callback when elapsed
 * 
 * Author: Bruno Citoni
 */

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public Action TimerElapsed;
    public Action<int> TimerCountdown;

    public TMP_Text timerText;
    public float timeDuration;
    private float timer, timerLeftOnStop;
    private Coroutine countdownCoroutine;

    public bool isCountDownTimer;
    public int topCountdown;

    public enum TimerFormatting
    {
        MILLI,
        SECONDS,
        MINUTES
    }
    public TimerFormatting formatting;

    public void SetDuration(float duration)
    {
        // can only set duration if timer is not running
        if (countdownCoroutine == null)
        {
            timeDuration = duration;
        }
    }

    public void ResumeTimer()
    {
        if (countdownCoroutine == null)
        {
            StartCountdownCoroutine();
        }
    }

    public void RestartTimer()
    {
        timer = timeDuration;
        if (countdownCoroutine == null)
        {
            StartCountdownCoroutine();
        }
    }

    public void StopTimer(bool invokeCallback)
    {
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
            countdownCoroutine = null;
        }
        timerLeftOnStop = timer;
        timer = 0;
        if (timerText != null) timerText.text = "";

        // should OnTimerFinish be called if timer is stopped?
        // matters when stopping it manually vs ending within countdown coroutine
        if (invokeCallback)
        {
            OnTimerFinish();
        }
    }

    public void PauseTimer()
    {
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
            countdownCoroutine = null;
        }
    }

    public float GetTimeLeft()
    {
        if (timer > 0)
        {
            return timer;
        }
        else
        {
            return timerLeftOnStop;
        }
    }

    public void OnTimerFinish()
    {
        TimerElapsed?.Invoke();
    }

    private void StartCountdownCoroutine()
    {
        List<int> countdownIntsToNotifyAbout = new List<int>();
        for (int i = topCountdown; i > 0; i--)
        {
            countdownIntsToNotifyAbout.Add(i);
        }

        countdownCoroutine = StartCoroutine(CountdownCoroutine(countdownIntsToNotifyAbout));
    }

    private IEnumerator CountdownCoroutine(List<int> countdownIntsToNotifyAbout)
    {
        while (timer > 0)
        {
            timer = Mathf.Max(timer - Time.deltaTime, 0);

            // todo
            // if timer is equal to any integer second below a certain variable value called "topCountdown" then notify via an Action with the integer that the timer is at currently. 
            // for example if topCountdown is set to 3, I want an event to be fired when timer = 3, timer =2 timer =1, timer = 0.
            // Check if timer is equal to any integer second below topCountdown
            if (countdownIntsToNotifyAbout.Count > 0)
            {
                if (timer <= countdownIntsToNotifyAbout[0])
                {
                    // Notify via an Action with the integer value of the current timer
                    TimerCountdown?.Invoke(countdownIntsToNotifyAbout[0]);

                    // remove this element from list to notify about
                    countdownIntsToNotifyAbout.RemoveAt(0);
                }
            }

            if (timerText != null)
            {
                FormatAndDisplayTimer();
            }
            yield return null;
        }

        StopTimer(true);
    }

    private void FormatAndDisplayTimer()
    {
        var timeSpan = TimeSpan.FromSeconds(timer);
        string formattedTime = "";

        switch (formatting)
        {
            case TimerFormatting.MINUTES:
                formattedTime = string.Format("{0:D1}:{1:D2}", timeSpan.Minutes * 60 + timeSpan.Seconds, timeSpan.Milliseconds / 10);
                break;
            case TimerFormatting.SECONDS:
                formattedTime = string.Format("{0:D1}", timeSpan.Seconds);
                break;
            case TimerFormatting.MILLI:
                formattedTime = string.Format("{0:D1}:{1:D2}", timeSpan.Seconds, timeSpan.Milliseconds / 10);
                break;
            default:
                break;
        }
        if (formattedTime == "0" && isCountDownTimer)
        {
            formattedTime = "GO!";
        }

        timerText.text = formattedTime;
    }
}