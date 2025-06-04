using UnityEngine;
using System;
using System.Collections;

public static class MonoBehaviourExtensions
{
    public static void Delay(this MonoBehaviour g, Action f, bool loop = false)
    {
        Delay(g, 1, f, loop);
    }

    public static void Delay(this MonoBehaviour g, int frames, Action f, bool loop = false)
    {
        g.StartCoroutine(DelayCoroutine(frames, f, loop));
    }

    public static void Delay(this MonoBehaviour g, float seconds, Action f, bool loop = false)
    {
        g.StartCoroutine(DelayCoroutine(seconds, f, loop));
    }

    public static IEnumerator DelayCoroutine(int frames, Action f, bool loop = false)
    {
        do
        {
            for (var n = 0; n < frames; ++n)
            {
                yield return null;
            }

            f();
        }
        while (loop);

    }

    public static IEnumerator DelayCoroutine(float seconds, Action f, bool loop = false)
    {
        do
        {
            yield return new WaitForSeconds(seconds);
            f();
        }
        while (loop);
    }
}