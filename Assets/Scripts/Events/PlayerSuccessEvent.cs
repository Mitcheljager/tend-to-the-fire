using UnityEngine;
using UnityEngine.Events;

public class PlayerSuccessEvent : MonoBehaviour {
    public static UnityEvent OnPlayerSuccessEvent = new();

    public static void Dispatch() {
        Debug.Log("Dispatching Player Success event");

        OnPlayerSuccessEvent.Invoke();
    }
}
