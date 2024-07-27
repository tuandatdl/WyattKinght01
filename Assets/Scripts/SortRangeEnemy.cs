using UnityEngine;

public class SortRangeEnemy : EnemyController
{
    [Header("Health")]
    [SerializeField] private int enemyMaxHealth = 10;
    [Space(5)]

    [Header("Damage")]
    [SerializeField] private int enemyDamage = 2;

    protected override void Start()
    {
        MaxHealth = enemyMaxHealth;
        damage = enemyDamage;
        base.Start();
    }

    protected override void Die()
    {
        base.Die();
    }
}
