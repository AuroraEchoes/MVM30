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

    [Header("Properties")]
    [SerializeField] private int TrapPlacementRadiusTiles = 3;

    private bool VisualisationEnabled;

    private Tilemap Tilemap;

    void Start()
    {
        Tilemap = GetComponent<Tilemap>();
        Events.Gameplay.ToggleTrapPlacementEvent += ToggleTrapPlacement;
    }

    void Update()
    {
        // TODO: Changing every tile every frame is not performant. Ideally cache and only change tiles which need to be changed
        if (VisualisationEnabled)
        {
            HighlightArea(PlayerFeetTransform.position);
        }
    }

    private void ToggleTrapPlacement(bool Enabled)
    {
        VisualisationEnabled = Enabled;
        if (!Enabled)
            ClearArea();
    }

    private void HighlightArea(Vector3 WorldPosition)
    {
        Vector3Int Center = Tilemap.WorldToCell(WorldPosition);

        Vector2 MousePos = Input.mousePosition;
        Vector3 WorldPos = Camera.main.ScreenToWorldPoint(MousePos);
        Vector3Int HoveredTile = Tilemap.WorldToCell(WorldPos);

        Tilemap.ClearAllTiles();
        foreach (Vector3Int CellPos in BaseTilemap.cellBounds.allPositionsWithin)
        {
            bool IsHovered = HoveredTile.x == CellPos.x && HoveredTile.y == CellPos.y;
            if (Vector3.Distance(Center, CellPos) < TrapPlacementRadiusTiles)
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
