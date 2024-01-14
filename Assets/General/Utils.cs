using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.ResourceManagement.AsyncOperations;

public static class Utils
{
    public static string GetLocalizedString(string keyName, string tableName = "Default")
    {
        LocalizedString localizeString = new LocalizedString() { TableReference = tableName, TableEntryReference = keyName };
        var stringOperation = localizeString.GetLocalizedStringAsync();

        if (stringOperation.IsDone && stringOperation.Status == AsyncOperationStatus.Succeeded)
        {
            return stringOperation.Result;
        }
        else
        {
            return null;
        }
    }

    public static Coroutine StartCoroutine(IEnumerator enumerator)
    {
        return GameManager.Instance.StartCoroutine(enumerator);
    }

    public static Vector2Int Rotate(this Vector2Int v, Rotation r)
    {
        switch (r)
        {
            case Rotation.p0:
                return v;
            case Rotation.p90:
                return new Vector2Int(-v.y, v.x);
            case Rotation.p180:
                return -v;
            case Rotation.p270:
                return new Vector2Int(v.y, -v.x);
        }
        return Vector2Int.zero;
    }

    public static float ToFloat(this Rotation r)
    {
        switch (r)
        {
            case Rotation.p0:
                return 0;
            case Rotation.p90:
                return Mathf.PI * 0.5f;
            case Rotation.p180:
                return Mathf.PI;
            case Rotation.p270:
                return Mathf.PI * -0.5f;
        }
        return 0;
    }
}

public enum Rotation
{
    p0 = 0,
    p90 = 1,
    p180 = 2,
    p270 = 3,
    m90 = 3,
    m180 = 2,
    m270 = 1,
}

public enum EntityType
{
    P,
    E,
}
