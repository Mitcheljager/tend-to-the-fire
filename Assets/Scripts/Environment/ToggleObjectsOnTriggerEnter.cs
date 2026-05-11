using UnityEngine;

[RequireComponent(typeof(BoxColliderGizmos))]
public class ToggleObjectsOnTriggerEnter : MonoBehaviour {
    [Header("Config")]
    public bool once = false;

    [Header("Objects")]
    public GameObject[] objectsToEnable;
    public GameObject[] objectsToDisable;

    void OnDrawGizmos() {
        if (this.enabled) Gizmos.DrawIcon(transform.position, "detection.png", false);
    }

    void OnTriggerEnter(Collider collider) {
        if (!collider.CompareTag("Player")) return;

        Activate();
    }

    public void Activate() {
        foreach (GameObject item in objectsToEnable) item.SetActive(true);
        foreach (GameObject item in objectsToDisable) item.SetActive(false);

        if (once) gameObject.SetActive(false);
    }
}
