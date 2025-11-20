using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldInteractionManager : MonoBehaviour
{
    [SerializeField] private TrapInteractionManager Traps;
    [SerializeField] private TileSelectVisualisation Visualisation;
    // TODO: This should probably get it’s own class at some point
    [SerializeField] private Tilemap TerrainTilemap;

    public void EnableTrapPlacementVisualisation(TrapData Data)
    {
        Visualisation.EnableTrapPlacement(Data);
    }

    public void DisableTrapPlacementVisualisation()
    {
        Visualisation.DisableTrapPlacement();
    }

    public bool TryPlaceTrap(Vector3 PlayerWorldPos, Vector3 TrapWorldPos, TrapData Trap)
    {
        Vector2Int TrapPos = TerrainTilemap.WorldToCell(TrapWorldPos).ToVector2Int();
        Vector2Int PlayerPos = TerrainTilemap.WorldToCell(PlayerWorldPos).ToVector2Int();
        bool TileIsEmpty = TerrainIsPlaceable(TrapPos) && Traps.TileIsEmpty(TrapPos);
        bool TileIsReachable = Vector2.Distance(TrapPos, PlayerPos) < Trap.PlacementRangeTiles;
        bool ValidPosition = TileIsEmpty && TileIsReachable;
        if (ValidPosition)
            Traps.PlaceTrap(TrapPos, Trap);
        return ValidPosition;
    }

    private bool TerrainIsPlaceable(Vector2Int Tile)
    {
        // TODO: Some sort of check for placeable vs non-placable tiles
        return TerrainTilemap.HasTile(Tile.ToVector3Int());
    }
}
