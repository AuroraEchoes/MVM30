using UnityEngine;

public class TestEnemy2Attack2: AttackBase
{
    public override void DoAttack(EnemyBase enemy, PlayerController PC)
        {
           Debug.Log("the second enemy is doing that attack");  
enemy.isAttacking = false;
        }
}
