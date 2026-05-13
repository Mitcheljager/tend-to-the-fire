using UnityEngine;

public class Interactable : MonoBehaviour {
    public string interactText;
    public Sprite interactImage;

    public virtual void Interact() {
    }

    public virtual string GetInteractText() {
        return interactText;
    }

    public virtual Sprite GetInteractImage() {
        return interactImage;
    }
}
