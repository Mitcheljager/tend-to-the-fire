using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UIAnimationHelper : MonoBehaviour {
    public CanvasGroup canvasGroup;
    public RectTransform rectTransform;

    [Header("Animation Clip (Optional)")]
    public GameObject UIObject;
    public Animation UIAnimation;
    public bool hideOnStart = true;
    public string showUIAnimationName = "Grow Image";
    public string hideUIAnimationName = "Shrink Image";

    private bool isAnimating = false;

    void Start() {
        if (UIObject != null && hideOnStart) UIObject.SetActive(false);
    }

    public void FlyIn(float distanceX = 0f, float distanceY = 0f, float duration = 0f) {
        if (isAnimating) return;

        StartCoroutine(FlyInCoroutine(distanceX, distanceY, duration));
    }

    public void SlideIn(float targetHeight = 40f, float duration = 0.5f) {
        if (isAnimating) return;

        StartCoroutine(SlideInCoroutine(targetHeight, duration));
    }

    public void FadeOut(float duration = 0.5f) {
        if (isAnimating) return;

        StopAllCoroutines();
        StartCoroutine(FadeOutCoroutine(duration));
    }

    public void FadeIn(float duration = 0.5f) {
        if (isAnimating) return;

        StartCoroutine(FadeInCoroutine(duration));
    }

    public void PlayShowUIAnimationClip() {
        UIObject.SetActive(true);
        UIAnimation.Play(showUIAnimationName);
    }

    public void PlayHideUIAnimationClip() {
        if (!UIObject.activeSelf) return;

        StartCoroutine(PlayHideUIAnimationClipCoroutine());
    }

    private IEnumerator FlyInCoroutine(float distanceX, float distanceY, float duration) {
        isAnimating = true;
        float timer = 0f;

        canvasGroup.alpha = 0;

        Vector2 startPosition = rectTransform.anchoredPosition;
        float startX = startPosition.x;
        float startY = startPosition.y;
        float targetX = startX - distanceX;
        float targetY = startY - distanceY;
        rectTransform.anchoredPosition = startPosition;

        while (timer < duration) {
            float currentAlpha = Mathf.Lerp(0f, 1f, timer / duration);
            float currentTop = Mathf.Lerp(startY, targetY, timer / duration);

            canvasGroup.alpha = currentAlpha;

            Vector2 currentPosition = rectTransform.anchoredPosition;
            currentPosition.x = startX - currentTop;
            currentPosition.y = startY - currentTop;
            rectTransform.anchoredPosition = currentPosition;

            timer += Time.deltaTime;

            yield return null;
        }

        canvasGroup.alpha = 1f;
        Vector2 finalPosition = rectTransform.anchoredPosition;
        finalPosition.x = startPosition.x - targetX;
        finalPosition.y = startPosition.y - targetY;
        rectTransform.anchoredPosition = finalPosition;

        isAnimating = false;
    }

    private IEnumerator SlideInCoroutine(float targetHeight, float duration) {
        isAnimating = true;
        float timer = 0f;

        canvasGroup.alpha = 0;

        Vector2 sizeDelta = rectTransform.sizeDelta;
        sizeDelta.y = 0;
        rectTransform.sizeDelta = sizeDelta;

        while (timer < duration) {
            float currentAlpha = Mathf.Lerp(0f, 1f, timer / duration);
            float currentHeight = Mathf.Lerp(0f, targetHeight, timer / duration);

            canvasGroup.alpha = currentAlpha;

            sizeDelta = rectTransform.sizeDelta;
            sizeDelta.y = currentHeight;
            rectTransform.sizeDelta = sizeDelta;

            timer += Time.deltaTime;

            yield return null;
        }

        canvasGroup.alpha = 1f;
        Vector2 finalSizeDelta = rectTransform.sizeDelta;
        finalSizeDelta.y = targetHeight;
        rectTransform.sizeDelta = finalSizeDelta;
        isAnimating = false;
    }

    private IEnumerator FadeOutCoroutine(float duration) {
        isAnimating = true;
        float timer = 0f;

        canvasGroup.alpha = 1f;

        while (timer < duration) {
            float currentAlpha = Mathf.Lerp(1f, 0f, timer / duration);
            canvasGroup.alpha = currentAlpha;
            timer += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0f;
        isAnimating = false;
    }

    private IEnumerator FadeInCoroutine(float duration) {
        isAnimating = true;
        float timer = 0f;

        canvasGroup.alpha = 0;

        while (timer < duration) {
            float currentAlpha = Mathf.Lerp(0f, 1f, timer / duration);
            canvasGroup.alpha = currentAlpha;
            timer += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 1f;
        isAnimating = false;
    }

    private IEnumerator PlayHideUIAnimationClipCoroutine() {
        UIAnimation.Play(hideUIAnimationName);

        yield return new WaitForSeconds(0.25f);

        UIObject.SetActive(false);

        yield return null;
    }
}
