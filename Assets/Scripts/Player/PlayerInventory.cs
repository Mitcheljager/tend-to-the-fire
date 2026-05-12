using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour {
    public List<Fuel> carryingFuel;

    public bool IsCarryingAnyFuel() {
        return carryingFuel.Count > 0;
    }
}
