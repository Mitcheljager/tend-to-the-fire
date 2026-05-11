using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerState : MonoBehaviour {
    public static PlayerState Instance { get; private set; }
    [Header("State")]
    public bool isDead = false;
    [Header("Animation")]
    public Transform rightHandTransform;

    private Rigidbody playerRigidbody;

    void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start () {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {

    }

    public void Kill() {
        isDead = true;
        playerRigidbody.isKinematic = true;

        PlayerDeathEvent.Dispatch();
    }
}
