using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInventory : MonoBehaviour {
    public float maxWeight = 50f;
    public List<Fuel> carryingFuel;
    public LayerMask dropFloorCheckMask;
    public float dropRandomPositionRange = 0.25f;
    [Header("State")]
    [Fade] public float currentWeight = 0f;

    void Update() {
        currentWeight = GetCurrentWeight();

        if (Input.GetButtonDown("Drop")) DropAllFuel();
    }

    public bool IsCarryingAnyFuel() {
        return carryingFuel.Count > 0;
    }

    public void PickUpFuel(Fuel fuel) {
        carryingFuel.Add(fuel);

        SetFuelMeshesActive(fuel, false);

        fuel.transform.parent = transform;
        fuel.transform.localPosition = Vector3.zero;
    }

    public void UseFuel(Fuel fuel, Fire fire) {
        carryingFuel.Remove(fuel);

        SetFuelMeshesActive(fuel, true);

        fuel.transform.parent = fire.transform;
        fuel.transform.localPosition = Vector3.zero;

        fire.activeFuel.Add(fuel);
    }

    public float GetCurrentWeight() {
        return carryingFuel.Sum(fuel => fuel.weight);
    }

    public void DropAllFuel() {
        Physics.Raycast(transform.position, Vector3.down, out RaycastHit floorHit, 10f, dropFloorCheckMask);
        Vector3 floorPosition = floorHit.point;

        foreach(Fuel fuel in carryingFuel) {
            fuel.transform.parent = null;

            fuel.transform.position = new(
                floorPosition.x + Random.Range(-dropRandomPositionRange, dropRandomPositionRange),
                floorPosition.y,
                floorPosition.z + Random.Range(-dropRandomPositionRange, dropRandomPositionRange)
            );

            SetFuelMeshesActive(fuel, true);
        }

        carryingFuel.Clear();
    }

    private void SetFuelMeshesActive(Fuel fuel, bool state) {
        foreach(GameObject mesh in fuel.meshes) mesh.SetActive(state);
    }
}
