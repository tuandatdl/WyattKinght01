using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip[] backgroundMusic; // Mảng chứa các bài nhạc nền cho các scenes
    public AudioClip menuMusic; // Bài nhạc nền cho menu
    private AudioSource audioSource;

    public static AudioManager instance; // Tạo instance để truy cập từ các script khác

    private void Awake()
    {
        // Kiểm tra nếu có instance khác đã tồn tại, nếu có thì phá hủy
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // Đảm bảo AudioManager không bị phá hủy khi chuyển scene
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        PlayMenuMusic(); // Chơi nhạc nền cho menu khi bắt đầu game
    }

    public void PlayMenuMusic()
    {
        audioSource.clip = menuMusic;
        audioSource.Play();
    }

    public void PlaySceneMusic(int sceneIndex)
    {
        if (sceneIndex == -1)
        {
            PlayMenuMusic();
        }
        else if (sceneIndex >= 0 && sceneIndex < backgroundMusic.Length)
        {
            audioSource.clip = backgroundMusic[sceneIndex];
            audioSource.Play();
        }
    }

    public void StopBackgroundMusic()
    {
        audioSource.Pause(); // Tạm dừng âm thanh nền
    }

    public void ResumeBackgroundMusic()
    {
        audioSource.UnPause(); // Tiếp tục phát âm thanh nền
    }
}
