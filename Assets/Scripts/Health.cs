using System;
using System.Collections;
using UnityEngine;

public abstract class Health : MonoBehaviour
{
    // Biến lưu trữ sức khỏe hiện tại
    [SerializeField] private int _currentHealth;
    // Biến lưu trữ sức khỏe tối đa
    private int maxHealth;

    // Sự kiện khi sức khỏe thay đổi
    public event Action<int, int> OnHealthChanged;
     
    // Biến đếm số lượng vàng
    private int goldCount = 0;

    // Phương thức khởi tạo, gán sức khỏe hiện tại bằng sức khỏe tối đa
    protected virtual void Start()
    {
         if (MaxHealth <= 0)
        {
            MaxHealth = 100; // Giá trị mặc định nếu MaxHealth chưa được thiết lập
        }
        CurrentHealth = MaxHealth;
    }

    // Thuộc tính để lấy và set sức khỏe hiện tại
    public int CurrentHealth
    {
        get { return _currentHealth; }
        set
        {
            // Gán giá trị sức khỏe hiện tại, không quá maxHealth
            _currentHealth = Mathf.Clamp(value, 0, maxHealth);
            OnHealthChanged?.Invoke(_currentHealth, maxHealth); // Gọi sự kiện khi sức khỏe thay đổi
        }
    }

    // Thuộc tính để lấy và set sức khỏe tối đa
    public int MaxHealth
    {
        get { return maxHealth; }
        set { maxHealth = value; }
    }

    // Phương thức xử lý khi bị tấn công, trừ sức khỏe và kiểm tra xem đã chết chưa
    public virtual void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        // Nếu sức khỏe hiện tại <= 0, gọi hàm chết
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }
    // Phương thức để hồi máu
    public void Heal(int health)
    {
        CurrentHealth += health;
    }

    // Phương thức xử lý va chạm
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Heart"))
        {
            Heal(1);
            Destroy(other.gameObject); // Xóa trái tim sau khi va chạm
        }
        else if (other.CompareTag("Gold"))
        {
            goldCount += 1;
            Destroy(other.gameObject); // Xóa vàng sau khi va chạm
        }
    }

    // Phương thức abstract, phải được implement trong các lớp kế thừa
    protected abstract void Die();

    // Phương thức thực thi quy trình chết, tắt collider, dừng velocity, và đợi 5 giây trước khi hủy
    protected IEnumerator DieRoutine()
    {
        // Tắt collider của đối tượng
        GetComponent<Collider2D>().enabled = false;
        // Đặt velocity của rigidbody về 0
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        // Chuyển đối tượng thành Static, không bị ảnh hưởng bởi lực
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

        // Chờ đợi 0.8 giây trước khi hủy đối tượng
        yield return new WaitForSeconds(0.8f);
        Destroy(gameObject);
    }
}
