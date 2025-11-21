using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TrapInteractionManager : MonoBehaviour
{
    public Tilemap Tilemap { get; private set; }
    // TODO: Make this a Dict with a trap type for dynamic behaviours
    public Dictionary<Vector2Int, TrapData> TrapLocations { get; private set; } = new Dictionary<Vector2Int, TrapData>();

    void Start()
    {
        Tilemap = GetComponent<Tilemap>();
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
        }
    }

    public bool TileIsEmpty(Vector2Int Tile)
    {
        return !TrapLocations.ContainsKey(Tile);
    }

    public void PlaceTrap(Vector2Int Tile, TrapData Data)
    {
        TrapLocations.Add(Tile, Data);
        Tilemap.SetTile(((Vector3Int)Tile), Data.TrapTile);
    }

    private void TriggerTrap(Vector2Int Tile, GameObject Other)
    {
        if (TrapLocations.ContainsKey(Tile))
        {
            TrapData Trap = TrapLocations[Tile];
            Debug.Log("Activating trap");
            Trap.TrapImplementation.Activate(Trap, Other);
            Tilemap.DeleteCells(((Vector3Int)Tile), 0, 0, 0);
        }
    }

    void OnTriggerEnter2D(Collider2D Other)
    {
        foreach (Vector2Int TileIntersection in ColliderTileIntersections(Other))
        {
            TriggerTrap(TileIntersection, Other.gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D Other)
    {
        foreach (Vector2Int TileIntersection in ColliderTileIntersections(Other))
        {
            TriggerTrap(TileIntersection, Other.gameObject);
        }
    }

    public List<Vector2Int> ColliderTileIntersections(Collider2D Collider)
    {
        List<Vector2Int> Cells = new List<Vector2Int>();

        foreach (Vector3Int Cell in Tilemap.cellBounds.allPositionsWithin)
        {
            if (Tilemap.HasTile(Cell))
            {
                Vector3 CellCenterWorld = Tilemap.layoutGrid.CellToWorld(Cell);
                Vector3 ClosestPoint = Collider.ClosestPoint(CellCenterWorld);
                Vector3Int ClosestPointCell = Tilemap.layoutGrid.WorldToCell(ClosestPoint);
                if (Cell == ClosestPointCell && !Cells.Contains((Vector2Int)Cell))
                {
                    Cells.Add(((Vector2Int)Cell));
                }
            }
        }
        return Cells;
    }
}
