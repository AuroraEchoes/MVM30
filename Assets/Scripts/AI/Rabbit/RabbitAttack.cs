
using System.Numerics;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;
public class RabbitAttack : AttackBase
{
    public float attackspeed = 400;
    public float MoveToSpotSpeed = 100;
    public int AttackRange = 10;
    public override void DoAttack(EnemyBase enemy, PlayerController PC)
    {
        Debug.Log("We be doing the rabbit attack");
        if (enemy is RabbitEnemy rabbit)
        {
            rabbit.isAttacking = true;
            rabbit.attackCoroutine = rabbit.StartCoroutine(MoveAttack(rabbit, PC));
        }
    }

private IEnumerator MoveAttack(RabbitEnemy rabbit, PlayerController PC)
    {
        rabbit.Waypoints.Clear();
        float value = 0;
        rabbit.SetSpeed(0);
        Vector3 endPosition = PC.gameObject.transform.position;
        Vector3 StartPosition = rabbit.EnemyFeetPos.position;
        foreach (Transform transforms in PC.gameObject.GetComponentsInChildren<Transform>())
        {
            if (transforms.gameObject.name.Contains("Feet"))
            {
                endPosition = transforms.position;
            }
        }
        Vector3Int StartTileCell = rabbit.tilemap.WorldToCell(StartPosition);
        Vector3Int EndTileCell = rabbit.tilemap.WorldToCell(endPosition);
        Vector3Int currentCell = StartTileCell;
        while(currentCell.x != EndTileCell.x && currentCell.y != EndTileCell.y)
        {
            for(int x = -1; x < 2; x++)
            {
                for(int y = -1; y < 2; y++)
                {
                    if(GetDistance(rabbit, currentCell + new Vector3Int(x, y, 0), EndTileCell) < GetDistance(rabbit, currentCell, EndTileCell))
                    {
                        currentCell = currentCell + new Vector3Int(x, y, 0);
                    }
                }
            }
            Vector3 worldPos = rabbit.tilemap.CellToWorld(new Vector3Int((int)currentCell.x, (int)currentCell.y,0));
            rabbit.Waypoints.Add(worldPos);
            Debug.Log("Bingle bongle dingle dangle");
        }
        float requiredTime = 0;
        if (rabbit.Waypoints.Count > 0)
        {
            requiredTime = Vector3.Distance(StartPosition, rabbit.Waypoints[0]) / (MoveToSpotSpeed * Time.fixedDeltaTime);
            for(int i = 0; i < rabbit.Waypoints.Count - 1; i++)
            {
                if(i < rabbit.Waypoints.Count - 1)
                {
                    requiredTime += Vector3.Distance(rabbit.Waypoints[i], rabbit.Waypoints[i + 1]) / (MoveToSpotSpeed * Time.fixedDeltaTime);
                }
            }
        }
        while (value < requiredTime)
        {
            value += Time.deltaTime;
            rabbit.SetSpeed(MoveToSpotSpeed);
            yield return null;
        }
        value = 0;
        rabbit.SetSpeed(0);
        rabbit.Waypoints.Clear();
        //Here we would play th animation for the big wind up and also display where the attack will be going
        yield return new WaitForSeconds(1.0f);
        List<Vector2> attackTiles = new List<Vector2>();
        endPosition = PC.gameObject.transform.position;
        EndTileCell = rabbit.tilemap.WorldToCell(endPosition);
        if (currentCell.x == EndTileCell.x)
        {
            if(EndTileCell.y > currentCell.y)
            {
                for (int i = 0; i < AttackRange; i++) attackTiles.Add(new Vector2(currentCell.x, currentCell.y + i));
                currentCell.y += AttackRange;
            }
            else
            {
                for (int i = 0; i < AttackRange; i++) attackTiles.Add(new Vector2(currentCell.x, currentCell.y - i));
                currentCell.y -= AttackRange;
            }
        }
        else
        {
            if(EndTileCell.x > currentCell.x)
            {
                for (int i = 0; i < AttackRange; i++) attackTiles.Add(new Vector2(currentCell.x + i, currentCell.y));
                currentCell.x += AttackRange;  
            }
            else
            {
                for (int i = 0; i < AttackRange; i++) attackTiles.Add(new Vector2(currentCell.x - i, currentCell.y));
                currentCell.x -= AttackRange;
            }
        }
        
        rabbit.Waypoints.Add(rabbit.tilemap.CellToWorld(currentCell));
        Debug.Log("Bingle bongle dingle dangle2");
        StartTileCell = rabbit.tilemap.WorldToCell(rabbit.EnemyFeetPos.position);
        requiredTime = Vector3.Distance(StartTileCell, rabbit.Waypoints[0]) / (attackspeed * Time.fixedDeltaTime);
        EnemyAttackVisualisation EAV = GameObject.FindObjectOfType<EnemyAttackVisualisation>();
        EAV.VisualiseAttack(this, attackTiles);
        while (value < requiredTime)
        {
            value += Time.deltaTime;
            rabbit.SetSpeed(attackspeed);
            yield return null;
        }
        Debug.Log("hopium3");

        rabbit.SetSpeed(rabbit.stats.speed);
        rabbit.DoCoolDownExternal();
        rabbit.isAttacking = false;
        EAV.DestroyTilemap(this);
        yield return null;
    }

    private float GetDistance(RabbitEnemy rabbit, Vector3Int cell1, Vector3Int cell2)
    {
        return Vector2.Distance(cell1.ToVector2Int(), cell2.ToVector2Int());
    }
}
