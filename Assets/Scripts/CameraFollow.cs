using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* offset: Một Vector3 định nghĩa vị trí lệch của camera so với mục tiêu. Trong trường hợp này, camera cách mục tiêu 10 đơn vị trên trục z.
smoothTime: Một float xác định thời gian để camera bắt kịp mục tiêu. Giá trị nhỏ hơn sẽ làm cho camera di chuyển nhanh hơn.
velocity: Một Vector3 được sử dụng nội bộ bởi hàm SmoothDamp để theo dõi vận tốc của camera.
target: Một tham chiếu Transform đến đối tượng mục tiêu mà camera sẽ theo dõi. */
public class CameraFollow : MonoBehaviour
{
 private Vector3 offset = new Vector3(0f, 0f, -10f);
 private float smoothTime = 0.25f;
 private Vector3 velocity = Vector3.zero;
 [SerializeField] private Transform target;
    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition = target.position + offset;
        /*giải thích 
        transform.position: Vị trí hiện tại của camera.
        targetPosition: Vị trí mục tiêu mà camera cần di chuyển tới.
        ref velocity: Tham chiếu đến vận tốc hiện tại của camera.
        smoothTime: Thời gian để di chuyển mượt mà từ vị trí hiện tại đến vị trí mục tiêu.*/
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
