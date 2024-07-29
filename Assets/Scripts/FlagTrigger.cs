using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class FlagTrigger : MonoBehaviour
{
    public GameObject NextGamePanel; // Bảng NextGamePanel
    public Button menuButton; // Nút menu
    public Button nextLevelButton; // Nút next level

    public AudioClip victorySound; // Âm thanh chiến thắng
    private AudioSource audioSource; // AudioSource component

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>(); // Lấy AudioSource component
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        {
            if (other.gameObject.CompareTag("Player"))
            {
                PlayVictorySound(); // Phát âm thanh chiến thắng
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
    private void PlayVictorySound()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.StopBackgroundMusic(); // Tạm dừng âm thanh nền
        }

        if (audioSource != null && victorySound != null)
        {
            audioSource.clip = victorySound;
            audioSource.Play();
        }

        // Resume background music after the victory sound has finished
        StartCoroutine(ResumeBackgroundMusicAfterDelay(victorySound.length));
    }
     private IEnumerator ResumeBackgroundMusicAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Chờ âm thanh chiến thắng kết thúc
        if (AudioManager.instance != null)
        {
            AudioManager.instance.ResumeBackgroundMusic(); // Tiếp tục phát âm thanh nền
        }
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
