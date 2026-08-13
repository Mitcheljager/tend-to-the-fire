using UnityEngine;

public class ApplyRandomScaleOnStart : MonoBehaviour {
    [Header("Per component")]
    public Vector3 minimumScale = new(0.9f, 0.9f, 0.9f);
    public Vector3 maximumScale = new(1.1f, 1.1f, 1.1f);
    [Header("Uniform")]
    public bool uniform = false;
    public Vector2 scaleRange = new(0.9f, 1.1f);

    void Start() {
        if (uniform) {
            float random = Random.Range(scaleRange.x, scaleRange.y);
            Debug.Log(random);
            transform.localScale = transform.localScale * random;
        } else {
            transform.localScale = new(
                Random.Range(minimumScale.x, maximumScale.x),
                Random.Range(minimumScale.y, maximumScale.y),
                Random.Range(minimumScale.z, maximumScale.z)
            );
        }
    }
}
