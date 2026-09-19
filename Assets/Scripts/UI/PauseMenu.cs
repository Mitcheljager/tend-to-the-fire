using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour {
    public bool isPaused = false;
    public GameObject pauseMenuObject;
    public UIScreenManager UIscreenManager;
    public UIScreen initialUIScreen;
    public AudioSource[] pauseAudioSourcesOnShow;

    private InputAction pauseInput;

    void Start() {
        pauseInput = InputSystem.actions.FindAction("Pause");
    }

    void Update() {
        if (pauseInput.WasPressedThisFrame()) TogglePause();
    }

    public void TogglePause() {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;

        pauseMenuObject.SetActive(isPaused);

        if (isPaused) {
            UIscreenManager.ShowScreen(initialUIScreen);
            Cursor.lockState = CursorLockMode.None;
        } else {
            Cursor.lockState = CursorLockMode.Locked;
        }

        ToggleAudioSources();
    }

    private void ToggleAudioSources() {
        foreach (AudioSource audioSource in pauseAudioSourcesOnShow) {
            if (isPaused) audioSource.Pause();
            else audioSource.UnPause();
        }
    }
}
