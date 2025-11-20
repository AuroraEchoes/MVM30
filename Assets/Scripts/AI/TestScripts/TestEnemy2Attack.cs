using UnityEngine;

public class TestEnemy2Attack : AttackBase
{
    public override void DoAttack(EnemyBase enemy, PlayerController PC)
        {
           Debug.Log("The second enemy is doing this attack");  
			enemy.isAttacking = false;
        }
}
