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
}
