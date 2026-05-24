using UnityEngine;

public class Fuel : Interactable {
    public float maxFuel = 10f;
    public float weight = 0f;
    [Header("State")]
    [Fade] public float currentFuel = 0;

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

        if (currentFuel <= 0) DestroyFuel();
    }

    public void DestroyFuel() {
        Destroy(gameObject);
    }
}
