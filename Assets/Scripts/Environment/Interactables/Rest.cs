using UnityEngine;

public class Rest : Interactable {
    [Separator]
    public Transform cameraPosition;

    private PlayerRest playerRest;

    void OnDrawGizmos() {
        if (this.enabled) Gizmos.DrawIcon(transform.position, "rest.png", false);

        if (cameraPosition == null) return;

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(cameraPosition.position, 0.025f);
        Gizmos.DrawLine(cameraPosition.position, cameraPosition.position + cameraPosition.forward * 0.25f);
    }

    void Awake() {
        playerRest = FindFirstObjectByType<PlayerRest>();
    }

    public override void Interact() {
        playerRest.SetResting(true, cameraPosition);
    }
}
