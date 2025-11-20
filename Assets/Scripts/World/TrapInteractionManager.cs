using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TrapInteractionManager : MonoBehaviour
{
    private Tilemap TileMap;
    // TODO: Make this a Dict with a trap type for dynamic behaviours
    private List<Vector2Int> TrapLocations = new List<Vector2Int>();

    void Start()
    {
        TileMap = GetComponent<Tilemap>();
    }

    public bool TileIsEmpty(Vector2Int Tile)
    {
        return !TrapLocations.Contains(Tile);
    }

    public void PlaceTrap(Vector2Int Tile, TrapData Data)
    {
        TileMap.SetTile(Tile.ToVector3Int(), Data.TrapTile);
    }

    private void TriggerTrap(Vector2Int Tile)
    {
        TileMap.DeleteCells(Tile.ToVector3Int(), 1, 1, 1);
    }

    void OnTriggerEnter2D(Collider2D Other)
    {
        Debug.Log("Trigger entered");
    }
}
