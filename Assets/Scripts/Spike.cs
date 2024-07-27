using UnityEngine;

public class Spike : EnemyController
{
    [Header("Damage")]
    // Lượng sát thương của Spike
    [SerializeField] private int spikeDamage = 1;

    // Khởi tạo Spike
    private void Awake()
    {
        damage = spikeDamage;
    }

    // Ghi đè phương thức TakeDamage để Spike không bị ảnh hưởng bởi sát thương
    public override void TakeDamage(int damage)
    {
        // Spike không bị ảnh hưởng bởi sát thương, do đó không làm gì ở đây
    }

    // Xử lý va chạm với player
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Kiểm tra xem collider có phải là player không
        if ((playerLayer & (1 << collision.gameObject.layer)) != 0)
        {
            PlayerAttack playerAttack = collision.collider.GetComponent<PlayerAttack>(); // Lấy component PlayerAttack
            if (playerAttack != null && playerAttack.CurrentHealth > 0)
            {
                playerAttack.TakeDamage(damage); // Áp dụng sát thương cho player

                // Áp dụng lực phản kháng lên player
                Rigidbody2D playerRb = collision.collider.GetComponent<Rigidbody2D>();
                if (playerRb != null)
                {
                    Vector2 recoilDirection = (collision.transform.position - transform.position).normalized; // Tính toán hướng phản kháng
                    playerRb.AddForce(recoilDirection * recoilForce, ForceMode2D.Impulse); // Áp dụng lực phản kháng
                }

                // Kích hoạt animation "Hurting" của player
                Animator playerAnim = playerAttack.GetComponent<Animator>(); // Lấy component Animator từ player
                if (playerAnim != null)
                {
                    playerAnim.SetTrigger("Hurting"); // Kích hoạt animation bị tấn công
                }
            }
        }
    }
}
