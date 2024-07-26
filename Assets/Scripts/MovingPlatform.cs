using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Đặt player làm con của nền tảng khi đứng trên nền tảng
            collision.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Ngắt kết nối khi rời khỏi nền tảng
            collision.transform.SetParent(null);
        }
    }
}
