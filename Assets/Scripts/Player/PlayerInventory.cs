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
        fuel.transform.parent = transform;
        fuel.transform.localPosition = Vector3.zero;
    }

    public void UseFuel(Fuel fuel, Fire fire) {
        carryingFuel.Remove(fuel);

        fuel.mesh.SetActive(true);
        fuel.transform.parent = fire.transform;
        fuel.transform.localPosition = Vector3.zero;

        fire.activeFuel.Add(fuel);
    }
}
