using UnityEngine;
using UnityEngine.UI;
using Unity.Collections;
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

    private GameObject hitObject;
    private Interactable hoveringInteractable;

    void Update() {
        hitObject = GetMouseHoverObject();

        hoveringInteractable = UpdateGrabberTooltip();

        interactImage.SetActive(hoveringInteractable);

        Debug.DrawLine(transform.position, endTransform.position, hoveringInteractable ? Color.green : Color.red);

        if (hoveringInteractable && Input.GetMouseButtonDown(0)) hoveringInteractable.Interact();
    }

    private Interactable UpdateGrabberTooltip() {
        if (hitObject == null || (!CanInteract())) {
            uiText.text = " ";
            uiText.gameObject.SetActive(false);
            return null;
        }

        if (!hitObject.TryGetComponent<Interactable>(out var interactable)) return null;
        if (!uiText.gameObject.activeInHierarchy) {
            uiText.gameObject.SetActive(true);
            uiText.text = interactable.interactText;
        }

        return interactable;
    }

    public bool CanInteract() {
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
