using UnityEngine;

public class Matrix2D
{
    private Vector2 Origin;
    private Vector2 XAxis;
    private Vector2 YAxis;

    public Matrix2D(Vector2 XAxis, Vector2 YAxis)
    {
        this.Origin = Vector2.zero;
        this.XAxis = XAxis;
        this.YAxis = YAxis;
    }

    public Matrix2D(Vector2 Origin, Vector2 XAxis, Vector2 YAxis)
    {
        this.Origin = Origin;
        this.XAxis = XAxis;
        this.YAxis = YAxis;
    }

    public Vector2 Mult(Vector2 Other)
    {
        Vector2 Result = Origin + new Vector2(
            Other.x * XAxis.x + Other.y * YAxis.x,
            Other.x * XAxis.y + Other.y * YAxis.y
        );
        return Result;
    }
}
