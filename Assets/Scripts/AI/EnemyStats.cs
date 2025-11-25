using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "Enemy/Stats")]
public class EnemyStats : ScriptableObject
{
   public float health;
   public float speed;
   public float patrolRadius; 
	public float AttackDistance;
	public float minCooldown;
	public float maxCooldown;
   public float baseChaseRadius;
   public float extendedChaseRadius;

}
