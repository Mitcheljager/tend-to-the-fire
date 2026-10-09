using UnityEngine;
using UnityEngine.InputSystem;

public class FireMainMenu : MonoBehaviour {
    public Fire fire;
    public GameObject[] fuelPrefabs;
    [Header("Burst")]
    public Vector2 burstEmbersRange;
    public float burstCooldownSeconds = 0.5f;

    private float currentBurstCooldown = 0f;

    void Update() {
        if (fire.currentMultiplier < 0.75) {
            fire.AddFuelFromPrefab(fuelPrefabs[^1]);
        }

        if (currentBurstCooldown > 0f) {
            currentBurstCooldown -= Time.deltaTime;
            return;
        }

        if (Mouse.current.leftButton.wasPressedThisFrame) {
            BurstFireOnClick();
            currentBurstCooldown = burstCooldownSeconds;
        }
    }

    private void BurstFireOnClick() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit hit)) return;
        if (hit.collider.gameObject != fire.gameObject) return;

        fire.fireEffects.BurstEmbers(Random.Range((int) burstEmbersRange.x, (int) burstEmbersRange.y));
    }
}
