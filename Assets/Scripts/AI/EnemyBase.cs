using System;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.Tilemaps;
using System.Collections;
using System.Collections.Generic;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] private string name;
    [SerializeField] public EnemyStats stats;
    [SerializeField] protected AttackBase[] attacks;
    [SerializeField] public Transform EnemyFeetPos;
    private float currentHealth;
    private bool CanBeDamaged = true;
    private bool detectsPlayer = false;
    public bool isAttacking = false;
    public bool isCooldown = false;
    public List<Vector3> Waypoints;
    protected Tilemap tilemap;
    protected Rigidbody2D rb;
    protected Vector2 startPos;
    protected float currentSpeed;

    void Start()
    {
        currentHealth = stats.health;
        rb = GetComponent<Rigidbody2D>();
        startPos = transform.position;
        if (tilemap == null)
        {
            foreach(Tilemap map in FindObjectsByType<Tilemap>(FindObjectsSortMode.None))
            {
                if (map.name == "Ground")
                {
                    tilemap = map;
                }
            }
        }
        currentSpeed = stats.speed;
    }

    void FixedUpdate()
    {
        if (!isCooldown && !isAttacking)
        {
            Vector2 direction = new Vector2(0, 0);
            if (Waypoints.Count > 0)
            {
                direction = (Waypoints[0] - EnemyFeetPos.position).normalized;
                if (Vector3.Distance(EnemyFeetPos.position, Waypoints[0]) < 0.5f)
                {
                    Waypoints.RemoveAt(0);
                    currentSpeed = stats.speed;
                    isCooldown = true;
                    Invoke("CoolDownEnd", Random.Range(0.5f, 1.0f));
                }
            }
            else
            {
                Move();
            }
            rb.linearVelocity = direction * currentSpeed * Time.fixedDeltaTime;
        }
    }

    public virtual void Move()
    {
        if (!isAttacking && !isCooldown)
        {
            GetRandomPos();
        }
    }
    public void CoolDownEnd()
    {
        isCooldown = false;
    }

    public virtual void TakeDamage(float damage)
    {
        if(CanBeDamaged)
            currentHealth -= damage;
    }

    public virtual void Attack()
    {
        if (attacks.Length > 0)
        {
            int chosenAttack = UnityEngine.Random.Range(0, attacks.Length);
            attacks[chosenAttack].DoAttack(this);
        }
    }
    private void GetRandomPos()
    {
        if (tilemap == null) return;
        Waypoints = new List<Vector3>();
        Vector3Int randomPos = new Vector3Int(100000000,100000000,100000000);
        while (!tilemap.cellBounds.Contains(randomPos))
        {
            randomPos = GenerateWaypoint();
            if (tilemap.cellBounds.Contains(randomPos))
            {
                if (tilemap.GetTile(randomPos) != null)
                {
                    if (tilemap.GetTile(randomPos).name.Contains("wall"))
                    {
                        randomPos.x = 1000000000;
                    }
                }
            }
        }
        Waypoints.Add(tilemap.GetCellCenterWorld(randomPos));
    }

    private Vector3Int GenerateWaypoint()
    {
        float randomX = startPos.x + Random.Range(-stats.patrolRadius, stats.patrolRadius);
        float randomY = startPos.y + Random.Range(-stats.patrolRadius, stats.patrolRadius);
        
        Vector3Int randomPos = tilemap.WorldToCell(new Vector3(randomX, randomY, 0));
        return randomPos;
    }
    
    public virtual void Death()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // TODO: add in implementation for when the player enters a trigger radius so the enemy thats chasing them.
    }
    
    
}
