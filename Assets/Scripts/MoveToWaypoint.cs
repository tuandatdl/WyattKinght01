using UnityEngine;

public class MoveBetweenWaypoints : MonoBehaviour
{
    [SerializeField] private Transform waypoint1; // Diem den thu nhat
    [SerializeField] private Transform waypoint2; // Diem den thu hai
    [SerializeField] private float speed = 2.0f;  // Toc do di chuyen
    [SerializeField] private float stopDuration = 0.5f; // Thoi gian dung tai moi diem den

    private Transform targetWaypoint; // Diem den hien tai
    private float stopTimer; // Dem thoi gian dung
    private bool isStopping; // Trang thai dung

    private EnemyController enemyController; // Tham chieu den EnemyController

    private void Start()
    {
        // Khoi tao diem den ban dau la waypoint1
        targetWaypoint = waypoint1;
        enemyController = GetComponent<EnemyController>(); // Lay tham chieu den EnemyController
    }

    private void Update()
    {
        // Neu dang dung, xu ly thoi gian dung
        if (isStopping)
        {
            HandleStopping();
        }
        else
        {
            // Di chuyen den diem den hien tai
            MoveTowardsTarget();

            // Kiem tra xem da den diem den chua
            CheckWaypointArrival();
        }
    }

    // Ham xu ly thoi gian dung
    private void HandleStopping()
    {
        // Tang dem thoi gian dung len moi khung hinh
        stopTimer += Time.deltaTime;

        // Neu da dung du thoi gian, chuyen sang trang thai di chuyen
        if (stopTimer >= stopDuration)
        {
            // Ket thuc trang thai dung
            isStopping = false;

            // Dat lai dem thoi gian
            stopTimer = 0.0f;

            // Chuyen sang diem den tiep theo
            SwitchTargetWaypoint();
        }

        // Thong bao cho EnemyController rang di chuyen da dung
        if (enemyController != null)
        {
            enemyController.SetMovementParams(0, 0); // Ngung hoat dong di chuyen
        }
    }

    // Ham di chuyen den diem den hien tai
    private void MoveTowardsTarget()
    {
        // Tinh toan huong di chuyen den diem den
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;
        float horizontalSpeed = direction.x * speed; // Tinh toc do theo phuong ngang

        // Cap nhat EnemyController voi cac tham so di chuyen
        if (enemyController != null)
        {
            enemyController.SetMovementParams(Mathf.Sign(direction.x), Mathf.Abs(horizontalSpeed)); // Truyen tham so huong va toc do
        }

        transform.position += direction * speed * Time.deltaTime; // Di chuyen doi tuong theo huong duoc tinh toan
    }

    // Ham kiem tra xem doi tuong da den diem den chua
    private void CheckWaypointArrival()
    {
        // Neu khoang cach giua doi tuong va diem den nho hon 0.1f, bat dau trang thai dung
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            isStopping = true;
        }
    }

    // Ham chuyen diem den hien tai sang diem den khac
    private void SwitchTargetWaypoint()
    {
        // Neu diem den hien tai la waypoint1, chuyen sang waypoint2
        if (targetWaypoint == waypoint1)
        {
            targetWaypoint = waypoint2;
        }
        // Neu diem den hien tai la waypoint2, chuyen sang waypoint1
        else if (targetWaypoint == waypoint2)
        {
            targetWaypoint = waypoint1;
        }
    }
}
