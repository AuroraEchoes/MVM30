using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] private string name;
    [SerializeField] public EnemyStats stats;
    [SerializeField] private AttackBase[] attacks;
    private float currentHealth;
    private bool CanBeDamaged = true;
    private bool detectsPlayer = false;

    void Start()
    {
        currentHealth = stats.health;
    }

    public virtual void Move()
    {}

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

    public virtual void Death()
    {
        
    }
}
