using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PauseMenu : MonoBehaviour {
    public bool pauseable = true;
    public bool isPaused = false;
    public GameObject pauseMenuObject;
    public UIScreenManager UIscreenManager;

    [Header("State")]
    [Fade] public List<AudioSource> playingAudioSourcesOnPause;

    private InputAction pauseInput;

    void Start() {
        pauseInput = InputSystem.actions.FindAction("Pause");
    }

    void Update() {
        if (pauseInput.WasPressedThisFrame()) TogglePause();
    }

    public void TogglePause() {
        if ((isPaused || !pauseable) && !UIscreenManager.IsOnInitialUIScreen()) {
            UIscreenManager.Reset();
            return;
        }

        if (!pauseable) return;

        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;

        pauseMenuObject.SetActive(isPaused);

        if (isPaused) {
            UIscreenManager.Reset();
            Cursor.lockState = CursorLockMode.None;
        } else {
            Cursor.lockState = CursorLockMode.Locked;
        }

        ToggleAudioSources();
    }

    public void Quit() {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    private void ToggleAudioSources() {
        if (isPaused) {
            playingAudioSourcesOnPause.Clear();

            foreach (AudioSource audioSource in GameObject.FindObjectsByType<AudioSource>(FindObjectsSortMode.None)) {
                if (audioSource.isPlaying) playingAudioSourcesOnPause.Add(audioSource);
            }
        }

        foreach (AudioSource audioSource in playingAudioSourcesOnPause) {
            if (isPaused) audioSource.Pause();
            else audioSource.UnPause();
        }
    }
}
