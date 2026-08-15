using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using TMPro;

[System.Serializable]
public class BigDialogItem {
    [TextArea] public string text;

    public BigDialogItem(string _text) => text = _text;
}

public class BigDialog : MonoBehaviour {
    public TMP_Text text;
    public List<BigDialogItem> items;
    [Header("Animation")]
    public float textFadeTimeSeconds = 0.5f;
    public UIAnimationHelper uiAnimationHelper;
    public UIAnimationHelper textUiAnimationHelper;
    [Header("Event")]
    public UnityEvent eventOnEnable;
    public UnityEvent eventOnEnd;

    private PlayerState playerState;
    private int currentItemIndex = 0;

    void Awake() {
        items.Insert(0, new(""));
    }

    void OnEnable() {
        playerState = FindFirstObjectByType<PlayerState>();

        playerState.SetInStasis(true);
        text.text = items[currentItemIndex].text;

        eventOnEnable.Invoke();

        Next();
    }

    void Update() {
        if (Input.GetButtonDown("Interact")) Next();
    }

    private void Next() {
        currentItemIndex++;

        if (currentItemIndex >= items.Count) {
            End();
            return;
        }

        textUiAnimationHelper.FadeIn(textFadeTimeSeconds);

        text.text = items[currentItemIndex].text;
    }

    private void End() {
        eventOnEnd.Invoke();
        playerState.SetInStasis(false);

        gameObject.SetActive(false);
    }
}
