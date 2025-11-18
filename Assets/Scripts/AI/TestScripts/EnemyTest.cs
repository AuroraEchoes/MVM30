
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class EnemyTest : EnemyBase
{
    public List<Vector3> Waypoints;
    public float currentSpeed = 1;
    public Coroutine attackCoroutine;
    public Tilemap tilemap;
    private Vector3 startPosition;

    public bool isAttacking = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosition = transform.position;
        tilemap = FindAnyObjectByType<Tilemap>();
        currentSpeed = stats.speed;
        GetRandomPos();
        //Attack();
       // Move();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public override void Move()
    {
        if (!isAttacking)
            StartCoroutine(MoveAlongPath());
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
    }

    public override void Attack()
    {   
        base.Attack();
    }

    private void GetRandomPos()
    {
        Waypoints = new List<Vector3>();
        float randomX = startPosition.x + Random.Range(-stats.patrolRadius, stats.patrolRadius);
        float randomY = startPosition.y + Random.Range(-stats.patrolRadius, stats.patrolRadius);
        Vector3Int randomPos = new Vector3Int((int)Mathf.Floor(randomX), (int)Mathf.Floor(randomY), 0);
        if(tilemap.cellBounds.Contains(randomPos))
        {
            Debug.Log("Position is within bounds");
            
        }
        Waypoints.Add(tilemap.GetCellCenterWorld(randomPos));
        Move();
    }

    public override void Death()
    {
        
    }

    private IEnumerator  MoveAlongPath()
    {
        while (Waypoints.Count > 0)
        {
            float value = 0;
            
            Vector3 StartPosition = transform.position;
            Vector3 nextposition = tilemap.GetCellCenterWorld(new Vector3Int((int)Mathf.Floor(Waypoints[0].x),
                (int)Mathf.Floor(Waypoints[0].y), (int)Mathf.Floor(Waypoints[0].z)));
            float requiredTime = Vector3.Distance(StartPosition, nextposition) / currentSpeed;
            
            while (value < 1)
            {
                value += Time.deltaTime;
                gameObject.transform.position = Vector3.Lerp(StartPosition, nextposition, value / requiredTime);
                yield return null;
            }
            Waypoints.RemoveAt(0);
            yield return null;
        }
        
        yield return new WaitForSeconds(Random.Range(0.5f, 5.0f));
        GetRandomPos();
    }
    
}
