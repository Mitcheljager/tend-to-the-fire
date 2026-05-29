using UnityEngine;

public class PlayerState : MonoBehaviour {
    [Header("State")]
    public bool isDead = false;
    public bool isInTotalSafetyRange = false;

    private Fire fire;

    void Update() {
        fire = FindFirstObjectByType<Fire>();

        bool wasInTotalSafetyRange = isInTotalSafetyRange;
        isInTotalSafetyRange = IsInTotalSafetyRange();

        if (isInTotalSafetyRange && !wasInTotalSafetyRange) PlayerEvent.EnteredTotalSafetyRange();
    }

    public void Kill() {
        isDead = true;
    }

    private bool IsInTotalSafetyRange() {
        return Vector3.Distance(transform.position, fire.transform.position) < fire.currentTotalSafetyRange;
    }
}
