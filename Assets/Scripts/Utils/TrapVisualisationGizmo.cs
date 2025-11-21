using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using System.Collections.Generic;

public class TrapVisualisationGizmo : MonoBehaviour
{
    [SerializeField] public bool Enabled = true;
    [SerializeField] public bool EnableTileTrapsVisualisation = true;
    [SerializeField] public bool EnableStoredTrapsVisualisation = true;
    [SerializeField] public bool EnableEnemyOccupiedTilesVisualisation = true;
    [SerializeField] public bool EnableMissedTileLineDrawing = true;
    [SerializeField] private List<Collider2D> TrackedEntities = new List<Collider2D>();
    [SerializeField] private TrapInteractionManager TrapManager;

    readonly Color TrapStoredColour = Color.green;
    readonly Color TrapPlacedColor = Color.yellow;
    readonly Color TrackedEntityColor = Color.red;
    readonly Color CollidingEntityColor = Color.magenta;

    private void OnDrawGizmos()
    {
        if (TrapManager is null || TrapManager.Tilemap == null || !Enabled) return;

        if (EnableStoredTrapsVisualisation)
        {
            Gizmos.color = TrapStoredColour;
            foreach (KeyValuePair<Vector2Int, TrapData> KVP in TrapManager.TrapLocations)
            {
                Vector3 WorldPosition = TrapManager.Tilemap.GetCellCenterWorld((Vector3Int)KVP.Key);
                Gizmos.DrawSphere(WorldPosition + new Vector3(-0.3f, 0.0f, -5.0f), 0.05f);
                Handles.Label(WorldPosition + new Vector3(0.0f, -0.1f, 0.0f), $"Loc {KVP.Key}\nID {KVP.Value.TrapIdentifier}");
            }
        }
        if (EnableTileTrapsVisualisation)
        {
            Gizmos.color = TrapPlacedColor;
            foreach (Vector3Int Cell in TrapManager.Tilemap.cellBounds.allPositionsWithin)
            {
                if (TrapManager.Tilemap.HasTile(Cell))
                {
                    Vector3 WorldPosition = TrapManager.Tilemap.GetCellCenterWorld(Cell);
                    Gizmos.DrawSphere(WorldPosition + new Vector3(-0.2f, 0.0f, -6.0f), 0.05f);
                    Handles.Label(WorldPosition, $"Cell {Cell}\nWorld {WorldPosition}");
                }
            }
        }

        Gizmos.color = CollidingEntityColor;
        if (EnableEnemyOccupiedTilesVisualisation)
        {
            Gizmos.color = CollidingEntityColor;
            foreach (Collider2D Collider in TrackedEntities)
            {
                if (Collider is not null)
                {
                    foreach (Vector2Int Intersection in TrapManager.ColliderTileIntersections(Collider))
                    {
                        Vector3 WorldPosition = TrapManager.Tilemap.GetCellCenterWorld(((Vector3Int)Intersection));
                        Gizmos.DrawSphere(WorldPosition, 0.1f);
                    }
                }
            }
        }

        if (EnableMissedTileLineDrawing)
        {
            foreach (Collider2D Collider in TrackedEntities)
            {
                if (Collider is not null)
                {
                    Vector2Int CellPos = (Vector2Int)TrapManager.Tilemap.WorldToCell(Collider.transform.position);
                    for (int X = CellPos.x - 2; X <= CellPos.x + 2; X++)
                    {
                        for (int Y = CellPos.y - 2; Y <= CellPos.y + 2; Y++)
                        {
                            Vector2Int Cell = new Vector2Int(X, Y);
                            Vector3 CellCenterWorld = TrapManager.Tilemap.layoutGrid.CellToWorld((Vector3Int)Cell);
                            Vector3 ClosestPoint = Collider.ClosestPoint(CellCenterWorld);
                            Vector2Int ClosestPointCell = (Vector2Int)TrapManager.Tilemap.layoutGrid.WorldToCell(ClosestPoint);
                            if (Cell == ClosestPointCell)
                            {
                                Gizmos.color = TrackedEntityColor;
                                Vector3 WorldPosition = TrapManager.Tilemap.GetCellCenterWorld(((Vector3Int)ClosestPointCell));
                                Gizmos.DrawSphere(WorldPosition + new Vector3(-0.1f, 0.0f, 0.0f), 0.1f);
                                Handles.Label(WorldPosition, $"Cell {Cell}");
                            }
                            else
                            {
                                Gizmos.color = Color.yellow;
                                Vector3 WorldPosition = TrapManager.Tilemap.GetCellCenterWorld(((Vector3Int)Cell));
                                Handles.Label(WorldPosition, $"{Cell} -> {ClosestPointCell}");
                                Gizmos.DrawLine(ClosestPoint, WorldPosition);
                                Gizmos.color = Color.green;
                                Gizmos.DrawLine(ClosestPoint, TrapManager.Tilemap.GetCellCenterWorld(((Vector3Int)ClosestPointCell)));
                            }
                        }
                    }
                }
            }
        }
    }
}
