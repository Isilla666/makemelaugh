using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static class ExtensionsBase
{
    public static IEnumerable<GameObject> FindClosureGameObjectsWithTag(this GameObject gameObject, string tag)
    {
        var goStack = new Stack<GameObject>();
        goStack.Push(gameObject);
        while (goStack.Count > 0)
        {
            var go = goStack.Pop();
            var childCount = go.transform.childCount;
            if (go.CompareTag(tag))
                yield return go;
            for (var i = 0; i < childCount; i++)
                goStack.Push(go.transform.GetChild(i).gameObject);
        }
    }
    public static Vector3 SetZ(this Vector3 vector3, float value)
    {
        vector3.z = value;
        return vector3;
    }
    public static T AddComponent<T>(this GameObject go, T toAdd) where T : Component
    {
        return go.AddComponent<T>().GetCopyOf(toAdd) as T;
    }
    public static T GetCopyOf<T>(this Component comp, T other) where T : Component
    {
        var type = comp.GetType();
        if (type != other.GetType()) return null;
        const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        var properties = type.GetProperties(flags);
        foreach (var propertyInfo in properties)
        {
            if (!propertyInfo.CanWrite) continue;
            try 
            {
                propertyInfo.SetValue(comp, propertyInfo.GetValue(other, null), null);
            }
            catch
            {
                // ignored
            }
        }
        var fields = type.GetFields(flags);
        foreach (var fieldInfo in fields) 
        {
            fieldInfo.SetValue(comp, fieldInfo.GetValue(other));
        }
        return comp as T;
    }

    public static void Then<T>(this T self, Action<T> notNull)
    {
        if (self != null) notNull?.Invoke(self);
    }

    public static T With<T>(this T self, Action<T> apply, Func<bool> when)
    {
        if (when()) apply?.Invoke(self);
        return self;
    }
    
    public static T With<T>(this T self, Action<T> apply)
    {
        apply?.Invoke(self);
        return self;
    }
    
    public static T With<T>(this T self, Action<T> apply,bool when)
    {
        if (when) apply?.Invoke(self);
        return self;
    }
    
    public static Transform CreateParent(this MonoBehaviour mono, Vector3 parentPosition)
    {
        var parent = new GameObject($"{mono.name} parent").transform;

        parent.SetParent(mono.transform.parent);
        parent.transform.position = parentPosition;
        parent.localScale = Vector3.one;
        mono.transform.SetParent(parent);

        return parent;
    }

    private static int CountCornersVisibleFrom(this RectTransform rectTransform, Camera camera)
    {
        var screenBounds = new Rect(0f, 0f, Screen.width, Screen.height);
        var objectCorners = new Vector3[4];
        rectTransform.GetWorldCorners(objectCorners);

        int visibleCorners = 0;
        for (var i = 0; i < objectCorners.Length; i++)
        {
            var tempScreenSpaceCorner = camera.WorldToScreenPoint(objectCorners[i]);
            if (screenBounds.Contains(tempScreenSpaceCorner))
                visibleCorners++;
        }

        return visibleCorners;
    }

    public static bool IsFullyVisibleFrom(this RectTransform rectTransform, Camera camera)
    {
        return CountCornersVisibleFrom(rectTransform, camera) == 4;
    }

    public static bool IsVisibleFrom(this RectTransform rectTransform, Camera camera)
    {
        return CountCornersVisibleFrom(rectTransform, camera) > 0;
    }
}
