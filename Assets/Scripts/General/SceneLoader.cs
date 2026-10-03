using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {
    public GameObject loadingScreenObject;

    public void ResetCurrentScene() {
        StartCoroutine(LoadSceneWithLoadingScreen(SceneManager.GetActiveScene().name));
    }

    public void LoadScene(string sceneName) {
        StartCoroutine(LoadSceneWithLoadingScreen(sceneName));
    }

    private IEnumerator LoadSceneWithLoadingScreen(string sceneName) {
        if (loadingScreenObject != null) loadingScreenObject.SetActive(true);

        yield return new WaitForSeconds(0);

        SceneManager.LoadSceneAsync(sceneName);
    }
}
