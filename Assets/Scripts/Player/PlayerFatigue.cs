using UnityEngine;

public class PlayerFatigue : MonoBehaviour {
    [Range(0f, 1f)] public float maxFatigue = 0.5f;
    [Header("State")]
    [Fade] public float currentFatigue = 0f;
}
