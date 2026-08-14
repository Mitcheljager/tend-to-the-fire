using UnityEngine;
using TMPro;

[System.Serializable]
public class BigDialogItem {
    [TextArea] public string text;
}

public class BigDialog : MonoBehaviour {
    public TMP_Text text;
    public UIAnimationHelper uiAnimationHelper;
    public BigDialogItem[] items;

    private int currentItemIndex = 0;

    void OnEnable() {
        text.text = items[currentItemIndex].text;
    }

    void Update() {
        if (Input.GetButtonDown("Interact")) Next();
    }

    private void Next() {
        currentItemIndex++;

        if (currentItemIndex >= items.Length) {
            End();
            return;
        }

        text.text = items[currentItemIndex].text;
    }

    private void End() {
        gameObject.SetActive(false);
    }
}
