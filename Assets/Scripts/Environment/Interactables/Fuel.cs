using UnityEngine;

public class Fuel : Interactable {
    public float maxFuel = 10f;
    [Header("State")]
    [Fade] public float currentFuel = 0;

    void Start() {
        currentFuel = maxFuel;
    }

    public override void Interact() {
        Debug.Log("Pick up");
    }

    public void DecreaseCurrentFuel(float amount) {
        currentFuel -= amount;

        if (currentFuel <= 0) DestroyFuel();
    }

    public void DestroyFuel() {
        Destroy(gameObject);
    }
}
