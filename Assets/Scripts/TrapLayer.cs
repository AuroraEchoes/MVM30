using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TrapLayer : MonoBehaviour
{
    [SerializeField] private TileBase TrapTile;
    private Tilemap TileMap;
    // TODO: Make this a Dict with a trap type for dynamic behaviours
    private List<Vector2Int> TrapLocations;

    void Start()
    {
        TileMap = GetComponent<Tilemap>();
    }

    void Update()
    {

    }

    public void PlaceTrap(Vector2Int Tile)
    {
        TileMap.SetTile(Tile.ToVector3Int(), TrapTile);
    }

    public void PlaceTrap(Vector3 MouseScreenPosition)
    {
        Vector2 MousePos = MouseScreenPosition;
        Vector3 WorldPos = Camera.main.ScreenToWorldPoint(MousePos);
        Vector3Int HoveredTile = TileMap.WorldToCell(WorldPos);
        PlaceTrap(HoveredTile.ToVector2Int());
    }
}
