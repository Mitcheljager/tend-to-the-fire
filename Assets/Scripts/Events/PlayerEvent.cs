using UnityEngine;
using UnityEngine.Events;

public class PlayerEvent : MonoBehaviour {
    public static UnityEvent OnPlayerDiedEvent = new();
    public static UnityEvent OnPlayerEnteredTotalSafetyRange = new();

    public static void EnteredTotalSafetyRange() {
        OnPlayerEnteredTotalSafetyRange.Invoke();
    }

    public static void Died() {
        OnPlayerDiedEvent.Invoke();
    }
}
