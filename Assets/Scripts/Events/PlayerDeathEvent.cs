using UnityEngine;
using UnityEngine.Events;

public class PlayerDeathEvent : MonoBehaviour {
    public static UnityEvent OnPlayerDeathEvent = new();

    public static void Dispatch() {
        Debug.Log("Dispatching Player Death event");

        OnPlayerDeathEvent.Invoke();
    }
}
