using UnityEngine;
using TMPro;

public class PlayerInteract : MonoBehaviour {
    public PlayerState playerState;
    public Transform endTransform;
    [Header("Cursor & UI")]
    public GameObject interactImage;
    public TMP_Text uiText;
    [Header("Mask")]
    public LayerMask layerMask;
    [Header("Interact limits")]
    public float interactRange = 2f;

    void Update() {
        GameObject hitObject = GetMouseHoverObject();
        Interactable interactable = UpdateInteractTooltip(hitObject);

        interactImage.SetActive(interactable);
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
            uiText.text = interactable.GetInteractableText();
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
