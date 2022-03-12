using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Extenders
{
    #region Transforms

    //Breadth-first search
    public static Transform FindDeepChild(this Transform aParent, string aName)
    {
        if (aParent != null)
        {
            var result = aParent.Find(aName);
            if (result != null)
                return result;

            foreach (Transform child in aParent)
            {
                result = child.FindDeepChild(aName);
                if (result != null)
                    return result;
            }
        }

        return null;
    }

    public static T FindDeepChild<T>(this Transform aParent, string aName)
    {
        T result = default(T);

        var transform = aParent.FindDeepChild(aName);

        if (transform != null)
        {
            result = (typeof(T) == typeof(GameObject)) ? (T)Convert.ChangeType(transform.gameObject, typeof(T)) : transform.GetComponent<T>();
        }

        if (result == null)
        {
            Debug.LogError($"FindDeepChild didn't find: '{aName}' on GameObject: '{aParent.name}'");
        }

        return result;
    }

    #endregion

    #region Collections

    public static T GetLoop<T>(this IEnumerable<T> i_Array, int i_Index)
    {
        int len = i_Array.Count();
        if (i_Index >= 0)
        {
            return i_Array.ElementAt(i_Index % i_Array.Count());
        }
        else
        {
            i_Index = -i_Index;
            i_Index %= len;
            i_Index = len - i_Index;
            return i_Array.ElementAt(i_Index % len);
        }
    }

    #endregion

    #region Vector3s

    public static bool IsGreaterOrEqual(this Vector3 local, Vector3 other)
    {
        if (local.x >= other.x && local.y >= other.y && local.z >= other.z)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool IsLesserOrEqual(this Vector3 local, Vector3 other)
    {
        if (local.x <= other.x && local.y <= other.y && local.z <= other.z)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion
}
