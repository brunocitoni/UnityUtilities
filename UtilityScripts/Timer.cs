/*
 * Timer.cs
 * 
 * A simple timer class with a built in callback when elapsed
 * 
 * Author: Bruno Citoni
 */

using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public Action TimerElapsed;

    public TMP_Text timerText;
    public float timeDuration;
    private float timer;
    private Coroutine countdownCoroutine;

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
        timer = 0;

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
        return timer;
    }

    public void OnTimerFinish()
    {
        TimerElapsed?.Invoke();
    }

    private void StartCountdownCoroutine()
    {
        countdownCoroutine = StartCoroutine(CountdownCoroutine());
    }

    private IEnumerator CountdownCoroutine()
    {
        while (timer > 0)
        {
            timer = Mathf.Max(timer - Time.deltaTime, 0);

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
        string formattedTime = string.Format("{0:D1}:{1:D2}", timeSpan.Minutes * 60 + timeSpan.Seconds, timeSpan.Milliseconds / 10);

        timerText.text = formattedTime;
    }
}
