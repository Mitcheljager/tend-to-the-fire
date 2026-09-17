using UnityEngine;

public class RadioButton : MonoBehaviour {
    public int value = 0;
    public GameObject[] activeObjects;
    public bool active = false;

    private RadioButtonGroup radioButtonGroup;

    void Start() {
        SetState(active);

        radioButtonGroup = GetComponentInParent<RadioButtonGroup>();
    }

    public void Change() {
        radioButtonGroup.ToggleRadioButtons(value);
    }

    public void SetState(bool state) {
        active = state;

        foreach (GameObject activeObject in activeObjects) {
            activeObject.SetActive(active);
        }
    }
}
