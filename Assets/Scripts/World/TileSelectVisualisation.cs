using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSelectVisualisation : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Tilemap BaseTilemap;
    [SerializeField] private Transform PlayerFeetTransform;
    [SerializeField] private TileBase ValidTile;
    [SerializeField] private TileBase ValidHoveredTile;
    [SerializeField] private TileBase ObstructedTile;
    [SerializeField] private TileBase OutOfRangeTile;

    private bool VisualisationEnabled;
    private TrapData VisualisedTrap;
    private Tilemap Tilemap;

    void Start()
    {
        Tilemap = GetComponent<Tilemap>();
    }

    void Update()
    {
        // TODO: Changing every tile every frame is not performant. Ideally cache and only change tiles which need to be changed
        if (VisualisationEnabled)
        {
            HighlightArea(PlayerFeetTransform.position);
        }
    }

    public void EnableTrapPlacement(TrapData Trap)
    {
        VisualisationEnabled = true;
        VisualisedTrap = Trap;
    }

    public void DisableTrapPlacement()
    {
        VisualisationEnabled = false;
        ClearArea();
    }

    private void HighlightArea(Vector3 WorldPosition)
    {
        if (VisualisedTrap is null)
        {
            Debug.LogWarning("Tried to visualise trap, but trap is null");
            return;
        }

        Vector3Int Center = Tilemap.WorldToCell(WorldPosition);

        Vector2 MousePos = Input.mousePosition;
        Vector3 WorldPos = Camera.main.ScreenToWorldPoint(MousePos);
        Vector3Int HoveredTile = Tilemap.WorldToCell(WorldPos);

        Tilemap.ClearAllTiles();
        foreach (Vector3Int CellPos in BaseTilemap.cellBounds.allPositionsWithin)
        {
            bool IsHovered = HoveredTile.x == CellPos.x && HoveredTile.y == CellPos.y;
            if (Vector2.Distance(CellPos.ToVector2Int(), Center.ToVector2Int()) < VisualisedTrap.PlacementRangeTiles)
            {
                if (IsHovered)
                    Tilemap.SetTile(CellPos, ValidHoveredTile);
                else
                    Tilemap.SetTile(CellPos, ValidTile);
            }
            else
            {
                if (IsHovered)
                    Tilemap.SetTile(CellPos, ObstructedTile);
                else
                    Tilemap.SetTile(CellPos, OutOfRangeTile);
            }
        }
    }

    public void ClearArea()
    {
        Tilemap.ClearAllTiles();
    }
}
