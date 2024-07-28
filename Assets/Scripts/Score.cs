using System.Collections;
using UnityEngine;

public class Score : MonoBehaviour
{
    // Biến đếm số lượng vàng
    private int goldCount = 0;
    
    // Phương thức xử lý va chạm
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Triggered with: " + other.tag);
        if (other.CompareTag("Gold"))
        {
            Debug.Log("Gold tag detected.");
            AddGold(1); // Cập nhật giá trị goldCount
            Debug.Log("Gold collected. Current gold count: " + goldCount);
            Destroy(other.gameObject); // Xóa vàng sau khi va chạm
        }
        else if (other.CompareTag("Health"))
        {
            Heal(1); // Cộng 1 đơn vị máu hoặc điều chỉnh tùy ý
            Debug.Log("Health collected.");
            Destroy(other.gameObject); // Xóa máu sau khi va chạm
        }
    }

    // Phương thức cộng máu
    public void Heal(int health)
    {
        // Gọi phương thức Heal trong script Health
        GetComponent<Health>().Heal(health);
    }

    // Phương thức cộng tiền
    public void AddGold(int gold)
    {
        goldCount += gold;
    }

    // Phương thức lấy số lượng vàng
    public string GetGoldCount()
    {
        return goldCount.ToString();
    }
}
