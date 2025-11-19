using System.Collections;
using UnityEngine;

public class TestEnemyAttack1 : AttackBase
{
    public float attackspeed = 400;
    public override void DoAttack(EnemyBase enemy , PlayerController PC)
        {
           Debug.Log("We be doing ythis attack");
           if (enemy is EnemyTest test)
           {
               test.isAttacking = true;
               test.attackCoroutine = test.StartCoroutine(MoveAttack(test,PC));
           }
        }

    private IEnumerator MoveAttack(EnemyTest test, PlayerController PC)
    {
        test.Waypoints.Clear();
        float value = 0;
		test.SetSpeed(0);
        Vector3 endPosition = PC.gameObject.transform.position;
        Vector3 StartPosition = test.EnemyFeetPos.position;
		foreach(Transform transforms in PC.gameObject.GetComponentsInChildren<Transform>())
		{
			if(transforms.gameObject.name.Contains("Feet"))
			{
				endPosition = transforms.position;
			}	
		}	
		test.Waypoints.Add(endPosition);	
		yield return new WaitForSeconds(1.0f);
        float requiredTime = Vector3.Distance(StartPosition, endPosition) / (attackspeed * Time.fixedDeltaTime);
        while (value < requiredTime)
        {
            value+= Time.deltaTime;
            test.SetSpeed(attackspeed);
			yield return null;
        }
        
		test.SetSpeed(test.stats.speed);
		test.DoCoolDownExternal();
        test.isAttacking = false;
        yield return null;
    }
}
