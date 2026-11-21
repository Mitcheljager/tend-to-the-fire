using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerInteract : MonoBehaviour {
    public Transform endTransform;
    [Header("Cursor & UI")]
    public Transform uiParent;
    public Image interactImage;
    public TMP_Text uiText;
    [Header("Mask")]
    public LayerMask layerMask;
    public int interactableLayerIndex = 0;
    public int interactableInRangeLayerIndex = 0;
    public int interactableSelectedLayerIndex = 0;
    [Header("Interact limits")]
    public float interactRange = 2f;
    public float sphereCastRadius = 0.1f;

    private Interactable lastSelectedInteractable;
    private InputAction interactInput;

    void Start() {
        interactInput = InputSystem.actions.FindAction("Interact");
    }

    void Update() {
        if (lastSelectedInteractable) {
            lastSelectedInteractable.SetLayer(interactableLayerIndex);
            lastSelectedInteractable = null;
        }

        GameObject hitObject = GetMouseHoverObject();
        Interactable interactable = UpdateInteractTooltip(hitObject);

        if (interactable) {
            lastSelectedInteractable = interactable;
            lastSelectedInteractable.SetLayer(interactableSelectedLayerIndex);
        }

        interactImage.gameObject.SetActive(interactable);
        if (interactable && interactInput!.WasPressedThisFrame()) interactable.Interact();

        Debug.DrawLine(transform.position, endTransform.position, interactable ? Color.green : Color.red);
    }

    void LateUpdate() {
        if (lastSelectedInteractable) lastSelectedInteractable.SetLayer(interactableSelectedLayerIndex);
    }

    private Interactable UpdateInteractTooltip(GameObject hitObject) {
        if (hitObject == null || (!CanInteract(hitObject))) {
            uiText.text = " ";
            uiText.gameObject.SetActive(false);
            return null;
        }

        if (!hitObject.TryGetComponent<Interactable>(out var interactable)) return null;

        uiParent.position = Camera.main.WorldToScreenPoint(hitObject.transform.position);

        uiText.gameObject.SetActive(true);
        uiText.text = interactable.GetInteractText();
        interactImage.sprite = interactable.GetInteractImage();

        return interactable;
    }

    public bool CanInteract(GameObject hitObject) {
        return hitObject && hitObject.CompareTag("Interactable");
    }

    public GameObject GetMouseHoverObject() {
        Vector3 position = transform.position;
        Vector3 target = position + transform.forward * interactRange;

        Physics.Raycast(position, transform.forward, out RaycastHit raycastHit, interactRange, layerMask);

        if (raycastHit.collider != null) {
            endTransform.position = raycastHit.point;
            return raycastHit.collider.gameObject;
        }

        Physics.SphereCast(position, sphereCastRadius, transform.forward, out RaycastHit spherecastHit, interactRange, layerMask);

        endTransform.position = spherecastHit.point;

		if (spherecastHit.collider == null) {
            endTransform.position = target;
            return null;
        }

        return spherecastHit.collider.gameObject;
	}
}
