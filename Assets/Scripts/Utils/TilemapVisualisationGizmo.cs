using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

public class TilemapVisualisationGizmo : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] public bool Enabled;

    private Tilemap Tilemap;
    private void Reset()
    {
        Tilemap = GetComponent<Tilemap>();
    }

    private void OnDrawGizmos()
    {
        if (Tilemap is null || !Enabled) return;

        Gizmos.color = Color.blue;
        foreach (Vector3Int Cell in Tilemap.cellBounds.allPositionsWithin)
        {
            Vector3 WorldPosition = Tilemap.CellToWorld(Cell);
            Gizmos.DrawSphere(WorldPosition + Vector3.up * 0.25f, 0.1f);
            Handles.color = Color.blue;
            Handles.Label(WorldPosition, $"Cell {Cell}\nWorld {WorldPosition}");
        }
    }
#endif
}
