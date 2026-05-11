using UnityEngine;
using UnityEngine.Events;

public class ChangeEvent : MonoBehaviour {
    public static UnityEvent<SettingsKey> OnChangeEvent = new();

    public static void Dispatch(SettingsKey key) {
        OnChangeEvent.Invoke(key);
    }
}
