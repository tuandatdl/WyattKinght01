using UnityEngine;

public class Score1 : MonoBehaviour
{
    private int goldCount = 0;

    public delegate void GoldChanged(int newGoldCount);
    public event GoldChanged OnGoldChanged;

    public void AddGold(int gold)
    {
        goldCount += gold;
        Debug.Log("Gold added. New gold count: " + goldCount); // Debug log để kiểm tra
        OnGoldChanged?.Invoke(goldCount); // Kích hoạt sự kiện khi số lượng vàng thay đổi
    }

    public string GetGoldCount()
    {
        return goldCount.ToString();
    }
}
