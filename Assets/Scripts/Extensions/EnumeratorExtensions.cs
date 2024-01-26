using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnumeratorExtensions
{
    public static IEnumerator AddCallback(this IEnumerator enumerator, Action callback)
    {
        yield return enumerator;
        callback?.Invoke();
    }
    public static IEnumerator AddLateCallback(this IEnumerator enumerator, Action callback, float waitSeconds)
    {
        yield return enumerator;
        yield return new WaitForSeconds(waitSeconds);
        callback?.Invoke();
    }
    public static IEnumerator Wait(this IEnumerator enumerator, float waitSeconds)
    {
        yield return new WaitForSeconds(waitSeconds);
        yield return enumerator;
    }
    public static IEnumerator Combine(this IEnumerator enumerator, IEnumerator addedEnumerator)
    {
        yield return enumerator;
        yield return addedEnumerator;
    }
    public static IEnumerator Combine(this List<IEnumerator> enumerators)
    {
        return enumerators.GetEnumerator();
    }
    public static IEnumerator WaitAll(this List<Coroutine> list, Action callBack = null)
    {
        foreach (var coroutine in list) yield return coroutine;
        callBack?.Invoke();
    }
}
