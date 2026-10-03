using UnityEngine;

public class StaggerShowOnAwake : MonoBehaviour {
    public int framesBetweenStagger = 10;
    public GameObject[] staggeredObjects;

    private int currentObject = 0;
    private int currentFrame = 0;

    void Awake() {
        foreach(GameObject staggeredObject in staggeredObjects) {
            staggeredObject.SetActive(false);
        }
    }

    void Update() {
        if (currentFrame > currentObject * framesBetweenStagger) {
            staggeredObjects[currentObject].SetActive(true);
            currentObject++;
        }

        currentFrame++;

        if (currentObject >= staggeredObjects.Length) this.enabled = false;
    }
}
