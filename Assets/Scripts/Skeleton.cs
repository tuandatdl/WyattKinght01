using UnityEngine;

public class Skeleton : EnemyController
{
    [SerializeField] private int skeletonMaxHealth = 10;
    [SerializeField] private int skeletonDamage = 2;
    protected override void Start()
    {
        maxHealth = skeletonMaxHealth;
        damage = skeletonDamage;
        base.Start();
    }
    protected override void Die()
    {
        base.Die();
    }
}
