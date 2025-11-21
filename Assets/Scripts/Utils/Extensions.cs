using UnityEngine;

public static class Extensions
{
    public static Vector2Int ToVector2Int(this Vector3Int Vector3)
    {
        return new Vector2Int(Vector3.x, Vector3.y);
    }

    public static Vector3Int ToVector3Int(this Vector2Int Vector2)
    {
        return new Vector3Int(Vector2.x, Vector2.y, 0);
    }

    public static Vector2 ToVector2(this Vector3 Vector3)
    {
        return new Vector2(Vector3.x, Vector3.y);
    }

    public static Vector3 ToVector3(this Vector2 Vector2)
    {
        return new Vector3(Vector2.x, Vector2.y, 0);
    }
}
