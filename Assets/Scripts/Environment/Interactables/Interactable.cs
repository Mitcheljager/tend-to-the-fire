using UnityEngine;

public class Interactable : MonoBehaviour {
    [Header("Interactable")]
    public string interactText;
    public Sprite interactImage;
    public GameObject[] meshes;
    public Collider interactableCollider;

    public virtual void Interact() {
    }

    public virtual string GetInteractText() {
        return interactText;
    }

    public virtual Sprite GetInteractImage() {
        return interactImage;
    }
}
