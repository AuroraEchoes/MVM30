
using UnityEngine;



public class EnemyTest : EnemyBase
{
    public Coroutine attackCoroutine;


    
    public override void Move()
    {
        base.Move();
    }
    
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
    }

    public override void Attack()
    {   
        base.Attack();
    }

    public override void Death()
    {
        
    }
    
}
