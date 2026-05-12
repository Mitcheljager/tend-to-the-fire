using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour {
    public List<Fuel> carryingFuel;

    public bool IsCarryingAnyFuel() {
        return carryingFuel.Count > 0;
    }

    public void PickUpFuel(Fuel fuel) {
        carryingFuel.Add(fuel);

        fuel.mesh.SetActive(false);
        fuel.transform.position = Vector3.zero;
        fuel.transform.parent = transform.parent;
    }
}
