using UnityEngine;

public class RotateOverTime : MonoBehaviour {
    public float rotationsPerMinute = 10f;

    void Update() {
        transform.Rotate(0, 6f * rotationsPerMinute * Time.deltaTime, 0f);
    }
}
