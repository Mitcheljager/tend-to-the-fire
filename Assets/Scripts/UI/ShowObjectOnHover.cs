using UnityEngine;
using UnityEngine.EventSystems;

public class ShowObjectOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public GameObject hoverObject;

    void OnEnable() {
        hoverObject.SetActive(false);
    }

    void OnDisable() {
        hoverObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        hoverObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (!eventData.fullyExited) return;
        hoverObject.SetActive(false);
    }
}
