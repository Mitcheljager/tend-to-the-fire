using UnityEngine;

public class AlignToGround : MonoBehaviour {
    public LayerMask layerMask;
    public int slerpSpeed = 10;

    void Update() {
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, layerMask)) {
            Quaternion slopeRotation = Quaternion.FromToRotation(transform.up, hit.normal);
            transform.rotation = Quaternion.Slerp(transform.rotation, slopeRotation * transform.rotation, slerpSpeed * Time.deltaTime);
        }
    }
}
