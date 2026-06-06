using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerSlow : MonoBehaviour {
    [Fade] public List<SlowPlayerWhileInside> slowZones;

    void Start() {
        slowZones = FindObjectsByType<SlowPlayerWhileInside>(FindObjectsSortMode.None).ToList();
    }

    public bool IsPlayerBeingSlowed() {
        return slowZones.Any(slowZone => slowZone.isPlayerInside);
    }
}
