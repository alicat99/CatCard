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
        var text = localizeString.GetLocalizedString();
        return text;
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
        return r switch
        {
            Rotation.p0 => 0,
            Rotation.p90 => Mathf.PI * 0.5f,
            Rotation.p180 => Mathf.PI,
            Rotation.p270 => Mathf.PI * -0.5f,
            _ => 0,
        };
    }

    public static Rotation Inverse(this Rotation r)
    {
        return r switch
        {
            Rotation.p0 => Rotation.p0,
            Rotation.p90 => Rotation.m90,
            Rotation.p180 => Rotation.m180,
            Rotation.p270 => Rotation.m270,
            _ => Rotation.p0,
        };
    }

    public static EntityType Inverse(this EntityType type)
    {
        return type == EntityType.P ? EntityType.E : EntityType.P;
    }

    public static Color SetA(this Color color, float value)
    {
        color.a = value;
        return color;
    }

    public static Color SetR(this Color color, float value)
    {
        color.r = value;
        return color;
    }

    public static Color SetG(this Color color, float value)
    {
        color.g = value;
        return color;
    }

    public static Color SetB(this Color color, float value)
    {
        color.b = value;
        return color;
    }

    public static Vector3 SetX(this Vector3 vector, float value)
    {
        vector.x = value;
        return vector;
    }

    public static Vector3 SetY(this Vector3 vector, float value)
    {
        vector.y = value;
        return vector;
    }

    public static Vector3 SetZ(this Vector3 vector, float value)
    {
        vector.z = value;
        return vector;
    }

    public static Vector2 SetX(this Vector2 vector, float value)
    {
        vector.x = value;
        return vector;
    }

    public static Vector2 SetY(this Vector2 vector, float value)
    {
        vector.y = value;
        return vector;
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
