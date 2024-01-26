using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class MonoBehaviourExtensions 
{
    public static void RestartCoroutine(this MonoBehaviour mono, ref Coroutine coroutine, IEnumerator iEnumerator)
    {
        if (mono == null || (mono != null && mono.Equals(null))) return;
        if (!mono.gameObject.activeInHierarchy) return;
        if(coroutine != null) mono.StopCoroutine(coroutine);
        coroutine = mono.StartCoroutine(iEnumerator);
    }
    public static (List<Coroutine>, Coroutine) StartParallelCoroutine(this MonoBehaviour monoBehaviour, List<IEnumerator> enumerators, Action callBack = null)
    {
        if (monoBehaviour == null || (monoBehaviour != null && monoBehaviour.Equals(null))) return (null, null);
        if (!monoBehaviour.gameObject.activeInHierarchy) return (null, null);
        var outList = enumerators.Select(monoBehaviour.StartCoroutine).ToList();
        return (outList,  monoBehaviour.StartCoroutine(outList.WaitAll(callBack)));
    }

    public static void StopParallelCoroutine(this MonoBehaviour monoBehaviour, ref (List<Coroutine> childCoroutines, Coroutine parentCoroutine) coroutine)
    {
        var (childCoroutines, parentCoroutine) = coroutine;
        if (parentCoroutine != null)
        {
            monoBehaviour.StopCoroutine(parentCoroutine);
        }
        if(childCoroutines!=null)
        {
            foreach (var coroutineChildCoroutine in childCoroutines)
            {
                if (coroutineChildCoroutine != null)
                {
                    monoBehaviour.StopCoroutine(coroutineChildCoroutine);
                }
            }
        }
    }
    public static void Wait(this MonoBehaviour mono, ref Coroutine coroutine, float time, Action callback)
    {
        if (mono == null || (mono != null && mono.Equals(null))) return;
        if (!mono.gameObject.activeInHierarchy) return;
        if(coroutine != null) mono.StopCoroutine(coroutine);
        coroutine = mono.StartCoroutine(WaitCallback(time, callback));
    }
    public static void WaitOut(this MonoBehaviour mono, out Coroutine coroutine, float time, Action callback)
    {
        if ((mono == null || (mono != null && mono.Equals(null)))||(!mono.gameObject.activeInHierarchy))
        {
            coroutine = null;
            return;
        }

        coroutine = mono.StartCoroutine(WaitCallback(time, callback));
    }
    private static IEnumerator WaitCallback(float time, Action callback)
    {
        yield return new WaitForSeconds(time);
        callback?.Invoke();
    }
}