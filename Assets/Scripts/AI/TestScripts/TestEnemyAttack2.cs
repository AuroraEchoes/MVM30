using UnityEngine;

public class TestEnemyAttack2 : AttackBase
{
    public override void DoAttack(EnemyBase enemy, PlayerController PC)
        {
           Debug.Log("We be doing that attack");  
enemy.isAttacking = false;
        }
}
