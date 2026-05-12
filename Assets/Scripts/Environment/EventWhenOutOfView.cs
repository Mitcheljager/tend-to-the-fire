using UnityEngine;
using UnityEngine.Events;

public class EventWhenOutOfView : MonoBehaviour {
    [Header("Event")]
    public UnityEvent assignedEvent;

    [Header("Config")]
    public float maxDistance = 10f;
    public float minDistance = 2f;
    public float angleBuffer = 2f; // This is use to add a buffer to the view angle, useful for large objects that might stick halfway into view

    private Camera playerCamera;
    private PlayerCloseEyes playerCloseEyes;
    private bool wasInView = false;

    void OnDrawGizmosSelected() {
        float angle = GetViewAngle();

        Vector3 leftBoundary = Quaternion.Euler(0, -angle, 0) * Camera.main.transform.forward * maxDistance;
        Vector3 rightBoundary = Quaternion.Euler(0, angle, 0) * Camera.main.transform.forward * maxDistance;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(Camera.main.transform.position, Camera.main.transform.position + leftBoundary);
        Gizmos.DrawLine(Camera.main.transform.position, Camera.main.transform.position + rightBoundary);
        Gizmos.DrawWireSphere(Camera.main.transform.position, maxDistance);
    }

    void Start() {
        playerCamera = Camera.main;
        playerCloseEyes = FindFirstObjectByType<PlayerCloseEyes>();
    }

    void Update() {
        float currentDistance = Vector3.Distance(transform.position, playerCamera.transform.position);
        // Prevent toggling when the player is close to the object, otherwise it might toggle as they pass through the object.
        // Also stop if it's beyond the max distance just prevent unnecessary computations.
        if (currentDistance < minDistance) return;
        if (currentDistance > maxDistance) return;

        bool isInView = IsInView();

        if (wasInView && !isInView) assignedEvent.Invoke();

        wasInView = isInView;
    }

    private bool IsInView() {
        if (playerCloseEyes.isFullyClosed) return false;

        return IsInViewAngleOfPlayer();
    }

    private bool IsInViewAngleOfPlayer() {
        Vector3 direction = (transform.position - playerCamera.transform.position).normalized;
        float currentViewAngle = Vector3.Angle(direction, playerCamera.transform.forward);

        if (currentViewAngle > GetViewAngle()) return false;
        return true;
    }

    private float GetViewAngle() {
        float verticalAngle = Camera.main.fieldOfView;
        float horizontalAngle = 2f * Mathf.Atan(Mathf.Tan(Camera.main.fieldOfView * Mathf.Deg2Rad / 2f) * Camera.main.aspect) * Mathf.Rad2Deg;

        return Mathf.Max(verticalAngle, horizontalAngle) + angleBuffer;
    }
}
