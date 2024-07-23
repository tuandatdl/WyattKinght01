using UnityEngine;

public class Skeleton : EnemyController
{
    [Header("Health")]
    // Suc khoe toi da cua Skeleton
    [SerializeField] private int skeletonMaxHealth = 10;
    [Space(5)]

    [Header("Damage")]
    // Luong sat thuong cua Skeleton
    [SerializeField] private int skeletonDamage = 2;

    // Khoi tao skeleton
    protected override void Start()
    {
        MaxHealth = skeletonMaxHealth; // Dat suc khoe toi da cho Skeleton
        damage = skeletonDamage; // Dat sat thuong cho Skeleton
        base.Start(); // Goi ham Start cua class cha
    }

    // Xy ly chet
    protected override void Die()
    {
        base.Die(); // Goi ham Die cua class cha
    }
}
