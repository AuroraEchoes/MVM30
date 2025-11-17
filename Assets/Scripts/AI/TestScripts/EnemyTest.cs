using UnityEngine;

public class EnemyTest : EnemyBase
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Attack();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public override void Move()
    {
        
    }

    public override void TakeDamage()
    {
        
    }

    public override void Attack()
    {   
        base.Attack();
    }

    public override void Death()
    {
        
    }
}
