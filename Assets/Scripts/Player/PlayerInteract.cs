using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour {
    public PlayerState playerState;
    public Transform endTransform;
    [Header("Cursor & UI")]
    public Image interactImage;
    public TMP_Text uiText;
    [Header("Mask")]
    public LayerMask layerMask;
    public int interactableLayerIndex = 0;
    public int interactableSelectedLayerIndex = 0;
    [Header("Interact limits")]
    public float interactRange = 2f;

    private Interactable lastSelectedInteractable;

    void Update() {
        if (lastSelectedInteractable) {
            foreach(GameObject mesh in lastSelectedInteractable.meshes) mesh.layer = interactableLayerIndex;
            lastSelectedInteractable = null;
        }

        GameObject hitObject = GetMouseHoverObject();
        Interactable interactable = UpdateInteractTooltip(hitObject);

        if (interactable) {
            lastSelectedInteractable = interactable;
            foreach(GameObject mesh in lastSelectedInteractable.meshes) mesh.layer = interactableSelectedLayerIndex;
        }

        interactImage.gameObject.SetActive(interactable);
        if (interactable && Input.GetButtonDown("Interact")) interactable.Interact();

        Debug.DrawLine(transform.position, endTransform.position, interactable ? Color.green : Color.red);
    }

    private Interactable UpdateInteractTooltip(GameObject hitObject) {
        if (hitObject == null || (!CanInteract(hitObject))) {
            uiText.text = " ";
            uiText.gameObject.SetActive(false);
            return null;
        }

        if (!hitObject.TryGetComponent<Interactable>(out var interactable)) return null;
        if (!uiText.gameObject.activeInHierarchy) {
            uiText.gameObject.SetActive(true);
            uiText.text = interactable.GetInteractText();
            interactImage.sprite = interactable.GetInteractImage();
        }

        return interactable;
    }

    public bool CanInteract(GameObject hitObject) {
        return hitObject && hitObject.CompareTag("Interactable");
    }

    public GameObject GetMouseHoverObject() {
        Vector3 position = transform.position;
        Vector3 target = position + transform.forward * interactRange;

        Physics.Linecast(position, target, out RaycastHit raycastHit, layerMask);

        endTransform.position = raycastHit.point;

		if (raycastHit.collider == null) {
            endTransform.position = target;
            return null;
        }

        return raycastHit.collider.gameObject;
	}
}
