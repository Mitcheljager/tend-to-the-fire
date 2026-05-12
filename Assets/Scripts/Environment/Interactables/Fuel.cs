using UnityEngine;

public class Fuel : Interactable {
    public float maxFuel = 10f;
    public GameObject mesh;
    [Header("State")]
    [Fade] public float currentFuel = 0;

    private PlayerInventory playerInventory;

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
