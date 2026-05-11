using UnityEngine;

public class GameObjectHelper : MonoBehaviour {
    public void ToggleActive() {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
