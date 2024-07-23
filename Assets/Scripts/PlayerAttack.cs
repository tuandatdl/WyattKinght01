using UnityEngine;

public class PlayerAttack : Health
{
    [Header("Damage")]
    // Luong sat thuong cua player khi tan cong
    [SerializeField] private int attackDamage;
    // Kich thuoc vung tan cong
    [SerializeField] private Vector2 attackRange;
    // Diem xuat phat cua tan cong
    [SerializeField] private Transform attackPoint;
    // Layer mask de chon doi tuong bi tan cong
    [SerializeField] private LayerMask attackLayer;
    // Hieu ung khi tan cong
    [SerializeField] private GameObject effectAttack;
    // Kiem tra xem co duoc phep tan cong hay khong
    [SerializeField] private bool IsAttacking = true;
    private bool canAttack;

    [Header("Health")]
    // Suc khoe toi da cua player
    [SerializeField] private int playerMaxHealth = 10;

    [Header("Cool Down")]
    // Thoi gian cooldown giua cac lan tan cong
    [SerializeField] private float attackCooldown = 1.0f;
    private float attackCooldownTimer = 0.0f;

    [Header("Recoil")]
    // Luong luc phan khuc khi tan cong
    [SerializeField] private float recoilForce = 5f;

    private Rigidbody2D rb;
    private Animator anim;

    // Khoi tao cac component
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Khoi tao suc khoe va start base
    protected override void Start()
    {
        MaxHealth = playerMaxHealth;
        base.Start();
    }

    // Cap nhat cooldown va xu ly input tan cong
    private void Update()
    {
        UpdateAttackCooldown(); // Cap nhat thoi gian cooldown
        InputAttack(); // Xu ly input tan cong
    }

    // Cap nhat thoi gian cooldown
    private void UpdateAttackCooldown()
    {
        if (attackCooldownTimer > 0)
        {
            attackCooldownTimer -= Time.deltaTime; // Giam thoi gian cooldown
        }
        else
        {
            canAttack = true; // Cho phep tan cong neu cooldown da het
        }
    }

    // Xu ly input tan cong
    private void InputAttack()
    {
        if (canAttack && IsAttacking && Input.GetMouseButtonDown(0)) // Kiem tra dieu kien tan cong
        {
            Attack(); // Thuc hien tan cong
            attackCooldownTimer = attackCooldown; // Dat lai thoi gian cooldown
            canAttack = false; // Ngung cho phep tan cong cho den khi cooldown het
        }
    }

    // Ham tan cong khi player nhan duoc input tan cong
    private void Attack()
    {
        // Lấy danh sách các đối tượng trong vùng tấn công
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPoint.position, attackRange, 0, attackLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            Health enemyHealth = enemy.GetComponent<Health>(); // Lấy component Health từ enemy
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(attackDamage); // Áp dụng sát thương cho enemy

                Animator enemyAnim = enemy.GetComponent<Animator>(); // Lấy component Animator từ enemy
                if (enemyAnim != null)
                {
                    enemyAnim.SetTrigger("Hurting"); // Kích hoạt animation bị tấn công
                    ApplyRecoilForce(enemy); // Áp dụng lực phản kháng cho enemy
                }
            }
        }
        anim.SetTrigger("Attacking"); // Kích hoạt animation tấn công của player
        AttackEffect(); // Tạo hiệu ứng tấn công
    }
    public override void TakeDamage(int damage)
    {
        Debug.Log("Player taking damage"); // Kiểm tra xem hàm có được gọi không
        CurrentHealth -= damage;
        // Nếu sức khỏe hiện tại <= 0, gọi hàm chết
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }
    // Ham tao hieu ung tan cong tai vi tri cua attackPoint
    private void AttackEffect()
    {
        GameObject effect = Instantiate(effectAttack, attackPoint.position, Quaternion.identity); // Tao hieu ung tan cong
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

    // Ham ap dung luc phan khuc cho enemy
    private void ApplyRecoilForce(Collider2D enemy)
    {
        Rigidbody2D enemyRb = enemy.GetComponent<Rigidbody2D>();
        if (enemyRb != null)
        {
            Vector2 recoilDirection = (enemy.transform.position - transform.position).normalized; // Tính toán hướng phản kháng
            Debug.Log("Recoil Direction: " + recoilDirection); // Kiểm tra hướng phản kháng
            enemyRb.AddForce(recoilDirection * recoilForce, ForceMode2D.Impulse); // Áp dụng lực phản kháng
            Debug.Log("Applied Recoil Force: " + recoilForce); // Kiểm tra lực phản kháng
        }
    }

    // Ve gizmo cho vung tan cong trong editor
    private void OnDrawGizmos()
    {
        if (attackPoint == null) return; // Neu attackPoint la null, thoat ra

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPoint.position, attackRange); // Ve vung tan cong
    }

    // Xy ly chet
    protected override void Die()
    {
        anim.SetTrigger("IsDead"); // Kich hoat animation chet
        StartCoroutine(DieRoutine()); // Bat dau routine chet
    }

}
