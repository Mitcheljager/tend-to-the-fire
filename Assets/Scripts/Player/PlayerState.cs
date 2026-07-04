using UnityEngine;

public class PlayerState : MonoBehaviour {
    [Header("State")]
    public bool isDead = false;
    public bool isInTotalSafetyRange = false;

    private Fire fire;

    void Start() {
        fire = FindFirstObjectByType<Fire>();
    }

    void Update() {
        if (fire == null) return;

        bool wasInTotalSafetyRange = isInTotalSafetyRange;
        isInTotalSafetyRange = IsInTotalSafetyRange();

        if (isInTotalSafetyRange && !wasInTotalSafetyRange) PlayerEvent.EnteredTotalSafetyRange();
    }

    public void Kill() {
        isDead = true;

        Debug.Log("Kill");

        PlayerEvent.OnPlayerDiedEvent.Invoke();
    }

    private bool IsInTotalSafetyRange() {
        return Vector3.Distance(transform.position, fire.transform.position) < fire.currentTotalSafetyRange;
    }
}
