using UnityEngine;

public class RabbitEnemy : EnemyBase
{
    public Coroutine attackCoroutine;

    public override void EnemyLogic()
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
        else if(attackCoroutine != null && isAttacking)
        {
            AttackMove();
        }

        if (Waypoints.Count > 0)
        {
            Vector2 direction = (Waypoints[0] - EnemyFeetPos.position).normalized;
            rb.linearVelocity = direction * currentSpeed * Time.fixedDeltaTime;
        }
        

    }
    public override void Move()
    {
        base.Move();
    }

    public  void AttackMove()
    {
        if (Waypoints.Count > 0)
        {
            if (Vector3.Distance(EnemyFeetPos.position, Waypoints[0]) < 0.5f)
            {
                Waypoints.RemoveAt(0);
            }
        }
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
