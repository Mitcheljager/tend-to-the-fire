using UnityEngine;
using UnityEngine.Events;
using TMPro;

[System.Serializable]
public class BigDialogItem {
    [TextArea] public string text;
}

public class BigDialog : MonoBehaviour {
    public TMP_Text text;
    public UIAnimationHelper uiAnimationHelper;
    public BigDialogItem[] items;
    [Header("Event")]
    public UnityEvent eventOnEnable;
    public UnityEvent eventOnEnd;

    private PlayerState playerState;
    private int currentItemIndex = 0;

    void OnEnable() {
        playerState = FindFirstObjectByType<PlayerState>();

        playerState.SetInStasis(true);
        text.text = items[currentItemIndex].text;

        eventOnEnable.Invoke();
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
        eventOnEnd.Invoke();
        playerState.SetInStasis(false);

        gameObject.SetActive(false);
    }
}
