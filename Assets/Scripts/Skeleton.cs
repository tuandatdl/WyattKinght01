using UnityEngine;

public class Skeleton : EnemyController
{
    [Header("Health")]
    [SerializeField] private int skeletonMaxHealth = 10;
    [Space(5)]

    [Header("Damage")]
    [SerializeField] private int skeletonDamage = 2;

    protected override void Start()
    {
        MaxHealth = skeletonMaxHealth;
        damage = skeletonDamage;
        base.Start();
    }

    protected override void Die()
    {
        base.Die();
    }
}
