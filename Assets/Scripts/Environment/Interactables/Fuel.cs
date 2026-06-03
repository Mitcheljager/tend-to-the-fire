using UnityEngine;

public class Fuel : Interactable {
    public float maxFuel = 10f;
    public float weight = 0f;
    public Renderer[] materialRenderers;
    [Header("State")]
    [Fade] public float currentFuel = 0;
    [Fade] [Range(0f, 1f)] public float currentFuelNormalized = 1f;

    private PlayerInventory playerInventory;

    void OnDrawGizmos() {
        if (this.enabled) Gizmos.DrawIcon(transform.position, "fuel.png", false);
    }

    void Start() {
        playerInventory = FindAnyObjectByType<PlayerInventory>();

        currentFuel = maxFuel;
    }

    public override void Interact() {
        Debug.Log("Pick up");

        playerInventory.PickUpFuel(this);
    }

    public void DecreaseCurrentFuel(float amount) {
        currentFuel -= amount;
        currentFuelNormalized = 1 / maxFuel * currentFuel;

        if (currentFuel <= 0) DestroyFuel();

        SetMaterial();
    }

    public void DestroyFuel() {
        Destroy(gameObject);
    }

    private void SetMaterial() {
        float multiplier = 1f / maxFuel * currentFuel;

        if (materialRenderers == null) return;

        foreach (Renderer renderer in materialRenderers) {
            renderer.material.SetFloat("_Current_Value", 1f - multiplier);
        }
    }
}
