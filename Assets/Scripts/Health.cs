using System.Collections;
using UnityEngine;

public abstract class Health : MonoBehaviour
{
    // Bien luu tru suc khoe hien tai
    [SerializeField] private int _currentHealth;
    // Bien luu tru suc khoe toi da
    private int maxHealth;

    // Ham khoi tao, gan suc khoe hien tai bang suc khoe toi da
    protected virtual void Start()
    {
        CurrentHealth = MaxHealth;
    }

    // Thuoc tinh de lay va set suc khoe hien tai
    public int CurrentHealth
    {
        get { return _currentHealth; }
        set
        {
            // Gan gia tri suc khoe hien tai, khong qua maxHealth
            _currentHealth = Mathf.Clamp(value, 0, maxHealth);
        }
    }

    // Thuoc tinh de lay va set suc khoe toi da
    public int MaxHealth
    {
        get { return maxHealth; }
        set { maxHealth = value; }
    }

    // Ham xu ly khi bi tan cong, tru suc khoe va kiem tra xem da chet chua
    public virtual void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        // Neu suc khoe hien tai <= 0, goi ham chet
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    // Ham abstract, phai duoc implement trong cac lop ke thua
    protected abstract void Die();

    // Ham thuc thi quy trinh chet, tat collider, dung velocity, va doi 5 giay truoc khi huy
    protected IEnumerator DieRoutine()
    {
        // Tat collider cua doi tuong
        GetComponent<Collider2D>().enabled = false;
        // Dat velocity cua rigidbody ve 0
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        // Chuyen doi tuong thanh Static, khong bi anh huong boi luc
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

        // Cho doi 5 giay truoc khi huy doi tuong
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
