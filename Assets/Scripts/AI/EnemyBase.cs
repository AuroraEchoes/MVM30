using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] private string name;
    [SerializeField] private EnemyStats stats;
    [SerializeField] private AttackBase[] attacks;
    

    public virtual void Move()
    {
        
    }

    public virtual void TakeDamage()
    {
        
    }

    public virtual void Attack()
    {
        
    }

    public virtual void Death()
    {
        
    }
}
