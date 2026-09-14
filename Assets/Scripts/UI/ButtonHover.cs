using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public TMP_Text textElement;
    public Color hoverColor = Color.red;
    public Vector3 hoverScale = Vector3.one;

    private Color baseColor;
    private Vector3 baseScale;

    private void Start() {
        baseColor = textElement.color;
        baseScale = transform.localScale;
    }

    void OnDisable() {
        Reset();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        textElement.color = hoverColor;
        transform.localScale = hoverScale;
    }

    public void OnPointerExit(PointerEventData eventData) {
        Reset();
    }

    private void Reset() {
        textElement.color = baseColor;
        transform.localScale = baseScale;
    }
}
