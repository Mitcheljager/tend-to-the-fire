using UnityEngine;
using UnityEngine.Events;

public class InteractableByEvent : MonoBehaviour {
    [Header("Event")]
    public UnityEvent assignedEvent;

    public void Interact() {
        assignedEvent.Invoke();
    }
}
