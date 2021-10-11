using System;
using System.Collections;
using UnityEngine;

public struct TimeUTC
{
    public int msec;
    public int sec;
    public int min;

    public TimeUTC(float seconds)
    {
        msec = (int)(seconds - (int)seconds) * 100;
        sec = (int)(seconds % 60);
        min = (int)(seconds / 60 % 60);
    }

    public int GetAllTimeInSeconds () => (min * 60) + sec + Mathf.RoundToInt(msec / 1000); 

    public string GetTimeUTCString(bool showMsec)
    {
        return showMsec ? string.Format("{0:00}:{1:00}:{2:00}", min, sec, msec) : string.Format("{0:00}:{1:00}", min, sec);
    }

    public static string GetTimeUTCStringFromTime(float seconds, bool showMsec)
    {
        int msec = (int)(seconds - (int)seconds) * 100;
        int sec = (int)(seconds % 60);
        int min = (int)(seconds / 60 % 60);

        return showMsec ? string.Format("{0:00}:{1:00}:{2:00}", min, sec, msec) : string.Format("{0:00}:{1:00}", min, sec);
    }
}

public class Timer
{
    public event Action<float> onTimerChanged;

    public TimeUTC timerTime;
    public float duration;
    public float time;

    public Coroutine timer;

    public void OnTimerChanged()
    {
        onTimerChanged?.Invoke(time);
    }
}

public static class TimerManager 
{
    public static void StartTimer(this MonoBehaviour monoBehaviour, Timer timer, TimeUTC time, float secDuration)
    {
        timer.timerTime = time;
        timer.duration = secDuration;

        timer.timer = monoBehaviour.StartCoroutine(TimerCoroutine(timer, time, secDuration, true));
    }

    public static void ResumeTimer(this MonoBehaviour monoBehaviour, Timer timer)
    {
        timer.timer = monoBehaviour.StartCoroutine(TimerCoroutine(timer, new TimeUTC(timer.time), timer.duration, false));
    }

    public static void StopTimer(this MonoBehaviour monoBehaviour, Timer timer)
    {
        monoBehaviour.StopCoroutine(timer.timer);
    }

    public static void RestartTimer(this MonoBehaviour monoBehaviour, Timer timer)
    {
        timer.time = 0;

        StopTimer(monoBehaviour, timer);

        timer.timer = monoBehaviour.StartCoroutine(TimerCoroutine(timer, timer.timerTime, timer.duration, true));
    }

    private static IEnumerator TimerCoroutine(Timer timer, TimeUTC timerTime, float secDuration, bool startNew)
    {
        if (startNew)
            timer.time = timerTime.GetAllTimeInSeconds();

        timer.time += secDuration;

        while (timer.time > 0)
        {
            yield return new WaitUntil(() => GameStatesManager.CheckCurrentState(GameStates.game));

            timer.time -= secDuration;

            timer.OnTimerChanged();

            yield return new WaitForSeconds(secDuration);
        }

        yield break;
    }
}
