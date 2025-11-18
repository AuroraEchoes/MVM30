using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldInteractionManager : MonoBehaviour
{
    [SerializeField] private TrapInteractionManager Traps;
    [SerializeField] private TileSelectVisualisation Visualisation;
    // TODO: This should probably get it’s own class at some point
    [SerializeField] private Tilemap TerrainTilemap;

    public void ToggleTrapPlacementMode(bool NewState)
    {
        Visualisation.ToggleTrapPlacement(NewState);
    }

    public bool TryPlaceTrap(Vector3 PlayerWorldPos, Vector3 TrapWorldPos, TrapData Trap)
    {
        Vector2Int TrapPos = TerrainTilemap.WorldToCell(TrapWorldPos).ToVector2Int();
        Vector2Int PlayerPos = TerrainTilemap.WorldToCell(PlayerWorldPos).ToVector2Int();
        bool TileIsEmpty = TerrainIsPlaceable(TrapPos) && Traps.TileIsEmpty(TrapPos);
        // TODO: Use TrapData to determine range (for now using const 3)
        bool TileIsReachable = Vector2.Distance(TrapPos, PlayerPos) < 3.0f;
        bool ValidPosition = TileIsEmpty && TileIsReachable;
        if (ValidPosition)
            Traps.PlaceTrap(TrapPos);
        return ValidPosition;
    }

    private bool TerrainIsPlaceable(Vector2Int Tile)
    {
        // TODO: Some sort of check for placeable vs non-placable tiles
        return TerrainTilemap.HasTile(Tile.ToVector3Int());
    }

}
