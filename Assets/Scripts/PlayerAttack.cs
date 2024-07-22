using UnityEngine;

public class PlayerAttack : Health
{
    [SerializeField] private int attackDamage;
    [SerializeField] private Vector2 attackRange;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask attackLayer;
    [SerializeField] private GameObject effectAttack;
    [SerializeField] private int playerMaxHealth = 10;
    //coutdown
    [SerializeField] private float attackCooldown = 1.0f;
    private float attackCooldownTimer = 0.0f;

    private Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    protected override void Start()
    {
        maxHealth = playerMaxHealth;
        CurrentHealth = maxHealth;
        base.Start();
    }
    private void Update()
    {
        if(attackCooldown > 0)
        {
            attackCooldownTimer -= Time.deltaTime;
        }
        InputAttack();
    }
    private void InputAttack()
    {
        if(Input.GetMouseButtonDown(0) && attackCooldownTimer <= 0)
        {
            Attack();
            attackCooldownTimer = attackCooldown;
        }
    }
    private void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPoint.position, attackRange, 0, attackLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            Health enemyHealth = enemy.GetComponent<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(attackDamage); // Deal damage to the enemy
                Animator enemyAnim = enemy.GetComponent<Animator>();
                if (enemyAnim != null)
                {
                    enemyAnim.SetTrigger("Hurting"); // Trigger the hurting animation
                }
            }
            
        }

        GameObject effect = Instantiate(effectAttack, attackPoint.position, Quaternion.identity);
        anim.SetTrigger("Attacking");
    }
    private void OnDrawGizmos()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPoint.position, attackRange);
    }
    protected override void Die()
    {
        anim.SetTrigger("IsDead");
        StartCoroutine(DieRoutine());
    }
}
