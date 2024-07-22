using UnityEngine;

public abstract class EnemyController : Health
{
    protected int damage;             
    [SerializeField] private Vector2 attackRange = new Vector2(1.0f, 1.0f); 
    [SerializeField] private LayerMask playerLayer;           
    [SerializeField] private Transform attackPoint;

    [SerializeField] private float attackCooldown = 1.0f; 
    private float attackCooldownTimer = 0.0f;

    private Animator animator;
    private Rigidbody2D rb;
    protected override void Start()
    {
        animator = GetComponent<Animator>();
        base.Start();
    }
    protected virtual void Update()
    {
        if (attackCooldownTimer > 0)
        {
            attackCooldownTimer -= Time.deltaTime;
        }

        AttackPlayer();
    }
    private void AttackPlayer()
    {
       
        if (attackCooldownTimer <= 0 && CurrentHealth > 0)
        {
            Collider2D[] hitPlayers = Physics2D.OverlapBoxAll(attackPoint.position, attackRange, 0, playerLayer);

            foreach (Collider2D player in hitPlayers)
            {
                PlayerAttack playerAttack = player.GetComponent<PlayerAttack>();
                if (playerAttack != null && playerAttack.CurrentHealth > 0)
                {
                    
                    Debug.Log("Enemy attacking player!");

                    playerAttack.TakeDamage(damage);
                    animator.SetTrigger("Attacking");

                    
                    Animator playerAnim = player.GetComponent<Animator>();
                    if (playerAnim != null)
                    {
                        playerAnim.SetTrigger("Hurting");
                    }
                    attackCooldownTimer = attackCooldown;
                    break;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (CurrentHealth > 0)
        {
            PlayerAttack playerAttack = collision.collider.GetComponent<PlayerAttack>();
            if (playerAttack != null && playerAttack.CurrentHealth > 0)
            {
                playerAttack.TakeDamage(damage);
                animator.SetTrigger("Hurting");

                
                Animator playerAnim = playerAttack.GetComponent<Animator>();
                if (playerAnim != null)
                {
                    playerAnim.SetTrigger("Hurting");
                }
            }
        }
    }

    protected override void Die()
    {
        animator.SetTrigger("IsDead");
        StartCoroutine(DieRoutine());
    }
    protected virtual void OnDrawGizmos()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPoint.position, attackRange);
    }
}
