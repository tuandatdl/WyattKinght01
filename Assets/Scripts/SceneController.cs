using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // Start is called before the first frame update
    public void ChuyenSangSceneMap()
   {
        SceneManager.LoadScene("Map1");
   }
   public void ChuyenSangSceneMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel()
    {
        // Tạm thời, bạn có thể thêm tên scene tiếp theo vào đây
        // Sau này, bạn có thể tạo một hệ thống quản lý level để tự động tải level tiếp theo
        if (SceneManager.GetActiveScene().name == "Map1")
        {
            SceneManager.LoadScene("Map2");
        }
        else if (SceneManager.GetActiveScene().name == "Map2")
        {
            SceneManager.LoadScene("Map3");
        }
        // Thêm các điều kiện tiếp theo cho các level tiếp theo
    }
}
