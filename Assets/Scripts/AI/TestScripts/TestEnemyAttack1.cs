using System.Collections;
using UnityEngine;

public class TestEnemyAttack1 : AttackBase
{
    public float attackspeed = 10;
    public override void DoAttack(EnemyBase enemy)
        {
           Debug.Log("We be doing ythis attack");
           if (enemy is EnemyTest test)
           {
               test.isAttacking = true;
               test.attackCoroutine = test.StartCoroutine(MoveAttack(test));
               //  StartCoroutine(MoveAttack(test));

           }
        }

    private IEnumerator MoveAttack(EnemyTest test)
    {
        test.Waypoints.Clear();
        float value = 0;
        Vector3 endPosition = test.transform.position;
        endPosition.x += 20;
        Vector3 StartPosition = test.transform.position;
        Debug.Log("We have begun the attack lunge");
        float requiredTime = Vector3.Distance(StartPosition, endPosition) / attackspeed;
        while (value < 1)
        {
            //int fps = (int)(1f / Time.unscaledDeltaTime);
            //Debug.Log(fps + " is the current FPS");
            value += Time.deltaTime;
            test.gameObject.transform.position = Vector3.Lerp(StartPosition, endPosition, value / requiredTime);
            yield return null;
        }
        Debug.Log("We have ended the attack lunge");

        test.isAttacking = false;
        yield return null;
    }
}
