using UnityEngine;

public class MoveBetweenWaypoints : MonoBehaviour
{
    [SerializeField] private Transform waypoint1; // First waypoint
    [SerializeField] private Transform waypoint2; // Second waypoint
    [SerializeField] private float speed = 2.0f;  // Movement speed
    [SerializeField] private float stopDuration = 0.5f; // Stop duration at each waypoint

    private Transform targetWaypoint; // Target waypoint
    private float stopTimer; // Stop time counter
    private bool isStopping; // Stop state

    private EnemyController enemyController; // Reference to EnemyController

    private Vector3 originalPosition; // Store original position for Y stabilization

    private void Start()
    {
        // Initialize target waypoint as waypoint1
        targetWaypoint = waypoint1;
        enemyController = GetComponent<EnemyController>();
        originalPosition = transform.position;
    }

    private void Update()
    {
        if (isStopping)
        {
            HandleStopping();
        }
        else
        {
            MoveTowardsTarget();
            CheckWaypointArrival();
        }
    }

    private void HandleStopping()
    {
        stopTimer += Time.deltaTime;
        if (stopTimer >= stopDuration)
        {
            isStopping = false;
            stopTimer = 0.0f;
            SwitchTargetWaypoint();
        }
        else
        {
            // Lock the enemy's Y position during the stop
            LockYAxisPosition();
        }

        // Communicate to EnemyController that movement has stopped
        if (enemyController != null)
        {
            enemyController.SetMovementParams(0, 0);
        }
    }

    private void MoveTowardsTarget()
    {
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;
        float horizontalSpeed = direction.x * speed; // Calculate horizontal speed

        // Update EnemyController with movement parameters
        if (enemyController != null)
        {
            enemyController.SetMovementParams(Mathf.Sign(direction.x), Mathf.Abs(horizontalSpeed));
        }

        transform.position += direction * speed * Time.deltaTime;
    }

    private void CheckWaypointArrival()
    {
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            isStopping = true;
            originalPosition = transform.position; // Store Y position when stopping
        }
    }

    private void SwitchTargetWaypoint()
    {
        targetWaypoint = (targetWaypoint == waypoint1) ? waypoint2 : waypoint1;
    }

    private void LockYAxisPosition()
    {
        // Lock Y position to prevent drifting during stop
        transform.position = new Vector3(transform.position.x, originalPosition.y, transform.position.z);
    }
}
