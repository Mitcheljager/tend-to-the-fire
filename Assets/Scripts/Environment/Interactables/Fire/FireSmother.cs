using UnityEngine;

public class FireSmother : MonoBehaviour {
    [Header("Config")]
    public Fire fire;
    public float smotherLossPerSecond = 0.5f;
    [Header("State")]
    [Fade][Range(0f, 1f)] public float currentSmother = 0f;

    void Update() {
        if (currentSmother <= 0f) return;
        if (currentSmother == 1f) return;

        DecreaseCurrentSmother();
    }

    public void AddSmother(Fuel fuel) {
        currentSmother = Mathf.Min(currentSmother + fuel.smotherIncrease, 1f);
    }

    private void DecreaseCurrentSmother() {
        currentSmother = Mathf.Max(currentSmother - (smotherLossPerSecond * Time.deltaTime), 0f);
    }
}
