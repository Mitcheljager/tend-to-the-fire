using UnityEngine;

public class ApplyRandomScaleOnStart : MonoBehaviour {
    [Header("Per component")]
    public Vector3 minimumScale = new(0.9f, 0.9f, 0.9f);
    public Vector3 maximumScale = new(1.1f, 1.1f, 1.1f);
    [Header("Uniform")]
    public bool uniform = false;
    public Vector2 uniformScaleRange = new(0.9f, 1.1f);

    void Start() {
        if (uniform) {
            transform.localScale = transform.localScale * Random.Range(uniformScaleRange.x, uniformScaleRange.y);
        } else {
            transform.localScale = new(
                Random.Range(minimumScale.x, maximumScale.x),
                Random.Range(minimumScale.y, maximumScale.y),
                Random.Range(minimumScale.z, maximumScale.z)
            );
        }
    }
}
