
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
	[SerializeField] private CircleCollider2D DetectionCollider;
    private float currentHealth;
    private bool CanBeDamaged = true;
    private bool detectsPlayer = false;
    public bool isAttacking = false;
    public bool isCooldown = false;
    public List<Vector3> Waypoints;
    public Tilemap tilemap;
    protected Rigidbody2D rb;
    protected Vector2 startPos;
    protected float currentSpeed;
    protected bool detectedPlayer;
    protected PlayerController PC;
	private Transform playerFeet;

    void Start()
    {
        currentHealth = stats.health;
        rb = GetComponent<Rigidbody2D>();
        startPos = transform.position;
        if (tilemap == null)
        {
            foreach (Tilemap map in FindObjectsByType<Tilemap>(FindObjectsSortMode.None))
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
        EnemyLogic();
    }

public virtual void EnemyLogic()
    {
        if (!isAttacking && !isCooldown)
        {
            if (detectedPlayer)
            {
                ChasePlayer();
            }
            else
            {
                Move();
            }
        }

        if (isCooldown)
        {
            rb.linearVelocity = new Vector2(0, 0) * 0;
            Waypoints.Clear();
        }
        Vector2 direction = new Vector2(0, 0);
        if (Waypoints.Count > 0)
        {
            direction = (Waypoints[0] - EnemyFeetPos.position).normalized;
        }
        rb.linearVelocity = direction * currentSpeed * Time.fixedDeltaTime;
    }

    public virtual void AttackMove()
    {
        if (Waypoints.Count > 0)
        {
            if (Vector3.Distance(EnemyFeetPos.position, Waypoints[0]) < 0.5f)
            {
                Waypoints.RemoveAt(0);
            }
        }
    }

    public virtual void ChasePlayer()
    {
        if (Waypoints.Count == 0)
        {
            Waypoints.Add(new Vector3(0, 0, 0));
        }

        currentSpeed = stats.speed;
		if(playerFeet == null)
		{
			foreach(Transform transforms in PC.gameObject.GetComponentsInChildren<Transform>())
			{
				if(transforms.gameObject.name.Contains("Feet"))
				{
					playerFeet = transforms;
					Waypoints[0] = playerFeet.position;
				}	
			}
		}
		else
		{
			Waypoints[0] = playerFeet.position;
		}
        if (Vector3.Distance(EnemyFeetPos.position, Waypoints[0]) < stats.AttackDistance)
        {
            isAttacking = true;
            Attack();
        }
    }

    public virtual void Move()
    {
        if (Waypoints.Count > 0)
        {
            if (Vector3.Distance(EnemyFeetPos.position, Waypoints[0]) < 0.5f)
            {
                Waypoints.RemoveAt(0);
                currentSpeed = 0;
                rb.linearVelocity = new Vector3(0, 0, 0);
                isCooldown = true;
                CancelInvoke("CoolDownEnd");
                Invoke("CoolDownEnd", Random.Range(stats.minCooldown, stats.maxCooldown));
            }
        }
        else
        {
            currentSpeed = stats.speed;
            GetRandomPos();
        }
    }
    public void CoolDownEnd()
    {
        isCooldown = false;
    }

    public void DoCoolDownExternal()
    {
        CancelInvoke("CoolDownEnd");
        isCooldown = true;
        Invoke("CoolDownEnd", Random.Range(stats.minCooldown, stats.maxCooldown));
    }

    public virtual void TakeDamage(float damage)
    {
        if (CanBeDamaged)
            currentHealth -= damage;
    }

    public void SetSpeed(float speed)
    {
        currentSpeed = speed;
    }

    public virtual void Attack()
    {
        if (attacks.Length > 0)
        {
            int chosenAttack = UnityEngine.Random.Range(0, attacks.Length);
            attacks[chosenAttack].DoAttack(this, PC);
        }
    }

    public virtual void AttackBegun()
    {
        rb.linearVelocity = new Vector3(0, 0, 0);
    }
    private void GetRandomPos()
    {
        if (tilemap == null) return;
        Waypoints = new List<Vector3>();
        Vector3Int randomPos = new Vector3Int(100000000, 100000000, 100000000);
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
		Vector3 cellSize = tilemap.layoutGrid.cellSize;
        float randomX = startPos.x + Random.Range(-stats.patrolRadius * cellSize.x, stats.patrolRadius* cellSize.x);
        float randomY = startPos.y + Random.Range(-stats.patrolRadius* cellSize.y, stats.patrolRadius* cellSize.y);

        Vector3Int randomPos = tilemap.WorldToCell(new Vector3(randomX, randomY, 0));
        return randomPos;
    }
    public virtual void Death()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerController>(out PlayerController player))
        {
            PC = player;
            detectedPlayer = true;
            DetectionCollider.radius = stats.extendedChaseRadius;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerController>(out PlayerController player))
        {
            PC = null;
            detectedPlayer = false;
            DetectionCollider.radius = stats.baseChaseRadius;
        }
    }
}
