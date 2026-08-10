using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class SmallMessage : MonoBehaviour {
    public TMP_Text text;
    public UIAnimationHelper uiAnimationHelper;
    public float showForSeconds = 5f;
    public float fadeSeconds = 1f;
    public List<string> messageQueue = new();

    private bool isCoroutineRunning = false;

    void OnEnable() {
        text.text = "";
        MessageEvent.OnShowSmallMessage.AddListener(AddMessageToQueue);
    }

    void OnDisable() {
        MessageEvent.OnShowSmallMessage.RemoveListener(AddMessageToQueue);
    }

    private void AddMessageToQueue(string message) {
        messageQueue.Add(message);

        if (!isCoroutineRunning) StartCoroutine(ShowNextMessage());
    }

    private IEnumerator ShowNextMessage() {
        if (isCoroutineRunning) yield break;
        if (messageQueue.Count == 0) yield break;

        isCoroutineRunning = true;

        text.text = messageQueue[0];

        uiAnimationHelper.FadeIn(fadeSeconds);

        yield return new WaitForSeconds(showForSeconds + fadeSeconds);

        uiAnimationHelper.FadeOut(fadeSeconds);

        yield return new WaitForSeconds(fadeSeconds);

        messageQueue.RemoveAt(0);
        isCoroutineRunning = false;

        StartCoroutine(ShowNextMessage());
    }
}
