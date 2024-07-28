using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text goldText; // Gán Text UI trong Inspector
    public Score1 playerScore; // Gán script Score1 trong Inspector

    private void Start()
    {
        // Đăng ký sự kiện để cập nhật text khi số lượng vàng thay đổi
        if (playerScore != null)
        {
            playerScore.OnGoldChanged += UpdateGoldText;
            UpdateGoldText(int.Parse(playerScore.GetGoldCount())); // Cập nhật text ban đầu
        }
    }

    private void UpdateGoldText(int newGoldCount)
    {
        if (goldText != null)
        {
            goldText.text = "Gold: " + newGoldCount;
        }
        else
        {
            Debug.LogError("GoldText is not assigned!");
        }
    }
}
