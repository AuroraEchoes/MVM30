using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "TrapData", menuName = "Scriptable Objects/TrapData")]
public class TrapData : ScriptableObject
{
    public string TrapIdentifier;
    public float CooldownSeconds;
    public TileBase TrapTile;
    public int PlacementRangeTiles;
    public TrapBase TrapImplementation;
}
