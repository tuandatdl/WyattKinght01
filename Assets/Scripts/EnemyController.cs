using UnityEngine;

public abstract class EnemyController : Health
{
    [Header("Damage")]
    protected int damage;

    [Header("Attack Settings")]
    [SerializeField] private Vector2 attackRange = new Vector2(1.0f, 1.0f);
    [SerializeField] protected LayerMask playerLayer;
    [SerializeField] private Transform attackPoint;

    [Header("Cool Down")]
    [SerializeField] private float attackCooldown = 1.0f;
    private float attackCooldownTimer = 0.0f;

    [Header("Recoil")]
    [SerializeField] protected float recoilForce = 10f;

    [SerializeField] protected GameObject effectAttack1;
    protected Animator animator;
    private Rigidbody2D rb;

    private float currentSpeed;
    private float currentDirection;

    private bool isAlive = true; // Flag to track enemy's alive status

    protected override void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();


        base.Start();
    }

    protected virtual void Update()
    {
        if (!isAlive) return; // Stop further actions if dead

        if (attackCooldownTimer > 0)
        {
            attackCooldownTimer -= Time.deltaTime;
        }

        AttackPlayer();
        UpdateAnimation(currentSpeed, currentDirection);
    }

    protected void AttackPlayer()
    {
        // Only attack if cooldown is 0 and enemy is alive
        if (attackCooldownTimer <= 0 && CurrentHealth > 0 && isAlive)
        {
            // Use Gizmos to visualize the attack range
            Collider2D[] hitPlayers = Physics2D.OverlapBoxAll(attackPoint.position, attackRange, 0, playerLayer);

            foreach (Collider2D player in hitPlayers)
            {
                PlayerAttack playerAttack = player.GetComponent<PlayerAttack>();
                if (playerAttack != null && playerAttack.CurrentHealth > 0)
                {
                    ApplyDamageToPlayer(playerAttack);
                    attackCooldownTimer = attackCooldown; // Reset cooldown
                    break;
                }
            }
        }
    }

    protected virtual void ApplyDamageToPlayer(PlayerAttack playerAttack)
    {
        playerAttack.TakeDamage(damage);
        if (animator != null)
        {
            animator.SetTrigger("Attacking");
        }

        Animator playerAnim = playerAttack.GetComponent<Animator>();
        if (playerAnim != null)
        {
            playerAnim.SetTrigger("Hurting");
        }

        AttackEffect();
    }
    private void AttackEffect()
    {
        GameObject effect = Instantiate(effectAttack1, transform.position, Quaternion.identity); // Tao hieu ung tan cong
        RotateEffect(effect); // Xoay hieu ung theo huong cua player
    }

    // Ham xoay hieu ung de phu hop voi huong cua player
    private void RotateEffect(GameObject effect)
    {
        Vector3 playerDirection = transform.localScale; // Lay huong cua player

        // Neu player quay sang trai, flip hieu ung
        if (playerDirection.x < 0)
        {
            effect.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            effect.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    protected override void Die()
    {
        if (animator != null)
        {
            animator.SetTrigger("IsDead");
        }

        isAlive = false; // Set isAlive flag to false

        if (rb != null)
        {
            rb.velocity = Vector2.zero; // Stop movement by zeroing velocity
            rb.isKinematic = true; // Make Rigidbody kinematic to prevent further physics interactions
        }

        StartCoroutine(DieRoutine());
    }

    protected virtual void OnDrawGizmos()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPoint.position, attackRange);
    }

    // UpdateAnimation now takes speed and direction as parameters
    public void UpdateAnimation(float speed, float direction)
    {
        if (animator != null)
        {
            bool isRunning = speed > 0.1f; // Consider running if speed is significant

            animator.SetBool("IsRunning", isRunning); // Set Running animation

            // Flip the character based on direction
            if (direction != 0)
            {
                transform.localScale = new Vector3(direction, 1, 1);
            }
        }
    }

    // Method to set movement parameters, used by other scripts
    public void SetMovementParams(float direction, float speed)
    {
        currentDirection = direction;
        currentSpeed = speed;
    }
}
