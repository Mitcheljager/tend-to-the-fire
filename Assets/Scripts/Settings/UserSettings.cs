using UnityEngine;

public class UserSettings : MonoBehaviour {
    public void OnEnable() {
        ChangeEvent.OnChangeEvent.AddListener(PossibilyUpdateFromEvent);
    }

    public void OnDisable() {
        ChangeEvent.OnChangeEvent.RemoveListener(PossibilyUpdateFromEvent);
    }

    public virtual void PossibilyUpdateFromEvent(SettingsKey key) {

    }
}
