using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] private string name;
    [SerializeField] private EnemyStats stats;
    [SerializeField] private AttackBase[] attacks;
    private float currentHealth;

    void Start()
    {
        currentHealth = stats.health;
    }

    public virtual void Move()
    {
        
    }

    public virtual void TakeDamage()
    {
        
    }

    public virtual void Attack()
    {
        if (attacks.Length > 0)
        {
            int chosenAttack = UnityEngine.Random.Range(0, attacks.Length);
            attacks[chosenAttack].DoAttack();
        }
    }

    public virtual void Death()
    {
        
    }
}
