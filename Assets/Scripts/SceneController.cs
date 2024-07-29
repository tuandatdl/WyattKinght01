using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void ChuyenSangSceneMap()
    {
        string sceneName = "Map1";
        SceneManager.LoadScene(sceneName);
        AudioManager.instance.PlaySceneMusic(0); // Chơi nhạc nền cho Map1 (vị trí 0 trong mảng)
    }

    public void ChuyenSangSceneMenu()
    {
        string sceneName = "Menu";
        SceneManager.LoadScene(sceneName);
        AudioManager.instance.PlaySceneMusic(-1); // Chơi nhạc nền cho Menu (sử dụng -1 để phân biệt)
    }

    public void Restart()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);

        int sceneIndex = GetSceneIndexByName(currentSceneName);
        AudioManager.instance.PlaySceneMusic(sceneIndex); // Chơi nhạc nền cho scene hiện tại
    }

    public void NextLevel()
    {
        string nextSceneName = "";
        int nextSceneIndex = -1;

        if (SceneManager.GetActiveScene().name == "Map1")
        {
            nextSceneName = "Map2";
            nextSceneIndex = 1; // Chơi nhạc nền cho Map2 (vị trí 1 trong mảng)
        }
        else if (SceneManager.GetActiveScene().name == "Map2")
        {
            nextSceneName = "Map3";
            nextSceneIndex = 2; // Chơi nhạc nền cho Map3 (vị trí 2 trong mảng)
        }
        // Thêm các điều kiện tiếp theo cho các level tiếp theo

        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
            AudioManager.instance.PlaySceneMusic(nextSceneIndex); // Chơi nhạc nền cho scene tiếp theo
        }
    }

    private int GetSceneIndexByName(string sceneName)
    {
        switch (sceneName)
        {
            case "Map1":
                return 0;
            case "Map2":
                return 1;
            case "Map3":
                return 2;
            default:
                return -1;
        }
    }
}
