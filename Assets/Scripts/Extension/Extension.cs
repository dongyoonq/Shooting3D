using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extension
{
    public static bool IsValid(this GameObject gameObject)
    {
        return gameObject != null && gameObject.activeInHierarchy;
    }

    public static bool IsValid(this Component component)
    {
        return component != null && component.gameObject.activeInHierarchy;
    }
}
