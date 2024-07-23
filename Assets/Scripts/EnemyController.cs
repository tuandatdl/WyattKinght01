using UnityEngine;

public abstract class EnemyController : Health
{
    [Header("Damage")]
    // Lượng sát thương của enemy
    protected int damage;

    [Header("Attack Settings")]
    // Kích thước vùng tấn công của enemy
    [SerializeField] private Vector2 attackRange = new Vector2(1.0f, 1.0f);
    // Layer mask để chọn player bị tấn công
    [SerializeField] protected LayerMask playerLayer; // Thay đổi từ private thành protected
    // Điểm xuất phát của tấn công của enemy
    [SerializeField] private Transform attackPoint;

    [Header("Cool Down")]
    // Thời gian cooldown giữa các lần tấn công
    [SerializeField] private float attackCooldown = 1.0f;
    private float attackCooldownTimer = 0.0f;

    [Header("Recoil")]
    // Lực phản kháng khi player bị tấn công
    [SerializeField] protected float recoilForce = 10f; // Tăng giá trị lực phản kháng

    private Animator animator;

    // Khởi tạo các component
    protected override void Start()
    {
        animator = GetComponent<Animator>(); // Lấy component Animator
        base.Start(); // Gọi hàm Start của class cha
    }

    // Cập nhật cooldown và xử lý tấn công player
    protected virtual void Update()
    {
        if (attackCooldownTimer > 0)
        {
            attackCooldownTimer -= Time.deltaTime; // Giảm thời gian cooldown
        }
        AttackPlayer(); // Xử lý tấn công player
    }

    // Hàm tấn công player khi enemy ở trong thời gian tấn công
    protected void AttackPlayer()
    {
        // Kiểm tra cooldown và sức khỏe của enemy
        if (attackCooldownTimer <= 0 && CurrentHealth > 0)
        {
            // Lấy danh sách các player trong vùng tấn công
            Collider2D[] hitPlayers = Physics2D.OverlapBoxAll(attackPoint.position, attackRange, 0, playerLayer);

            foreach (Collider2D player in hitPlayers)
            {
                PlayerAttack playerAttack = player.GetComponent<PlayerAttack>(); // Lấy component PlayerAttack
                if (playerAttack != null && playerAttack.CurrentHealth > 0)
                {
                    ApplyDamageToPlayer(playerAttack); // Áp dụng sát thương cho player
                    attackCooldownTimer = attackCooldown; // Đặt lại thời gian cooldown
                    break; // Dừng vòng lặp sau khi đã tấn công 1 player
                }
            }
        }
    }

    // Hàm xử lý va chạm với player
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (CurrentHealth > 0)
        {
            PlayerAttack playerAttack = collision.collider.GetComponent<PlayerAttack>(); // Lấy component PlayerAttack
            if (playerAttack != null && playerAttack.CurrentHealth > 0)
            {
                ApplyDamageToPlayer(playerAttack); // Áp dụng sát thương cho player
                // Áp dụng lực phản kháng lên player
                Rigidbody2D playerRb = collision.collider.GetComponent<Rigidbody2D>();
                if (playerRb != null)
                {
                    Vector2 recoilDirection = (collision.transform.position - transform.position).normalized; // Tính toán hướng phản kháng
                    Debug.Log("Recoil Direction: " + recoilDirection); // Kiểm tra hướng phản kháng
                    playerRb.AddForce(recoilDirection * recoilForce, ForceMode2D.Impulse); // Áp dụng lực phản kháng
                    Debug.Log("Applied Recoil Force: " + recoilForce); // Kiểm tra lực phản kháng
                }
            }
        }
    }

    // Áp dụng sát thương cho player
    protected virtual void ApplyDamageToPlayer(PlayerAttack playerAttack)
    {
        playerAttack.TakeDamage(damage); // Áp dụng sát thương
        if (animator != null)
        {
            animator.SetTrigger("Attacking"); // Kích hoạt animation tấn công
        }

        Animator playerAnim = playerAttack.GetComponent<Animator>(); // Lấy component Animator từ player
        if (playerAnim != null)
        {
            playerAnim.SetTrigger("Hurting");
        }
    }

    // Xử lý chết
    protected override void Die()
    {
        if (animator != null)
        {
            animator.SetTrigger("IsDead"); // Kích hoạt animation chết
        }
        StartCoroutine(DieRoutine()); // Bắt đầu routine chết
    }

    // Vẽ gizmo cho vùng tấn công trong editor
    protected virtual void OnDrawGizmos()
    {
        if (attackPoint == null) return; // Nếu attackPoint là null, thoát ra

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPoint.position, attackRange); // Vẽ vùng tấn công
    }
}
