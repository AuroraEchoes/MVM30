using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyTest : EnemyBase
{
    public List<Vector3> Waypoints;
    public float currentSpeed = 1;
    public Coroutine attackCoroutine;

    public bool isAttacking = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentSpeed = stats.speed;
        Attack();
        Move();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public override void Move()
    {
        if (!isAttacking)
            StartCoroutine(MoveAlongPath());
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

    private IEnumerator  MoveAlongPath()
    {
        while (Waypoints.Count > 0)
        {
            float value = 0;
            
            Vector3 StartPosition = transform.position;
            float requiredTime = Vector3.Distance(StartPosition, Waypoints[0]) / currentSpeed;
            while (value < 1)
            {
                //int fps = (int)(1f / Time.unscaledDeltaTime);
                //Debug.Log(fps + " is the current FPS");
                value += Time.deltaTime;
                this.gameObject.transform.position = Vector3.Lerp(StartPosition, Waypoints[0], value / requiredTime);
                yield return null;
            }
            Waypoints.RemoveAt(0);
            yield return null;
        }
        yield return null;
    }
    
}
