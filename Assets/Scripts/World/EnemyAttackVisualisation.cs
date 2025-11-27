using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;


public class EnemyAttackVisualisation : MonoBehaviour
{
    public Tilemap tilemapTemplate;
    public Dictionary<AttackBase, Tilemap> tilemapDictionary = new Dictionary<AttackBase, Tilemap>();
    [SerializeField] private TileBase ObstructedTile;


    public void VisualiseAttack(AttackBase attack, List<Vector2> attackTiles)
    {

        
        Tilemap tilemapInstance = Instantiate(tilemapTemplate, this.transform);

        tilemapInstance.ClearAllTiles();
        tilemapDictionary.Add(attack, tilemapInstance);
        foreach (Vector2 tilePos in attackTiles)
        {

            Vector3Int cellPos = new Vector3Int(Mathf.FloorToInt(tilePos.x), Mathf.FloorToInt(tilePos.y), 0);
            tilemapInstance.SetTile(cellPos, ObstructedTile);
        }
    }

    public void DestroyTilemap(AttackBase attack)
    {
        if(tilemapDictionary.ContainsKey(attack))
        {
            Tilemap tilemapInstance = tilemapDictionary[attack];
            Destroy(tilemapInstance.gameObject);
            tilemapDictionary.Remove(attack);
        }
    }
}
