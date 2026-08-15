using UnityEngine;

public class PlayerState : MonoBehaviour {
    [Header("State")]
    public bool isDead = false;
    public bool isInTotalSafetyRange = false;
    public bool isInStasis = false;

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

    public void SetInStasis(bool state) {
        isInStasis = state;
    }

    private bool IsInTotalSafetyRange() {
        return Vector3.Distance(transform.position, fire.transform.position) < fire.currentTotalSafetyRange;
    }
}
