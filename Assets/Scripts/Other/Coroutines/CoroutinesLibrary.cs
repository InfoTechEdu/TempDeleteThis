using System.Collections;
using UnityEngine;

public static class CoroutinesLibrary
{
    public delegate void CallMethodDelegate();

    public static Coroutine CallMethodOnTimer(this MonoBehaviour monoBehaviour, CallMethodDelegate method, float seconds)
    {
        return monoBehaviour.StartCoroutine(CallMethodOnTimerCoroutine(method, seconds));
    }

    public static IEnumerator CallMethodOnTimerCoroutine(CallMethodDelegate method, float seconds)
    {
        yield return new WaitForSeconds(seconds);

        method?.Invoke();
    }
}
