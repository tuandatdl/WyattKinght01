using System.Collections;
using UnityEngine;

public abstract class Health : MonoBehaviour
{
    [SerializeField] private int _currentHealth;
    protected int maxHealth;
    protected virtual void Start()
    {
        CurrentHealth = maxHealth;
    }
    public int CurrentHealth
    {
        get { return _currentHealth; }
        set
        {
            _currentHealth = Mathf.Clamp(value, 0, maxHealth);
        }
    }
    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        if(CurrentHealth <= 0) 
        {
            Die();
        }
    }
    protected abstract void Die();
    protected IEnumerator DieRoutine()
    {
        GetComponent<Collider2D>().enabled = false; 
        GetComponent<Rigidbody2D>().velocity = Vector2.zero; 
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
