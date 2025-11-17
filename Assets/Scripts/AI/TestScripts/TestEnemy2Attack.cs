using UnityEngine;

public class TestEnemy2Attack : AttackBase
{
    public override void DoAttack(EnemyBase enemy)
        {
           Debug.Log("The second enemy is doing this attack");  
        }
}
