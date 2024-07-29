using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText;    // UI Text để hiển thị điểm
    public GameObject[] coins;  // Mảng các đối tượng coin
    public GameObject[] hearts; // Mảng các đối tượng heart

    private int goldCount = 0;    // Biến đếm số lượng vàng

    [SerializeField] protected GameObject effecfItem;

    // Tham chiếu đến đối tượng Health của người chơi
    private PlayerAttack playerAttack;


    private void Start()
    {
        playerAttack = FindObjectOfType<PlayerAttack>();
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        scoreText.text = " " + goldCount.ToString();
        
    }

    public void AddGold(int gold)
    {
        goldCount += gold;
        UpdateScoreUI();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Gold"))
        {
            AddGold(1);
            GameObject effect = Instantiate(effecfItem, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Health"))
        {
            playerAttack.Heal(2); // Tăng máu cho người chơi khi nhặt heart
            GameObject effect = Instantiate(effecfItem, transform.position, Quaternion.identity);
            Destroy(other.gameObject); // Xóa heart sau khi va chạm
            
        }
    }
}
