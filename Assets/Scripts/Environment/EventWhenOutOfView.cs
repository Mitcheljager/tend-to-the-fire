using UnityEngine;
using UnityEngine.Events;

public class EventWhenOutOfView : MonoBehaviour {
    [Header("Event")]
    public UnityEvent assignedEvent;

    [Header("Config")]
    public float maxDistance = 10f;
    public float minDistance = 2f;
    public float angleBuffer = 2f; // This is use to add a buffer to the view angle, useful for large objects that might stick halfway into view

    private PlayerCamera playerCamera;
    private PlayerFocus playerFocus;
    private bool wasInView = false;

    void Start() {
        playerCamera = FindFirstObjectByType<PlayerCamera>();
        playerFocus = FindFirstObjectByType<PlayerFocus>();
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
        if (playerFocus.isFullyClosed) return false;

        return playerCamera.IsInViewAngleOfPlayer(transform.position, angleBuffer);
    }
}
