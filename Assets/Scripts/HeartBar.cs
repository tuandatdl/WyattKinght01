using UnityEngine;
using UnityEngine.UI;

public class HeartBar : MonoBehaviour
{
    [SerializeField] private Image fillHeart; // Image cho thanh máu

    private Health health;

    private void Start()
    {
        // Tìm Health component trong cha hoặc bản thân GameObject
        health = GetComponentInParent<Health>();

        // Đăng ký sự kiện OnHealthChanged
        if (health != null)
        {
            health.OnHealthChanged += UpdateHeartBar;
            // Khởi tạo fillHeart ban đầu
            UpdateHeartBar(health.CurrentHealth, health.MaxHealth);
        }
    }

    private void OnDestroy()
    {
        // Hủy đăng ký sự kiện để tránh lỗi khi đối tượng bị phá hủy
        if (health != null)
        {
            health.OnHealthChanged -= UpdateHeartBar;
        }
    }

    // Phương thức cập nhật thanh máu dựa trên sức khỏe hiện tại và tối đa
    private void UpdateHeartBar(int currentHealth, int maxHealth)
    {
        if (fillHeart != null)
        {
            // Tính toán tỷ lệ và cập nhật fill amount cho Image
            float fillAmount = (float)currentHealth / maxHealth;
            fillHeart.fillAmount = fillAmount;
        }
    }
}
