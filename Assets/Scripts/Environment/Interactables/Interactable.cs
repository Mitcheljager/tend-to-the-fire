using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour {
    public string interactText;

    public virtual void Interact() {
    }

    public virtual string GetInteractableText() {
        return interactText;
    }
}
