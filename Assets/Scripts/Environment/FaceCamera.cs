using UnityEngine;

public class FaceCamera : MonoBehaviour {
    public bool keepY = false;

    private void LateUpdate() {
        Vector3 position = Camera.main.transform.position;
        if (keepY) position.y = transform.position.y;

        transform.LookAt(position);
    }
}
