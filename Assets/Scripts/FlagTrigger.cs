using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class FlagTrigger : MonoBehaviour
{
    public GameObject NextGamePanel; // Bảng NextGamePanel
    public Button menuButton; // Nút menu
    public Button nextLevelButton; // Nút next level

    private void OnTriggerEnter2D(Collider2D other)
    {
        {
            if (other.gameObject.CompareTag("Player"))
            {
                NextGamePanel.SetActive(true); // Hiển thị bảng NextGamePanel
            }
        }
    }
    private void Start()
    {
        // Đăng ký sự kiện cho nút menu và next level
        menuButton.onClick.AddListener(MenuButtonClicked);
        nextLevelButton.onClick.AddListener(NextLevelButtonClicked);
    }
    private void MenuButtonClicked()
    {
        // Tải scene menu
        SceneManager.LoadScene("Menu");
    }

    private void NextLevelButtonClicked()
    {
        // Tải scene next level
        SceneManager.LoadScene("NextLevel");
    }
}
