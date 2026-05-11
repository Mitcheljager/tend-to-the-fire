using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour {
    public string text;
    [Header("Event")]
    public UnityEvent assignedEvent;

    public void Interact() {
        assignedEvent.Invoke();
    }
}
