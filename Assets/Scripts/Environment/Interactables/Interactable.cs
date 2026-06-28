using UnityEngine;

public class Interactable : MonoBehaviour {
    [Header("Interactable")]
    public string interactText;
    public Sprite interactImage;
    public GameObject[] meshes;
    public Collider interactableCollider;
    public float interactableOutlineRange = 5f;

    private PlayerInteract playerInteract;

    void OnEnable() {
        playerInteract = FindFirstObjectByType<PlayerInteract>();
    }

    public virtual void Update() {
        bool isInOutlineRange = Vector3.Distance(transform.position, playerInteract.transform.position) < interactableOutlineRange;

        SetLayer(isInOutlineRange ? playerInteract.interactableInRangeLayerIndex : playerInteract.interactableLayerIndex);
    }

    public virtual void Interact() {
    }

    public virtual string GetInteractText() {
        return interactText;
    }

    public virtual Sprite GetInteractImage() {
        return interactImage;
    }

    public void SetLayer(int layerIndex) {
        if (meshes == null) return;

        foreach(GameObject mesh in meshes) mesh.layer = layerIndex;
    }
}
